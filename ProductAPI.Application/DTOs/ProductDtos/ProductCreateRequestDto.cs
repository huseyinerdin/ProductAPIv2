namespace ProductAPI.Application.DTOs.ProductDtos
{
    public class ProductCreateRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
