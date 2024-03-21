using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pinterest.Data;
using Pinterest.DTOs.AccountDto;
using Pinterest.DTOs.User;
using Pinterest.Entities;
using Pinterest.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace Pinterest.Controllers
{
	public class AccountController : ControllerBase
	{
		private readonly IAuthService _authService;
		public readonly IConfiguration _configuration;
		public readonly AppDbContext _appDbContext;

		public AccountController(IAuthService authService, IConfiguration configuration, AppDbContext appDbContext)
		{
			_authService = authService;
			_configuration = configuration;
			_appDbContext = appDbContext;
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			if (!ModelState.IsValid)
			{
				var errorMessage = string.Join(" | ", ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));
				return BadRequest(errorMessage);
			}

			var (status, message) = await _authService.Login(dto);
			if (status == 0)
				return BadRequest(message);

			return Ok(message);
		}
		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			if (!ModelState.IsValid)
			{
				var errorMessage = string.Join(" | ", ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));
				return BadRequest(errorMessage);
			}

			var (status, message) = await _authService.Register(dto, UserRoles.User);
			if (status == 0)
			{
				return BadRequest(message);
			}

			return Ok(message);
		}
		public async Task<IActionResult> LogOut()
		{
			return RedirectToAction(nameof(Login));
		}
	}
}
