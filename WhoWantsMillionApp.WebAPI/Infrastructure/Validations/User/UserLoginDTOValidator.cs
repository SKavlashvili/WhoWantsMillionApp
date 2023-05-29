using FluentValidation;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Validations.User
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidator()
        {
            RuleFor((UserLoginDTO newUser) => newUser.UserName)
                .Must((string userName) => !string.IsNullOrEmpty(userName) && userName.Length > 5)
                .WithMessage("UserName Length should be more than 5");

            RuleFor(newUser => newUser.Password)
                .Must(password => !string.IsNullOrEmpty(password) && password.Length > 8)
                .WithMessage("Password Length should be more then 8");
        }
    }
}
