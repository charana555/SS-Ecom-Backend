namespace EcommerceAPI.Models.Dto
{
	public class CartRequestDto
	{
		public int UserId { get; set; }
		public int ProductId { get; set; }
		public double UnitPrice { get; set; }
		public double Discount { get; set; }
		public int Quantity { get; set; }
		public double TotalPrice { get; set; }
	}
}
