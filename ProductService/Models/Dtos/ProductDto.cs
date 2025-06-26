namespace ProductService.Dtos
{

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string MainImageUrl { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public List<ProductVariantDto> Variants { get; set; } = new();
        public List<ProductImageDto> Images { get; set; } = new();
    }
}
