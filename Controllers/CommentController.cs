using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using Pinterest.DTOs.Comment;
using Pinterest.DTOs.Post;
using Pinterest.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
	[Route("api")]
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
		[HttpGet]
		[Route("getComments/{id}")]
		public IActionResult GetComments(int id)
		{
			var post = _appDbContext.Posts.FirstOrDefault(x => x.Id == id);
			if(post == null) return NotFound();

			var comments = _appDbContext.Comments.Where(x => x.PostId == post.Id).OrderByDescending(x => x).ToList();
			if(comments == null) return NotFound();

			var list = new List<GetCommentsDto>();

			foreach (var comment in comments)
			{
				var dto = new GetCommentsDto()
				{
					Id = comment.Id,
					CreatedAt = comment.CreatedDate.ToShortDateString(),
					Comment = comment.Description,
					Username = _appDbContext.Users.FirstOrDefault(x => x.Id == comment.AppUserId).UserName,
					PostId = id,
					UserId = comment.AppUserId,
					Url = _appDbContext.UserDetails.FirstOrDefault(x => x.AppUserId == comment.AppUserId).ProfilePicUrl
				};
				list.Add(dto);
			}

			return Ok(list);
		}
		[HttpPost]
		[Route("addComment/{id}")]
		public IActionResult AddComment(int id, [FromBody] AddCommentDto dto)
		{
			if (!ModelState.IsValid) return NotFound();

			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.ReadJwtToken(accessToken);
			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userId = userIdClaim.Value;

			var post = _appDbContext.Posts.FirstOrDefault(x => x.Id == id);
			if (post == null) return NotFound();

			var comment = new Comment()
			{
				Description = dto.Description,
				CreatedDate = DateTime.Now,
				PostId = id,
				AppUserId = userId,
			};

			_appDbContext.Comments.Add(comment);
			_appDbContext.SaveChanges();

			return Ok("Comment is added!");
		}
		[HttpDelete]
		[Route("deleteComment/{id}")]
		public IActionResult DeleteComment(int id)
		{
			var comment = _appDbContext.Comments.FirstOrDefault(x => x.Id == id);
			if (comment == null) return NotFound();

			_appDbContext.Comments.Remove(comment);
			_appDbContext.SaveChanges();

			return Ok("Comment deleted successfully!");
		}
	}
}
