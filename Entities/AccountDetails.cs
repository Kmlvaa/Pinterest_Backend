namespace Pinterest.Entities
{
	public class AccountDetails
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public DateTime BirthDate { get; set; }
		public string Language { get; set; }
		public int CountryId { get; set; }
		public Country Country { get; set; }
	}
}
