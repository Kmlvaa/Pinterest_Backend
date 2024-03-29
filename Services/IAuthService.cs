using Pinterest.DTOs.AccountDto;

namespace Pinterest.Services
{
	public interface IAuthService
	{
		Task<(int, string, string)> Login(LoginDto dto);
		Task<(int, string)> Register(RegisterDto dto, string role);
	}
}
