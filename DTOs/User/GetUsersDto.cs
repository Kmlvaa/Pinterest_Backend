using Pinterest.DTOs.Follower;

namespace Pinterest.DTOs.User
{
	public class GetUsersDto
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string UserId { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Gender { get; set; }
		public string Image { get; set; }
		public int FollowerCount { get; set; }
	}
}
