using FluentValidation;
using Pinterest.DTOs.AccountDto;

namespace Pinterest.DTOs.Profile
{
	public class EditProfileDto
	{
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string About { get; set; }
		public string Username { get; set; }
		public string ProfileUrl { get; set; }
		public string Gender { get; set; }
		public int UserId { get; set; }
	}
}
