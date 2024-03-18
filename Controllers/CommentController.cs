using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using Pinterest.DTOs.Comment;
using Pinterest.DTOs.Post;
using Pinterest.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;
		private readonly IHttpContextAccessor _contextAccessor;
		public CommentController(AppDbContext appDbContext, IHttpContextAccessor contextAccessor)
		{
			_appDbContext = appDbContext;
			_contextAccessor = contextAccessor;
		}
		[HttpPost]
		[Route("addComment")]
		public IActionResult AddComment([FromBody] AddCommentDto dto)
		{
			if (!ModelState.IsValid) return NotFound();

			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.ReadJwtToken(accessToken);
			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var post = _appDbContext.Posts.FirstOrDefault(x => x.AppUserId == userId);

			var comment = new Comment()
			{
				PostId = post.Id,
				Description = dto.Description,
				CreatedDate = DateTime.UtcNow,
			};
			_appDbContext.Add(comment);
			_appDbContext.SaveChanges();

			return Ok();
		}
	}
}
