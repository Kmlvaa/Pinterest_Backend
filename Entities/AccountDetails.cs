namespace Pinterest.Entities
{
	public class AccountDetails
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public DateTime BirthDate { get; set; }
		public string Country { get; set; }
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
	}
}
