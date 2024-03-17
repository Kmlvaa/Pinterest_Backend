using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pinterest.Data;
using Pinterest.DTOs.Profile;
using Pinterest.Entities;

namespace Pinterest.Controllers
{
	public class ProfileController : ControllerBase
	{
		private readonly AppDbContext _dbContext;
		public ProfileController(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		[Route("userDetails/{id}")]
		public IActionResult UserDetails(int id)
		{
			var data = _dbContext.UserDetails.FirstOrDefault(x => x.AppUserId == id);
			if (data == null)  return NotFound();

			var dto = new EditProfileDto
			{
				Firstname = data.Firstname,
				Lastname = data.Lastname,
				Username = data.Username,
				About = data.About,
				ProfileUrl = data.ProfilePicUrl,
				Gender = data.Gender
			};

			return Ok(dto);
		}

		[HttpPut]
		[Route("userDetails/{id}")]
		public IActionResult UserDetails(int id, EditProfileDto dto)
		{
			if(!ModelState.IsValid) return BadRequest();

			var data = _dbContext.UserDetails.FirstOrDefault(x => x.AppUserId == id);
			if (data == null) return NotFound();

			data.Firstname = dto.Firstname;
			data.Lastname = dto.Lastname;
			data.Username = dto.Username;
			data.About = dto.About;
			data.Gender = dto.Gender;
			data.ProfilePicUrl = dto.ProfileUrl;

			_dbContext.Update(data);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
