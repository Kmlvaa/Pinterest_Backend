namespace Pinterest.DTOs.Comment
{
	public class GetCommentsDto
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Username { get; set; }
		public int PostId { get; set; }
	}
}
