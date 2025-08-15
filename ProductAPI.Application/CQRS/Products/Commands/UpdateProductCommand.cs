using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.DTOs.ProductDtos;
using ProductAPI.Application.Services;
using ProductAPI.Core.Entities;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Application.CQRS.Products.Commands
{
    public record UpdateProductCommand(Guid Id, ProductUpdateRequestDto Request) : IRequest<ProductResponseDto>;

    public class UpdateProductCommandHandler(IGenericRepository<Product> repo, ICacheService cache, IMapper mapper, ILogger<UpdateProductCommand> logger) : IRequestHandler<UpdateProductCommand, ProductResponseDto>
    {
        private readonly IGenericRepository<Product> _repo = repo;
        private readonly ICacheService _cache = cache;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateProductCommand> _logger = logger;

        public async Task<ProductResponseDto> Handle(UpdateProductCommand command, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(command.Id)
                ?? throw new KeyNotFoundException("Product not found");

            entity.Name = command.Request.Name;
            entity.Description = command.Request.Description;
            entity.Price = command.Request.Price;

            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            _logger.LogInformation("Product with ID {Id} updated successfully.", command.Id);

            await _cache.RemoveAsync("products:all");

            return _mapper.Map<ProductResponseDto>(entity);
        }
    }
}
