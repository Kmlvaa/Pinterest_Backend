using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Pinterest.Data;
using Pinterest.DTOs.Comment;
using Pinterest.DTOs.Like;
using Pinterest.DTOs.Post;
using Pinterest.Entities;
using Pinterest.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;

namespace Pinterest.Controllers
{
	[Route("api")]
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
			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);
			if (token is null) return BadRequest("An error occurred in generating the token");

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");

			var userId = userIdClaim.Value;

			var posts = _appDbContext.Posts.Where(x => x.AppUserId != userId).OrderByDescending(x => x).ToList();
			if(posts is null) return NotFound();

			var list = new List<GetAllPostsDto>();

			foreach(var post in posts)
			{
				var postDto = new GetAllPostsDto()
				{
					Id = post.Id,
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
		[Route("getUserPosts/{id}")]
		public IActionResult GetUserPosts(string id)
		{

			var posts = _appDbContext.Posts.Where(x => x.AppUserId ==id).OrderByDescending(x => x).ToList();
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
					User = _appDbContext.AppUsers.FirstOrDefault(x => x.Id == post.AppUserId).UserName,
				};
				list.Add(postDto);
			}

			return Ok(list);
		}

		[HttpPost]
		[Route("addPost")]
		public IActionResult AddPost([FromForm] AddPostDto dto)
		{
			if (!ModelState.IsValid) return NotFound("Something went wrong!");
			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

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

			return Ok("Post created successfully!");
		}
		[HttpPut]
		[Route("editPost/{id}")]
		public IActionResult EditPost(int id, [FromBody] EditPostDto dto)
		{
			if (!ModelState.IsValid) return NotFound("Something went wrong!");

			var post = _appDbContext.Posts.FirstOrDefault(x => x.Id == id);
			if (post == null) return NotFound();

			post.Title = dto.Title;
			post.Description = dto.Description;
			post.CreatedAt = DateTime.UtcNow;
			post.ImageUrl = post.ImageUrl;

			_appDbContext.Posts.Update(post);
			_appDbContext.SaveChanges();

			return Ok("Post is updated!");
		}


		[HttpDelete]
		[Route("deletePost/{id}")]
		public IActionResult DeletePost(int id)
		{
			var post = _appDbContext.Posts.FirstOrDefault(x => x.Id == id);
			if (post is null) return NotFound();

			var comments = _appDbContext.Comments.Where(x => x.PostId == id).ToList();
			foreach(var comment in comments)
			{
				_appDbContext.Comments.Remove(comment);	
			}
			var likes = _appDbContext.Likes.Where(x => x.PostId == id).ToList();
			foreach (var like in likes)
			{
				_appDbContext.Likes.Remove(like);
			}
			var saveds = _appDbContext.Saveds.Where(x => x.PostId == id).ToList();
			foreach (var saved in saveds)
			{
				_appDbContext.Saveds.Remove(saved);
			}

			_fileService.DeleteFile(post.ImageUrl);

			_appDbContext.Posts.Remove(post);
			_appDbContext.SaveChanges();

			return Ok("Post is deleted successfully!");
		}
		[HttpGet]
		[Route("getPostDetails/{id}")]
		public IActionResult GetPostDetails(int id)
		{
			var post = _appDbContext.Posts.Include(x => x.Comments).Include(x => x.Likes).FirstOrDefault(x => x.Id == id);
			if (post is null) return NotFound();

			var comments = post.Comments.ToList();
			var commentList = new List<GetCommentsDto>();

			foreach( var comment in comments )
			{
				var commentItem = new GetCommentsDto
				{
					Id = comment.Id,
					PostId = post.Id,
					Username = _appDbContext.Users.FirstOrDefault(x => x.Id == comment.AppUserId).UserName,
					Comment = comment.Description,
					CreatedAt = comment.CreatedDate.ToShortDateString(),
				};
				commentList.Add(commentItem);
			}
			var likes = post.Likes.ToList();
			var likeList = new List<GetLikeDto>();

			foreach ( var like in likes)
			{
				var likeItem = new GetLikeDto
				{
					Id = like.Id,
					PostId = like.PostId,
					Username = _appDbContext.Users.FirstOrDefault(x => x.Id == like.AppUserId).UserName
				};
				likeList.Add(likeItem);
			}
			var postDto = new GetPostDetailsDto()
			{
				Id = post.Id,
				Title = post.Title,
				Description = post.Description,
				CreatedAt = post.CreatedAt.ToShortDateString(),
				Url = post.ImageUrl,
				UserId = post.AppUserId,
				User = _appDbContext.Users.FirstOrDefault(x => x.Id == post.AppUserId).UserName,
				Comments = commentList,
				Likes = likeList
			};
				
			return Ok(postDto);
		}
	}
}
