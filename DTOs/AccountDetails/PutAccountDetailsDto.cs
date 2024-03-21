namespace Pinterest.DTOs.AccountDetails
{
	public class PutAccountDetailsDto
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public DateTime Birthdate { get; set; }
		public string Country { get; set; }
	}
}