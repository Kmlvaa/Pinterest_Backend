using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using System.IdentityModel.Tokens.Jwt;
using Pinterest.DTOs.Follower;
using Pinterest.Entities;

namespace Pinterest.Controllers
{
	[Route("api")]
	[ApiController]
	public class FollowerController : ControllerBase
	{
		private readonly AppDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public FollowerController(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
		}
		[HttpGet]
		[Route("getFollowers/{id}")]
		public IActionResult GetFollowers(string id)
		{
			var followers = _dbContext.FollowerUsers.Where(x => x.AppUserId == id).ToList();
			if(followers is null) return NotFound();

			var list = new List<GetFollowerDto>();

			foreach(var follower in followers)
			{
				var dto = new GetFollowerDto()
				{
					Id = follower.Id,
					Username = _dbContext.AppUsers.FirstOrDefault(x => x.Id == id).UserName,
					Follower = follower.Username
				};
				list.Add(dto);
			}

			return Ok(list.Count);
		}

		[HttpPost]
		[Route("addFollower/{id}")]
		public IActionResult AddFollower(string id)
		{
			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var usernameClaim = token.Claims.FirstOrDefault(x => x.Type == "Username");
			var userId = userIdClaim.Value;
			var username = usernameClaim.Value;

			if(id == userId)
			{
				return BadRequest("You can not follow yourself");
			}

			var existingUser = _dbContext.FollowerUsers.Where(x => x.AppUserId == id).FirstOrDefault(x => x.Username == username);
			if (existingUser != null) return BadRequest("User already followed!");

			var follower = new FollowerUser()
			{
				AppUserId = id,
				Username = username
			};
			_dbContext.FollowerUsers.Add(follower);
			_dbContext.SaveChanges();

			return Ok("User is followed!");
		}
		[HttpDelete]
		[Route("unFollow/{id}")]
		public IActionResult UnFollow(string id)
		{
			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var usernameClaim = token.Claims.FirstOrDefault(x => x.Type == "Username");
			var username = usernameClaim.Value;

			var user = _dbContext.FollowerUsers.Where(x => x.AppUserId == id).FirstOrDefault(x => x.Username == username);
			if (user == null) return BadRequest("You are not following this account!");

			_dbContext.FollowerUsers.Remove(user);
			_dbContext.SaveChanges();

			return Ok("You unfollowed this account!");
		}
		[HttpGet]
		[Route("isUserFollowed/{id}")]
		public IActionResult IsUserFollowed(string id)
		{
			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var usernameClaim = token.Claims.FirstOrDefault(x => x.Type == "Username");
			var username = usernameClaim.Value;

			var user = _dbContext.FollowerUsers.Where(x => x.Username == username).FirstOrDefault(x => x.AppUserId == id);

			if (user is not null) return Ok(true);
			else return Ok(false);
		}
	}
}
