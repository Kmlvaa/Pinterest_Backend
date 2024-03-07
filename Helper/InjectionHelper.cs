using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pinterest.Data;
using Pinterest.DTOs.User;
using Pinterest.Entities;
using Pinterest.Services;
using System.Text;

namespace Pinterest.Helper
{
	public static class InjectionHelper
	{
		public static void AddAppServices(this IServiceCollection services, WebApplicationBuilder builder)
		{
			builder.Services.AddDbContext<AppDbContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
			builder.Services.AddSwaggerGen(opt =>
			{
				opt.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Pinterest",
					Description = "This is a simple API for Pinterest",
				});
			});

			builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
			{
				opt.Password.RequiredLength = 6;
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequireUppercase = true;
				opt.Password.RequireLowercase = false;
			}).AddEntityFrameworkStores<AppDbContext>();
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = builder.Configuration["Jwt:ValidAudience"],
					ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"]))
				};
			});
			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy(UserRoles.Admin, policy => policy.RequireRole(UserRoles.Admin));
				options.AddPolicy(UserRoles.User, policy => policy.RequireRole(UserRoles.User));
			});
			builder.Services.AddTransient<IAuthService, AuthService>();

			builder.Services.AddFluentValidationAutoValidation()
				.AddFluentValidationClientsideAdapters()
				.AddValidatorsFromAssemblyContaining<Program>();

			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			builder.Services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			});
		}
	}
}
