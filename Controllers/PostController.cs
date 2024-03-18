using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using Pinterest.DTOs.Post;
using Pinterest.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;
		private readonly IHttpContextAccessor _contextAccessor;
		public PostController(AppDbContext appDbContext, IHttpContextAccessor contextAccessor)
		{
			_appDbContext = appDbContext;
			_contextAccessor = contextAccessor;
		}
		[HttpGet]
		[Route("getPosts/{id}")]
		public IActionResult GetPosts()
		{
			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var posts = _appDbContext.Posts.FirstOrDefault(x => x.AppUserId == userId);

			return Ok();
		}

		[HttpPost]
		[Route("addPost")]
		public IActionResult AddPost([FromBody] AddPostDto dto)
		{
			if (!ModelState.IsValid) return NotFound();
			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var post = new Post()
			{
				Title = dto.Title,
				Description = dto.Description,
				CreatedAt = DateTime.UtcNow,
				ImageUrl = dto.Url,
			};

			_appDbContext.Add(post);
			_appDbContext.SaveChanges();

			return Ok();
		}
	}
}
