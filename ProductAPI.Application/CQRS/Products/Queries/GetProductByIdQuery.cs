using AutoMapper;
using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Core.Entities;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Application.CQRS.Products.Queries
{
    public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponseDto?>;

    public class GetProductByIdQueryHandler(IGenericRepository<Product> repo, IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductResponseDto?>
    {
        private readonly IGenericRepository<Product> _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<ProductResponseDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            return entity is null ? null : _mapper.Map<ProductResponseDto>(entity);
        }
    }
}
