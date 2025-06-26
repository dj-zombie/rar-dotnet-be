namespace ProductService.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public List<VariantDto> Variants { get; set; } = new();
    }
}
