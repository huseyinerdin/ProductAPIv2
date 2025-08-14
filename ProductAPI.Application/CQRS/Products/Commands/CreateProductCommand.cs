using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Services;
using ProductAPI.Core.Entities;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Application.CQRS.Products.Commands
{
    public record CreateProductCommand(ProductCreateRequestDto Request) : IRequest<ProductResponseDto>;

    public class CreateProductCommandHandler(IGenericRepository<Product> repo, ICacheService cache, IMapper mapper, ILogger<CreateProductCommand> logger) : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {
        private readonly IGenericRepository<Product> _repo = repo;
        private readonly ICacheService _cache = cache;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateProductCommand> _logger = logger;

        public async Task<ProductResponseDto> Handle(CreateProductCommand command, CancellationToken ct)
        {
            var entity = _mapper.Map<Product>(command.Request);

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            _logger.LogInformation("Product created with ID: {Id}", entity.Id);

            await _cache.RemoveAsync("products:all");

            return _mapper.Map<ProductResponseDto>(entity);
        }
    }
}
