using contacts_app.Common;
using contacts_app.Users.AuthorizeUser;
using contacts_app.Users.Model;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace contacts_app.Users
{
    public class UserService
    {
        private readonly UnitOfWork _uow;
        private readonly JWTSecurityTokenHelper _jwtHelper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IValidator<RequestAuthorizeUserDto> _validator;

        public UserService(UnitOfWork unitOfWork,
            JWTSecurityTokenHelper helper,
            IPasswordHasher<User> passwordHasher,
            IValidator<RequestAuthorizeUserDto> validator)
        {
            _uow = unitOfWork;
            _jwtHelper = helper;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public ResponseAuthorizeUserDto AuthorizeUser(RequestAuthorizeUserDto dto)
        {
            var userToAuthenticate = _uow.UserRepository.GetUserByEmail(dto.email);

            var validationResult = _validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                throw new BadHttpRequestException("Invalid payload");
            }

            if (userToAuthenticate == null)
            {
                throw new Exception();
            }

            var hashingResult = _passwordHasher.VerifyHashedPassword(userToAuthenticate, userToAuthenticate.Password, dto.password);

            if (hashingResult != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Not authorized");
            }

            var token = _jwtHelper.GenerateJwtToken(userToAuthenticate.Id.ToString());
            var userDto = userToAuthenticate.Adapt<ResponseAuthorizeUserDto>();
            return userDto;
        }
    }
}