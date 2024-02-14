using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _configuration;
        public UserController(ApplicationDbContext db , IConfiguration configuration)
        {
            _db = db;
			_configuration = configuration;
        }

		[HttpPost("register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Response Register([FromBody]UserRegisterDto userRegisterDto)
		{
			 Response res = new();
			if (userRegisterDto == null)
			{
				res.StatusCode = StatusCodes.Status400BadRequest;
				res.StatusMessage = "Please Provide all the required fields";
				return res;
			}

			if (!IsUniqueUser(userRegisterDto.Email))
			{
				res.StatusCode = StatusCodes.Status400BadRequest;
				res.StatusMessage = "User Already Exists , Please Login !!";
				return res;
			}

			string HashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

			User user = new()
			{
				Id = 0 ,
				Name = userRegisterDto.Name,
				Email = userRegisterDto.Email,
				Password = HashedPassword,
				CreatedOn = DateTime.Now,
			};

			_db.Add(user);
			_db.SaveChanges();

			res.StatusCode = StatusCodes.Status200OK;
			res.StatusMessage = "User created successfully!";

			return res;
		}

		[HttpPost("login")]
		public UserResponse Login([FromBody]UserLoginDto userLoginDto)
		{
			UserResponse res = new();
			if(userLoginDto == null)
			{
				res.StatusCode = StatusCodes.Status400BadRequest;
				res.StatusMessage = "Please provide all the required fields !";
				return res;
			}

		    var IsUserExits = _db.Users.FirstOrDefault(u => u.Email == userLoginDto.Email);

			if (IsUserExits == null)
			{
				res.StatusCode = StatusCodes.Status404NotFound;
				res.StatusMessage = "Invalid Credentials";
				return res;
			}

			if(!BCrypt.Net.BCrypt.Verify(userLoginDto.Password , IsUserExits.Password)) {
				res.StatusCode = StatusCodes.Status404NotFound;
				res.StatusMessage = "Invalid Credentials";
				return res;
			}

			string Token = CreateToken(IsUserExits);

			res.StatusCode = StatusCodes.Status200OK;
			res.StatusMessage = "User logged In successfully!";
			res.Token = Token;

			return res;

		}

		private bool IsUniqueUser(string email)
		{
			var user = _db.Users.FirstOrDefault(u => u.Email == email);

			if (user == null)
			{
				return true;
			}

			return false;
		}

		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name , user.Name),
				new Claim(ClaimTypes.Email , user.Email)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_configuration.GetSection("AppSettings:Token").Value!));

			var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

			var token = new JwtSecurityToken(
					claims: claims,
					expires: DateTime.Now.AddDays(1),
					signingCredentials: cred
				);
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
