namespace EcommerceAPI.Models.Dto
{
	public class ProductRequestDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public double UnitPrice { get; set; }
		public double Discount { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
