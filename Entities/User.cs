namespace Pinterest.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string About { get; set; }
		public string UserName { get; set; }
		public string Pronoun { get; set; }
		public string ImageUrl { get; set; }
		public List<Follows> Follows { get; set; }
		public List<Post> Posts { get; set; }
	}
}
