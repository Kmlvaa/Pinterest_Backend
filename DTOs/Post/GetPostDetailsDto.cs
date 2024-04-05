using Pinterest.DTOs.Comment;
using Pinterest.DTOs.Like;

namespace Pinterest.DTOs.Post
{
	public class GetPostDetailsDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string CreatedAt { get; set; }
		public string Url { get; set; }
		public string User { get; set; }
		public string UserId { get; set; }
		public List<GetCommentsDto> Comments { get; set; }
		public List<GetLikeDto> Likes { get; set; }
	}
}
