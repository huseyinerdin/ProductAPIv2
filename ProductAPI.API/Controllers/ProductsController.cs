using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.CQRS.Products.Commands;
using ProductAPI.Application.CQRS.Products.Queries;
using ProductAPI.Application.DTOs.ProductDtos;

namespace ProductAPI.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IMediator mediatr) : ControllerBase
    {
        private readonly IMediator _mediatr = mediatr;
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequestDto dto)
        {
            var created = await _mediatr.Send(new CreateProductCommand(dto));
            return created is null ? BadRequest("Product could not be created.")
                : CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _mediatr.Send(new GetProductsQuery());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _mediatr.Send(new GetProductByIdQuery(id));
            return product is null ? NotFound() : Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _mediatr.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateRequestDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Product ID mismatch.");
            }
            var updated = await _mediatr.Send(new UpdateProductCommand(id, dto));
            return Ok(updated);
        }
    }
}
