using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.DTOs.ProductDtos;
using ProductAPI.Application.Services;
using ProductAPI.Core.Entities;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Application.CQRS.Products.Queries
{
    public record GetProductsQuery() : IRequest<IEnumerable<ProductResponseDto>>;
    public class GetProductsQueryHandler(IGenericRepository<Product> repo, ICacheService cache, IMapper mapper, ILogger<GetProductsQuery> logger) : IRequestHandler<GetProductsQuery, IEnumerable<ProductResponseDto>>
    {
        private const string CacheKey = "products:all";
        private static readonly TimeSpan ExpireTimeSpan = TimeSpan.FromMinutes(5);

        private readonly IGenericRepository<Product> _repo = repo;
        private readonly ICacheService _cache = cache;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetProductsQuery> _logger = logger;

        public async Task<IEnumerable<ProductResponseDto>> Handle(GetProductsQuery request, CancellationToken ct)
        {
            var cached = await _cache.GetAsync<IEnumerable<ProductResponseDto>>(CacheKey);
            if (cached is not null)
            {
                _logger.LogInformation("Products fetched from cache.");
                return cached;
            }

            _logger.LogInformation("Products fetched from database.");
            var list = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<ProductResponseDto>>(list);
            await _cache.SetAsync(CacheKey, dto, ExpireTimeSpan);

            return dto;
        }
    }
}
