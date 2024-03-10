namespace Pinterest.Entities
{
	public class Saved
	{
		public int Id { get; set; }
		public int AppUserId { get; set; }
		public AppUser AppUser { get; set; }
	}
}
