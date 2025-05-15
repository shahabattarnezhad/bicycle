using Application.Contracts.Base;
using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Auth;
using Service.Contracts.Interfaces.Helpers;
using Shared.DTOs.Document;
using Shared.Enums;
using Shared.Helpers;
using Shared.Requests;
using Shared.Responses;

namespace Service.Services;

internal sealed class DocumentService : IDocumentService
{
    private readonly IRepositoryManager _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly IMemoryCacheService _cache;
    private readonly IUserContextService _userContextService;

    public DocumentService(
        IRepositoryManager repository,
        IMapper mapper,
        IMemoryCacheService cache,
        IFileService fileService,
        IUserContextService userContextService)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _fileService = fileService;
        _userContextService = userContextService;
    }

    public async Task<ApiResponse<IEnumerable<DocumentDto>>> GetAllAsync(DocumentParameters parameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //var prefix = DocumentCacheKeyHelper.DocumentPrefix;
        //var cacheKey = DocumentCacheKeyHelper.GenerateDocumentListKey(parameters);

        //var pagedDocuments = await _cache.GetOrCreateAsync(
        //    cacheKey,
        //    async () =>
        //    {
        //        return await _repository.Document.GetAllAsync(parameters, trackChanges, cancellationToken);
        //    },
        //    TimeSpan.FromMinutes(10),
        //    prefix
        //);

        var pagedDocuments =
            await _repository.Document.GetAllAsync(parameters, trackChanges, cancellationToken);

        var entitiesDto =
            _mapper.Map<IEnumerable<DocumentDto>>(pagedDocuments);

        return new ApiResponse<IEnumerable<DocumentDto>>(entitiesDto, "Documents retrieved successfully", pagedDocuments.Count());
    }


    public async Task<ApiResponse<DocumentDto>> CreateAsync(DocumentForCreationDto entityForCreation, CancellationToken cancellationToken = default)
    {
        var userId = _userContextService.UserId;

        var imagePath =
            await _fileService.SaveFileAsync(entityForCreation.DocumentFile);

        var entity = new Document
        {
            Path = imagePath,
            AppUserId = userId,
            UploadedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            Status = DocumentStatus.Pending
        };

        _repository.Document.CreateEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        //_cache.RemoveByPrefix(DocumentCacheKeyHelper.DocumentPrefix);

        var entityToReturn =
            _mapper.Map<DocumentDto>(entity);

        return new ApiResponse<DocumentDto>(entityToReturn, "Document created successfully");
    }


    public async Task<ApiResponse<DocumentDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //var cacheKey = DocumentCacheKeyHelper.GenerateDocumentKey(entityId);

        //var entity = await _cache.GetOrCreateAsync(
        //    cacheKey,
        //    async () => await FindEntity(entityId, trackChanges, cancellationToken),
        //    TimeSpan.FromMinutes(10),
        //    DocumentCacheKeyHelper.DocumentPrefix
        //);

        var entity =
            await FindEntity(entityId, trackChanges, cancellationToken);

        var entityDto =
            _mapper.Map<DocumentDto>(entity);

        return new ApiResponse<DocumentDto>(entityDto, "Document retrieved successfully");
    }


    public async Task<ApiResponse<string>> UpdateAsync(Guid entityId, DocumentForVerificationDto entityForUpdation, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity =
           await FindEntity(entityId, trackChanges, cancellationToken);

        _mapper.Map(entityForUpdation, entity);

        await _repository.SaveAsync(cancellationToken);

        //_cache.Remove(DocumentCacheKeyHelper.GenerateDocumentKey(entityId));
        //_cache.RemoveByPrefix(DocumentCacheKeyHelper.DocumentPrefix);

        return new ApiResponse<string>(null, "Document updated successfully");
    }

    private async Task<Document> FindEntity(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var document =
            await _repository.Document.GetAsync(entityId, trackChanges, cancellationToken);

        return document ?? throw new EntityNotFoundException(nameof(document), entityId);
    }
}
