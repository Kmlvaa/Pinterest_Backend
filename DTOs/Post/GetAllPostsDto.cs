﻿namespace Pinterest.DTOs.Post
{
	public class GetAllPostsDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Url { get; set; }
		public string User { get; set; }
		public string UserId { get; set; }
		public string UserPhoto { get; set; }
	}
}
