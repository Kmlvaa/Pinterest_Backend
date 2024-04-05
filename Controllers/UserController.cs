using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pinterest.Data;
using Pinterest.DTOs.User;
using Pinterest.Entities;

namespace Pinterest.Controllers
{	
	[Route("api")]
	[ApiController]
	public class UserController : ControllerBase
	{
		public readonly AppDbContext _appDbContext;
		public UserController(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		[HttpGet]
		[Route("adminDetails")]
		public IActionResult AdminDetails()
		{


			return Ok();
		}

		[HttpGet]
		[Route("getUsers")]
		public IActionResult Index()
		{
			var users = _appDbContext.UserDetails.ToList();

			var list = new List<GetUsersDto>();

			foreach (var user in users)
			{
				var dto = new GetUsersDto()
				{
					Id = user.Id,
					UserName = user.Username,
					Firstname = user.Firstname,
					Lastname = user.Lastname,
					Gender = user.Gender,
					Image = user.ProfilePicUrl,
					UserId = user.AppUserId,
					FollowerCount = _appDbContext.FollowerUsers.Where(x => x.AppUserId == user.AppUserId).Count(),
				};
				list.Add(dto);
			}
			return Ok(list);
		}

		[HttpDelete]
		[Route("deleteUser/{id}")]
		public IActionResult DeleteUser(string id)
		{
			var user = _appDbContext.AppUsers.FirstOrDefault(x => x.Id == id);
			if (user == null) return NotFound("User not found!");

			_appDbContext.AppUsers.Remove(user);
			_appDbContext.SaveChanges();

			return Ok("User deleted successfully!");
		}
		[HttpGet]
		[Route("searchUsers")]
		public IActionResult Search(string? input)
		{
			var users = input == null ? new List<AppUser>()
				: _appDbContext.AppUsers
				.Where(x => x.UserName.ToLower()
				.StartsWith(input.ToLower()))
				.ToList();

			return Ok(users);
		}
	}
}
