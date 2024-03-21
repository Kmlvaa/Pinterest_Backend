using Pinterest.Entities;

namespace Pinterest.DTOs.Follower
{
	public class AddFollowerDto
	{
		public string Username { get; set; }
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
	}
}
