using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pinterest.DTOs.AccountDto;
using Pinterest.DTOs.User;
using Pinterest.Entities;
using Pinterest.Services;

namespace Pinterest.Controllers
{
	public class AccountController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AccountController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest();

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
				return BadRequest();

			var (status, message) = await _authService.Register(dto, UserRoles.User);
			if (status == 0)
			{
				return BadRequest(message);
			}

			return Ok(message);
		}
	}
}
