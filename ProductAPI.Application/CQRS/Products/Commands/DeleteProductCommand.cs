using MediatR;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.Services;
using ProductAPI.Core.Entities;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Application.CQRS.Products.Commands
{
    public record DeleteProductCommand(Guid Id) : IRequest<Unit>;

    public class DeleteProductCommandHandler(IGenericRepository<Product> repo, ICacheService cache, ILogger<DeleteProductCommand> logger) : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IGenericRepository<Product> _repo = repo;
        private readonly ICacheService _cache = cache;
        private readonly ILogger<DeleteProductCommand> _logger = logger;

        public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(command.Id)
                ?? throw new KeyNotFoundException("Product not found");

            _repo.Remove(entity);
            await _repo.SaveChangesAsync();
            _logger.LogInformation("Product with ID {Id} deleted successfully", command.Id);

            await _cache.RemoveAsync("products:all");

            return Unit.Value;
        }
    }
}
