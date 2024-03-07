using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pinterest.Entities;

namespace Pinterest.Data
{
	public class AppDbContext: IdentityDbContext<AppUser>
	{
		public AppDbContext()
		{
		}
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			
		}
		public DbSet<AppUser> AppUsers {  get; set; }
		public DbSet<AccountDetails> AccountDetails { get; set; }
		public DbSet<Country> Country { get; set; }
		public DbSet<Follows> Follows { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<User> Users { get; set; }

	}
}
