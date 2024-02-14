using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost("addToCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Response AddToCart([FromBody]CartRequestDto cartRequestDto)
        {
            Response res = new();
            if(cartRequestDto == null)
            {
                res.StatusCode = StatusCodes.Status400BadRequest;
                res.StatusMessage = "Item cannot be empty!";
                return res;
            }

            Cart cart = new()
            { 
                ProductId = cartRequestDto.ProductId,
                UserId = cartRequestDto.UserId,
                Discount = cartRequestDto.Discount,
                UnitPrice = cartRequestDto.UnitPrice,
                Quantity = cartRequestDto.Quantity,
                TotalPrice = cartRequestDto.TotalPrice
            };

            _db.Carts.Add(cart);
            _db.SaveChanges();

            res.StatusCode = StatusCodes.Status200OK;
            res.StatusMessage = "Item added to cart successfully!";
            return res;
        }

        [HttpDelete( "{id:int}" ,Name ="removeFromCart")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Response RemoveFromCart(int id)
        {
            Response res = new();
            var CartItem = _db.Carts.FirstOrDefault(x => x.Id == id);
            if(CartItem == null)
            {
                res.StatusCode = StatusCodes.Status400BadRequest;
                res.StatusMessage = "Item that you are trying to delete doesn't exists!";
                return res;
            }

            _db.Carts.Remove(CartItem);
            _db.SaveChanges();
            res.StatusCode= StatusCodes.Status200OK;
            res.StatusMessage = "Item deleted successfully!";
            return res;
        }

        [HttpGet("{userId:int}" ,Name = "listCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public CartResponseDto GetCart(int userId)
        {
            CartResponseDto res = new();
            if(userId < 0)
            {
                res.StatusCode = StatusCodes.Status404NotFound;
                res.StatusMessage = "User doesn't exists !";
                return res;
            }

            var cart = _db.Carts.Where(e => e.UserId == userId).ToList();

            if(cart.Count <= 0) {
                res.StatusCode = StatusCodes.Status204NoContent;
                res.StatusMessage = "Cart is Empty !";
                return res;
            }

            res.StatusCode = StatusCodes.Status200OK;
            res.StatusMessage = "";
            res.CartList = cart;

            return res;
        }
    }
}
