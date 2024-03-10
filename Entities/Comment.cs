namespace Pinterest.Entities
{
	public class Comment
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime CreatedDate { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
	}
}
