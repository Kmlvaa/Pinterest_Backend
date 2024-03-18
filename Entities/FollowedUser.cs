namespace Pinterest.Entities
{
	public class FollowedUser
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public AppUser User { get; set; }
	}
}
