namespace Pinterest.Entities
{
	public class Country
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<AccountDetails> AccountDetails { get; set; }
	}
}
