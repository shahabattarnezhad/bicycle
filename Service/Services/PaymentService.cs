using Application.Contracts.Base;
using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Auth;
using Service.Services.Helpers;
using Shared.Consts;
using Shared.DTOs.Payment;
using Shared.DTOs.Station;
using Shared.Helpers;
using Shared.Requests;
using Shared.Responses;

namespace Service.Services;

internal sealed class PaymentService : IPaymentService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCacheService _cache;
    private readonly IUserContextService _userContextService;

    public PaymentService(IRepositoryManager repository, IMapper mapper, IMemoryCacheService cache, IUserContextService userContextService)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _userContextService = userContextService;
    }

    public async Task<ApiResponse<IEnumerable<PaymentDto>>> GetAllAsync(PaymentParameters parameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var prefix = PaymentCacheKeyHelper.PaymentPrefix;
        var cacheKey =
            PaymentCacheKeyHelper.GetPagedKey(parameters.PageNumber, parameters.PageSize);

        var pagedPayments = await _cache.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                return await _repository.Payment.GetAllAsync(parameters, trackChanges, cancellationToken);
            },
            TimeSpan.FromMinutes(30),
            CacheKeyPrefixes.Payment
        );

        var entitiesDto =
            _mapper.Map<IEnumerable<PaymentDto>>(pagedPayments);

        return new ApiResponse<IEnumerable<PaymentDto>>(entitiesDto, "Payments retrieved successfully", pagedPayments.Count());
    }

    public async Task<ApiResponse<PaymentDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var prefix = PaymentCacheKeyHelper.PaymentPrefix;
        var cacheKey = PaymentCacheKeyHelper.GetEntityKey(entityId);

        var entity = await _cache.GetOrCreateAsync(
            cacheKey,
            async () => await FindEntity(entityId, trackChanges, cancellationToken),
            TimeSpan.FromMinutes(10),
            CacheKeyPrefixes.Payment
        );

        var entityDto =
            _mapper.Map<PaymentDto>(entity);

        return new ApiResponse<PaymentDto>(entityDto, "Payment retrieved successfully");
    }

    public async Task<ApiResponse<PaymentDto>> CreateAsync(PaymentForCreationDto entityForCreation, CancellationToken cancellationToken = default)
    {
        var userId = _userContextService.UserId;

        var entity =
            _mapper.Map<Payment>(entityForCreation);

        entity.AppUserId = userId;

        _repository.Payment.CreateEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix(PaymentCacheKeyHelper.PaymentPrefix);

        var entityToReturn =
            _mapper.Map<PaymentDto>(entity);

        return new ApiResponse<PaymentDto>(entityToReturn, "Payment created successfully");
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity =
            await FindEntity(entityId, trackChanges, cancellationToken);

        _repository.Payment.DeleteEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        _cache.Remove(PaymentCacheKeyHelper.GetEntityKey(entityId));
        _cache.RemoveByPrefix(PaymentCacheKeyHelper.PaymentPrefix);

        return new ApiResponse<string>(null, "Payment deleted successfully");
    }

    private async Task<Payment> FindEntity(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var payment =
            await _repository.Payment.GetAsync(entityId, trackChanges, cancellationToken);

        return payment ?? throw new EntityNotFoundException(nameof(payment), entityId);
    }
}
