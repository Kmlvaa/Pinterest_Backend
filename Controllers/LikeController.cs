using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using Pinterest.DTOs.Comment;
using Pinterest.DTOs.Like;
using Pinterest.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LikeController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;
		private readonly IHttpContextAccessor _contextAccessor;
		public LikeController(AppDbContext appDbContext, IHttpContextAccessor contextAccessor)
		{
			_appDbContext = appDbContext;
			_contextAccessor = contextAccessor;
		}
		[HttpPost]
		[Route("addLike/{id}")]
		public IActionResult AddLike(int id)
		{
			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var existedLike = _appDbContext.Likes.Where(x => x.PostId == id).FirstOrDefault(x => x.AppUserId == userId);
			if (existedLike != null) return BadRequest("This post is already liked");

			var like = new Like()
			{
				AppUserId = userId,
				PostId = id,
			};

			_appDbContext.Likes.Add(like);
			_appDbContext.SaveChanges();

			return Ok("You liked this post!");
		}
		[HttpGet]
		[Route("getLikes/{id}")]
		public IActionResult GetLikes(int id)
		{
			var post = _appDbContext.Posts.FirstOrDefault(x => x.Id == id);
			if (post == null) return NotFound();

			var likes = _appDbContext.Likes.Where(x => x.PostId == post.Id).ToList();
			if (likes == null) return NotFound();

			var list = new List<GetLikeDto>();

			foreach (var like in likes)
			{
				var dto = new GetLikeDto()
				{
					Id = like.Id,
					Username = _appDbContext.Users.FirstOrDefault(x => x.Id == like.AppUserId).UserName,
					PostId = id
				};
				list.Add(dto);
			}

			return Ok(list);
		}
	}
}
