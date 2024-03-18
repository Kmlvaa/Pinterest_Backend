using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pinterest.DTOs.AccountDto;
using Pinterest.DTOs.User;
using Pinterest.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pinterest.Services
{
    public class AuthService : IAuthService
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _config;

		public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_config = config;
		}

		public async Task<(int, string)> Register(RegisterDto dto, string role)
		{
			var existingUser = await _userManager.FindByNameAsync(dto.Username);
			if (existingUser != null)
				return (0, "User already exists!");

			AppUser user = new AppUser()
			{
				FirstName = dto.Firstname,
				LastName = dto.Lastname,
				Email = dto.Email,
				UserName = dto.Username
			};

			var createUserResult = await _userManager.CreateAsync(user, dto.Password);
			if (!createUserResult.Succeeded)
				return (0, "User creation failed! Please check user details and try again.");

			if (!await _roleManager.RoleExistsAsync(role))
				await _roleManager.CreateAsync(new IdentityRole(role));

			if (await _roleManager.RoleExistsAsync(UserRoles.User))
				await _userManager.AddToRoleAsync(user, role);

			return (1, "User created successfully!");
		}
		public async Task<(int, string)> Login(LoginDto dto)
		{
			var user = await _userManager.FindByNameAsync(dto.Username);
			if (user == null) 
				return (0, "Invalid Username!");
			if (!await _userManager.CheckPasswordAsync(user, dto.Password)) 
				return (0, "Invalid Password!");

			var userRoles = await _userManager.GetRolesAsync(user);
			var authClaims = new List<Claim>
			{
			   new Claim(ClaimTypes.Name, user.UserName),
			   new Claim("UserID",user.Id),
			   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};

			foreach (var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}
			string token = GenerateToken(authClaims);
			return (1, token);
		}
		private string GenerateToken(IEnumerable<Claim> claims)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"]));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = _config["JWT:ValidIssuer"],
				Audience = _config["JWT:ValidAudience"],
				Expires = DateTime.UtcNow.AddHours(3),
				SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
				Subject = new ClaimsIdentity(claims)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
