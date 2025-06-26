namespace ProductService.Dtos
{
    public class VariantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = ""; // e.g. "Large Red"
        public decimal AdditionalPrice { get; set; }
    }
}
