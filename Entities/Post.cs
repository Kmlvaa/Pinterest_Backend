namespace Pinterest.Entities
{
	public class Post
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public DateTime CreatedAt { get; set; }
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
		public List<Comment> Comments { get; set; }
		public List<Like> Likes { get; set; }
		public List<Save> Save { get; set; }
	}
}
