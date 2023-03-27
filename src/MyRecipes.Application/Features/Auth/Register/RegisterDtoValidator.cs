using FluentValidation;

namespace MyRecipes.Application.Features.Auth.Register;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Password)
            .Matches("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,18}$")
            .WithMessage("The password must be between 6 to 18 characters long, and it must contain at least one capital letter and a number.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("The email must be a valid email address.");
    }
}