using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Pinterest.Helper;
using Pinterest.Services;

namespace Pinterest
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddDirectoryBrowser();

			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddAppServices(builder);

			var app = builder.Build();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("v1/swagger.json", "V1 Docs");

			});

			await DataSeed.InitializeAsync(app.Services);

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions 
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")), 
				RequestPath = "/Images"
			});

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