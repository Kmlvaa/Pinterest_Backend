using FluentValidation;

namespace Pinterest.DTOs.Profile
{
	public class GetUserDetailsDto
	{
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string About { get; set; }
		public string Username { get; set; }
		public string UserId { get; set; }
		public string ProfileUrl { get; set; }
		public string Gender { get; set; }
	}
}
