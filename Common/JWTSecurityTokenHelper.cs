using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace contacts_app.Common
{
    public class JWTSecurityTokenHelper
    {
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly AppConfiguration _appConfiguration;

        public JWTSecurityTokenHelper(IOptions<AppConfiguration> options)
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _appConfiguration = options.Value;
        }

        public string GenerateJwtToken(string userId)
        {
            var key = Encoding.ASCII.GetBytes(_appConfiguration.Key);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            };

            var identity = new ClaimsIdentity(claims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                //Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            // Retrieve the JWT secret from environment variables and encode it
            var key = Encoding.ASCII.GetBytes("5864afaa-d592-44eb-807f-7a21c4cdcc61");

            try
            {
                // Create a token handler and validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                    //ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                // Return the claims principal
                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                // Handle token expiration
                throw new ApplicationException("Token has expired.");
            }
        }
    }
}