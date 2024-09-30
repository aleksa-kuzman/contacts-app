using FluentValidation;

namespace contacts_app.Api.Users.AuthorizeUser
{
    public class RequestAuthorizeUserDtoValidator : AbstractValidator<RequestAuthorizeUserDto>
    {
        public RequestAuthorizeUserDtoValidator()
        {
            RuleFor(m => m.email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format");
        }
    }
}