using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using Pinterest.DTOs.Saved;
using Pinterest.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Pinterest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SavedController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;
		private readonly IHttpContextAccessor _contextAccessor;
		public SavedController(AppDbContext appDbContext, IHttpContextAccessor contextAccessor)
		{
			_appDbContext = appDbContext;
			_contextAccessor = contextAccessor;
		}
		[HttpPost]
		[Route("addSaved/{id}")]
		public IActionResult AddSaved(int id)
		{
			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userNameClaim = token.Claims.FirstOrDefault(x => x.Type == "Username");

			var userId = userIdClaim.Value;

			var post = _appDbContext.Posts.FirstOrDefault(p => p.Id == id);
			if (post is null) return NotFound();

			var existingSavedPost = _appDbContext.Saveds.Where(x => x.AppUserId == userId).FirstOrDefault(x => x.PostId == id);
			if (existingSavedPost is not null) return BadRequest("This post is already saved!");

			var save = new Save()
			{
				AppUserId = userId,
				PostId = id,
			};

			_appDbContext.Saveds.Add(save);
			_appDbContext.SaveChanges();

			return Ok("Post is saved!");
		}
		[HttpGet]
		[Route("getSaveds/{id}")]
		public IActionResult GetSaveds(string id)
		{
			var saveds = _appDbContext.Saveds.Where(x => x.AppUserId == id).ToList();

			var list = new List<GetSavedDto>();

			foreach (var saved in saveds)
			{
				var dto = new GetSavedDto()
				{
					Id = saved.Id,
					PostId = saved.PostId,
					UserId = saved.AppUserId,
				};
				list.Add(dto);
			}

			return Ok(list);
		}
		[HttpDelete]
		[Route("deleteSaved/{id}")]
		public IActionResult DeleteSaved(int id)
		{
			var accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.ReadJwtToken(accessToken);

			var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "UserID");
			var userNameClaim = token.Claims.FirstOrDefault(x => x.Type == "Username");

			var userId = userIdClaim.Value;

			var saved = _appDbContext.Saveds.Where(x => x.AppUserId == userId).FirstOrDefault(x => x.PostId == id);
			if (saved is null) return NotFound();

			_appDbContext.Saveds.Remove(saved);
			_appDbContext.SaveChanges();

			return Ok("Post removed from saveds!");
		}
	}
}
