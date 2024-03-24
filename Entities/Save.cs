namespace Pinterest.Entities
{
	public class Save
	{
		public int Id { get; set; }
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
	}
}
