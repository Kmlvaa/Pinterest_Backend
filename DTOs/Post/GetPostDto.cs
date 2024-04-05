using Pinterest.DTOs.Comment;
using Pinterest.DTOs.Like;

namespace Pinterest.DTOs.Post
{
	public class GetPostDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Url { get; set; }
		public string User { get; set; }
		public string UserId { get; set; }
	}
}
