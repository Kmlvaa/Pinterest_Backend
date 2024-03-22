﻿using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using System.IdentityModel.Tokens.Jwt;
using Pinterest.DTOs.Follower;
using Pinterest.Entities;

namespace Pinterest.Controllers
{
	[Route("api/[controller]")]
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
			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var followers = _dbContext.FollowerUsers.Where(x => x.AppUserId == id).ToList();
			if(followers is null) return NotFound();

			var list = new List<GetFollowerDto>();

			foreach(var follower in followers)
			{
				var dto = new GetFollowerDto()
				{
					Username = _dbContext.AppUsers.FirstOrDefault(x => x.Id == userId).UserName,
					AppUserId = id
				};
				list.Add(dto);
			}

			return Ok();
		}

		[HttpPost]
		[Route("addFollower/{id}")]
		public IActionResult AddFollower(string id)
		{
			var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var follower = new FollowerUser()
			{
				AppUserId = id,
				Username = _dbContext.AppUsers.FirstOrDefault(x => x.Id == userId).UserName
			};
			_dbContext.FollowerUsers.Add(follower);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
