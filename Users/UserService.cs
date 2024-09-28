using contacts_app.Common;
using contacts_app.Users.AuthorizeUser;
using contacts_app.Users.AuthorizeUser.Model;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace contacts_app.Users
{
    public class UserService
    {
        private readonly UnitOfWork _uow;
        private readonly JWTSecurityTokenHelper _jwtHelper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(UnitOfWork unitOfWork,
            JWTSecurityTokenHelper helper,
            IPasswordHasher<User> passwordHasher)
        {
            _uow = unitOfWork;
            _jwtHelper = helper;
            _passwordHasher = passwordHasher;
        }

        public ResponseAuthorizeUserDto AuthorizeUser(RequestAuthorizeUserDto dto)
        {
            var userToAuthenticate = _uow.UserRepository.GetUserByEmail(dto.email);

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