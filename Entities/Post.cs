namespace Pinterest.Entities
{
	public class Post
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public string link { get; set; }
		public bool Comment { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
