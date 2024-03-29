using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pinterest.Data;
using Pinterest.DTOs.User;

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
					Image = user.ProfilePicUrl
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
			if (user == null) return NotFound();

			_appDbContext.AppUsers.Remove(user);
			_appDbContext.SaveChanges();

			return Ok("User deleted successfully!");
		}
	}
}
