namespace Pinterest.Entities
{
	public class UserDetail
	{
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Username { get; set; }
		public string About { get; set; }
		public string Gender { get; set; }
		public string ProfilePicUrl { get; set; }
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
	}
}
