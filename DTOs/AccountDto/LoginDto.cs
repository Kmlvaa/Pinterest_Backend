using FluentValidation;

namespace Pinterest.DTOs.AccountDto
{
	public class LoginDto
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
	public class LoginDtoValidator : AbstractValidator<LoginDto>
	{
		public LoginDtoValidator() {
			RuleFor(a => a.Username).NotEmpty().WithMessage("Username is required!");
			RuleFor(a => a.Password).NotEmpty().WithMessage("Password is required!")
				.MinimumLength(6).WithMessage("Minimum 6 characters allowed!")
				.MaximumLength(20).WithMessage("Maximum length has reached!"); ;
		}
	}
}
