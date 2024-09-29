﻿using FluentValidation;

namespace contacts_app.Users.AuthorizeUser
{
    public class RequestAuthorizeUserDtoValidator : AbstractValidator<RequestAuthorizeUserDto>
    {
        public RequestAuthorizeUserDtoValidator()
        {
            RuleFor(m => m.email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(m => m.password)
                .NotEmpty()
                .Matches("^(?=.*\\d)(?=.*[a-zA-Z])(?=.*[A-Z]).{12,}$")
                .WithMessage("Invalid password format");
        }
    }
}