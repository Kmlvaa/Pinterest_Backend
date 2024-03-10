namespace Pinterest.Entities
{
	public class UserDetail
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string About { get; set; }
		public string Pronoun { get; set; }
		public string ProfilePicUrl { get; set; }
		public string CoverUrl { get; set; }
		public int AppUserId { get; set; }
		public AppUser AppUser { get; set; }
	}
}
