using System.ComponentModel.DataAnnotations;

namespace Pinterest.DTOs.Post
{
	public class AddPostDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public IFormFile? Url { get; set; }
	}
}
