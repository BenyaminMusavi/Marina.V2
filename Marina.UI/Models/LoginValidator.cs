using FluentValidation;

namespace Marina.UI.Models;

public class LoginValidator : AbstractValidator<LoginVm>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");

        //RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Invalid Credentials")
       .MinimumLength(6).WithMessage("Invalid Credentials")
       .MaximumLength(16).WithMessage("Invalid Credentials")
       .Matches(@"[A-Z]+").WithMessage("Invalid Credentials")
       .Matches(@"[a-z]+").WithMessage("Invalid Credentials")
       .Matches(@"[0-9]+").WithMessage("Invalid Credentials")
       .Matches(@"[\@\!\?\*\.]+").WithMessage("Invalid Credentials");
    }
}
