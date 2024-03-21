using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using Pinterest.DTOs.Post;
using Pinterest.Entities;
using Pinterest.Services;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly FileService _fileService;
		public PostController(AppDbContext appDbContext, IHttpContextAccessor contextAccessor, FileService fileService)
		{
			_appDbContext = appDbContext;
			_contextAccessor = contextAccessor;
			_fileService = fileService;
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
					Url = post.ImageUrl,
					User = _appDbContext.Users.FirstOrDefault(x => x.Id == post.AppUserId).UserName
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
			var userNameClaim = token.Claims.FirstOrDefault(x => x.Type == "Username");

			var userId = userIdClaim.Value;
			var username = userNameClaim.Value;

			var posts = _appDbContext.Posts.Where(x => x.AppUserId == userId).ToList();
			if (posts is null) return NotFound();

			var list = new List<GetPostDto>();

			foreach (var post in posts)
			{
				var postDto = new GetPostDto()
				{
					Id = post.Id,
					Title = post.Title,
					Description = post.Description,
					CreatedAt = post.CreatedAt,
					Url = post.ImageUrl,
					User = username
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

			var Url = _fileService.AddFile(dto.Url);

			var post = new Post()
			{
				AppUserId = userId,
				Title = dto.Title,
				Description = dto.Description,
				CreatedAt = DateTime.UtcNow,
				ImageUrl = Url,
			};

			_appDbContext.Posts.Add(post);
			_appDbContext.SaveChanges();

			return Ok();
		}
		[HttpDelete]
		[Route("deletePost/{id}")]
		public IActionResult DeletePost(int id)
		{
			var post = _appDbContext.Posts.FirstOrDefault(x => x.Id == id);
			if (post is null) return NotFound();

			_fileService.DeleteFile(post.ImageUrl);

			_appDbContext.Posts.Remove(post);
			_appDbContext.SaveChanges();

			return Ok();
		}
	}
}
