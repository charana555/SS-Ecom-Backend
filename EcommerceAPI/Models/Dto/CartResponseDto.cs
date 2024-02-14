namespace EcommerceAPI.Models.Dto
{
	public class CartResponseDto : Response
	{
        public List<Cart> CartList { get; set; }
    }
}
