using FluentValidation;

namespace MyRecipes.Application.Users;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("The username cannot be empty.")
            .MinimumLength(3)
            .WithMessage("The username must be atleast 3 characters long.")
            .MaximumLength(15)
            .WithMessage("The username must be under 16 characters long.")
            .Matches("^[a-zA-Z0-9]*$")
            .WithMessage("The username can only contain numbers and characters through A to Z.");

        RuleFor(x => x.Password)
            .Matches("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,18}$")
            .WithMessage("The password must be between 6 to 18 characters long, and it must contain atleast one capital letter and a number.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("The email must be a valid email address.");
    }
}