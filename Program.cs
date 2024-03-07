using Pinterest.Helper;

namespace Pinterest
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddAppServices(builder);

			var app = builder.Build();

			await DataSeed.InitializeAsync(app.Services);

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();
			app.UseCors(x => x
				.AllowAnyMethod()
				.AllowAnyHeader()
				.SetIsOriginAllowed(origin => true) // allow any origin 
				.AllowCredentials());

			app.Run();
		}
	}
}