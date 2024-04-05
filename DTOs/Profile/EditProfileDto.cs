using FluentValidation;
using Pinterest.DTOs.AccountDto;

namespace Pinterest.DTOs.Profile
{
	public class EditProfileDto
	{
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string About { get; set; }
		public string Username { get; set; }
		public IFormFile ProfileUrl { get; set; }
		public string Gender { get; set; }
	}
}
