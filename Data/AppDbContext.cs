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
		public DbSet<UserDetail> UserDetails { get; set; }
		public DbSet<AccountDetails> AccountDetails { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Saved> Saveds { get; set; }
		public DbSet<FollowedUser> FollowedUsers { get; set; }
		public DbSet<FollowerUser> FollowerUsers { get; set;}

	}
}
