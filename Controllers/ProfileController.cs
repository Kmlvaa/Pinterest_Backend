using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pinterest.Data;
using Pinterest.DTOs.Profile;
using Pinterest.Entities;
using Pinterest.Services;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
	[Route("api")]
	[ApiController]
	public class ProfileController : ControllerBase
	{
		private readonly AppDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly FileService _fileService;
		public ProfileController(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, FileService fileService)
		{
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
			_fileService = fileService;
		}

		[HttpGet]
		[Route("getUserDetails")]
		public IActionResult GetUserDetails()
		{
			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
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
		[HttpGet]
		[Route("getOtherUserDetails/{id}")]
		public IActionResult GetOtherUserDetails(string id)
		{
			var data = _dbContext.UserDetails.FirstOrDefault(x => x.AppUserId == id);
			if (data == null) return NotFound();

			var dto = new GetUserDetailsDto
			{
				Firstname = data.Firstname,
				Lastname = data.Lastname,
				Username = data.Username,
				About = data.About,
				ProfileUrl = data.ProfilePicUrl,
				Gender = data.Gender,
				UserId = data.AppUserId
			};

			return Ok(dto);
		}

		[HttpPut]
		[Route("putUserDetails")]
		public async Task<IActionResult> PutUserDetails([FromForm]EditProfileDto dto)
		{
			if (!ModelState.IsValid) return BadRequest();

			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.ReadJwtToken(accessToken);
			if(token is null) return BadRequest();
			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var data = _dbContext.UserDetails.FirstOrDefault(x => x.AppUserId == userId);
			if (data == null) return NotFound();

			var Url = _fileService.AddFile(dto.ProfileUrl);

			data.Firstname = dto.Firstname;
			data.Lastname = dto.Lastname;
			data.Username = dto.Username;
			data.About = dto.About;
			data.Gender = dto.Gender;
			data.ProfilePicUrl = Url;

			var user = _dbContext.AppUsers.FirstOrDefault(x => x.Id == userIdClaim.Value);
			if (user == null) return NotFound();

			user.FirstName = dto.Firstname;
			user.LastName = dto.Lastname;
			user.UserName = dto.Username;

			await _userManager.UpdateAsync(user);

			_dbContext.Update(data);
			_dbContext.SaveChanges();

			return Ok("User details updated!");
		}
	}
}
