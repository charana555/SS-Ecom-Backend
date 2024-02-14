using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
        private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment) 
        {
            _db = db;
			this._webHostEnvironment = webHostEnvironment;
		}
        [HttpGet("getProducts")]
        public async Task<ProductResponseDto> GetProducts()
        {
            ProductResponseDto res = new();

            List<Product> products = await _db.Products.Select(
                x => new Product()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Discount = x.Discount,
                    UnitPrice = x.UnitPrice,
                    ImageUrl = String.Format("{0}://{1}{2}/Images/{3}" , Request.Scheme ,Request.Host ,Request.PathBase, x.ImageUrl)
                }
                ).ToListAsync();

            if (products.Count <= 0)
            {
                res.StatusCode = StatusCodes.Status204NoContent;
                res.StatusMessage = "Product list is Empty !!";
                res.ProudctsList = [];
            }
            else
            {
                res.StatusCode = StatusCodes.Status200OK;
                res.StatusMessage = "";
                res.ProudctsList = products;
            }
            return res;
        }

        [HttpPost("addProduct")]
        public async Task<Response> AddProduct([FromForm]ProductRequestDto productRequestDto)
        {
            Response res = new();
            if(productRequestDto == null)
            {
                res.StatusCode = StatusCodes.Status400BadRequest;
                res.StatusMessage = "Product cannot be Empty!!";
                return res;
            }

            try
            {
				Product product = new()
				{
					Name = productRequestDto.Name,
					Description = productRequestDto.Description,
					Discount = productRequestDto.Discount,
					UnitPrice = productRequestDto.UnitPrice,
					ImageUrl = await SaveImage(productRequestDto.ImageFile)
				};

				_db.Products.Add(product);
				await _db.SaveChangesAsync();

				res.StatusCode = StatusCodes.Status201Created;
                res.StatusMessage = "Product added successfylly";


			} catch(Exception ex)
            {
				res.StatusCode = StatusCodes.Status400BadRequest;
				res.StatusMessage = "Something went wrong!!" + ex;
			}

            return res;
		}

        [NonAction]
        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ','-');
            imageName += DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);

            var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
              await  imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
