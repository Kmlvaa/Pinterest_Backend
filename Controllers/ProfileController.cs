using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pinterest.Data;
using Pinterest.DTOs.Profile;
using Pinterest.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProfileController : ControllerBase
	{
		private readonly AppDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public ProfileController(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpGet]
		[Route("getUserDetails")]
		public IActionResult GetUserDetails()
		{
			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.ReadJwtToken(accessToken);
			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var data = _dbContext.UserDetails.FirstOrDefault(x => x.AppUserId == userId);
			if (data == null) return NotFound();

			var dto = new GetUserDetailsDto
			{
				Firstname = data.Firstname,
				Lastname = data.Lastname,
				Username = data.Username,
				About = data.About,
				ProfileUrl = data.ProfilePicUrl,
				Gender = data.Gender,
			};

			return Ok(dto);
		}

		[HttpPut]
		[Route("PutUserDetails")]
		public IActionResult PutUserDetails(EditProfileDto dto)
		{
			if (!ModelState.IsValid) return BadRequest();

			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.ReadJwtToken(accessToken);
			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var data = _dbContext.UserDetails.FirstOrDefault(x => x.AppUserId == userId);
			if (data == null) return NotFound();

			data.Firstname = dto.Firstname;
			data.Lastname = dto.Lastname;
			data.Username = dto.Username;
			data.About = dto.About;
			data.Gender = dto.Gender;
			data.ProfilePicUrl = dto.ProfileUrl;

			_dbContext.Update(data);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
