﻿namespace Pinterest.Entities
{
	public class FollowedUser
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
	}
}
