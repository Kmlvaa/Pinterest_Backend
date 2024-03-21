using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using Pinterest.DTOs.AccountDetails;
using Pinterest.DTOs.Profile;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
		[Route("api/[controller]")]
		[ApiController]
		public class AccountDetailsController : ControllerBase
		{
			private readonly AppDbContext _dbContext;
			private readonly IHttpContextAccessor _httpContextAccessor;
			public AccountDetailsController(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
			{
				_dbContext = dbContext;
				_httpContextAccessor = httpContextAccessor;
			}

			[HttpGet]
			[Route("getAccountDetails")]
			public IActionResult GetAccountDetails()
			{
				var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
				var tokenHandler = new JwtSecurityTokenHandler();
				var token = tokenHandler.ReadJwtToken(accessToken);
				var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
				var userId = userIdClaim.Value;

				var data = _dbContext.AccountDetails.FirstOrDefault(x => x.AppUserId == userId);
				if (data == null) return NotFound();

				var dto = new GetAccountDetailsDto
				{
					Birthdate = data.BirthDate,
					Country = data.Country,
					Email = data.Email,
					Username = data.Username,
				};

				return Ok(dto);
			}

			[HttpPut]
			[Route("putAccountDetails")]
			public IActionResult PutAccountDetails(PutAccountDetailsDto dto)
			{
				if (!ModelState.IsValid) return BadRequest();

				var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
				var tokenHandler = new JwtSecurityTokenHandler();
				var token = tokenHandler.ReadJwtToken(accessToken);
				if (token is null) return BadRequest();
				var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
				var userId = userIdClaim.Value;

				var data = _dbContext.AccountDetails.FirstOrDefault(x => x.AppUserId == userId);
				if (data == null) return NotFound();

				data.Email = dto.Email;
				data.Username = dto.Username;
				data.BirthDate = dto.Birthdate;
				data.Country = dto.Country;

				var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
				if (user == null) return BadRequest();

				user.Email = dto.Email;
				user.UserName = dto.Username;

				_dbContext.Update(data);
				_dbContext.SaveChanges();

				return Ok();
			}
		}
	
}
