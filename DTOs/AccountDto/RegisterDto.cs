using FluentValidation;

namespace Pinterest.DTOs.AccountDto
{
	public class RegisterDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Username { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
	}
	public class RegisterDtoValidator : AbstractValidator<RegisterDto>
	{
		public RegisterDtoValidator()
		{
			RuleFor(a => a.Email).NotEmpty().WithMessage("Email is required!")
				.EmailAddress().WithMessage("Invalid email address!");
			RuleFor(a => a.Password).NotEmpty().WithMessage("Password is required")
				.MinimumLength(6).WithMessage("Minimum 6 characters allowed!")
				.MaximumLength(20).WithMessage("Maximum length has reached!");
			RuleFor(a => a.Username).NotEmpty().WithMessage("Username is required");
			RuleFor(a => a.Firstname).NotEmpty().WithMessage("Firstname is required!");
			RuleFor(a => a.Lastname).NotEmpty().WithMessage("Last name is required");
		}
	}
}
