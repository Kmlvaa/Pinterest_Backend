using Microsoft.AspNetCore.Identity;

namespace Pinterest.Entities
{
	public class AppUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public UserDetail UserDetail { get; set; }
		public AccountDetails AccountDetails { get; set; }
		public List<Post> Posts { get; set; }
		public List<FollowedUser> FollowedUsers { get; set; }
		public List<FollowerUser> FollowerUsers { get; set; }
		public List<Save> Saveds { get; set; }
		public List<Comment> Comments { get; set; }
	}
}
