using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
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

			builder.Services.AddSwaggerGen(opt =>
			{
				opt.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Description = "Enter the Bearer Authorization : Bearer Genreated-JWT-Token",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				opt.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id=JwtBearerDefaults.AuthenticationScheme,
							}
						},new string[]{}
					}
				});
			});

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