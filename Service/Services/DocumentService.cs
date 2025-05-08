using Application.Contracts.Base;
using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Helpers;
using Shared.DTOs.Document;
using Shared.Responses;
using System.Security.Claims;

namespace Service.Services;

internal sealed class DocumentService : IDocumentService
{
    private readonly IRepositoryManager _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly IMemoryCacheService _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DocumentService(
        IRepositoryManager repository,
        IMapper mapper,
        IMemoryCacheService cache,
        IFileService fileService,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _fileService = fileService;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<ApiResponse<DocumentDto>> CreateAsync(DocumentForCreationDto entityForCreation, CancellationToken cancellationToken = default)
    {
        string? userId = GetCurrentUserId();

        var imagePath =
            await _fileService.SaveFileAsync(entityForCreation.DocumentFile);

        var entity = new Document
        {
            //Id = Guid.NewGuid(),
            Path = imagePath,
            AppUserId = userId,
            UploadedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _repository.Document.CreateEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        var entityToReturn =
            _mapper.Map<DocumentDto>(entity);

        return new ApiResponse<DocumentDto>(entityToReturn, "Document created successfully");
    }

    private string GetCurrentUserId()
    {
        var userId =
                    _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new UserNotFoundException("User not found");
        return userId;
    }
}
