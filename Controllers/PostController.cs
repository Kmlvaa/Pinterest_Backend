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
		[Route("getAllPosts")]
		public IActionResult GetAllPosts()
		{
			var posts = _appDbContext.Posts.ToList();
			if(posts is null) return NotFound();

			var list = new List<GetAllPostsDto>();

			foreach(var post in posts)
			{
				var postDto = new GetAllPostsDto()
				{
					Title = post.Title,
					Description = post.Description,
					CreatedAt = post.CreatedAt,
					Url = post.ImageUrl
				};
				list.Add(postDto);
			}

			return Ok(list);
		}
		[HttpGet]
		[Route("getPosts")]
		public IActionResult GetPosts()
		{
			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var posts = _appDbContext.Posts.Where(x => x.AppUserId == userId).ToList();
			if (posts is null) return NotFound();

			var list = new List<GetPostDto>();

			foreach (var post in posts)
			{
				var postDto = new GetPostDto()
				{
					Title = post.Title,
					Description = post.Description,
					CreatedAt = post.CreatedAt,
					Url = post.ImageUrl
				};
				list.Add(postDto);
			}

			return Ok(list);
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
				AppUserId = userId,
				Title = dto.Title,
				Description = dto.Description,
				CreatedAt = DateTime.UtcNow,
				ImageUrl = dto.Url,
			};

			_appDbContext.Add(post);
			_appDbContext.SaveChanges();

			return Ok();
		}
		[HttpDelete]
		[Route("deletePost/{id}")]
		public IActionResult DeletePost(int id)
		{
			var post = _appDbContext.Posts.FirstOrDefault(x => x.Id == id);
			if (post is null) return NotFound();

			_appDbContext.Posts.Remove(post);
			_appDbContext.SaveChanges();

			return Ok();
		}
	}
}
