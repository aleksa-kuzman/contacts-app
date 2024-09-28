using System.IdentityModel.Tokens.Jwt;

namespace contacts_app.Users.AuthorizeUser
{
    public static class AuthorizeUser
    {
        internal static void MapAuthorizeUserEndpoint(this IEndpointRouteBuilder app) =>
            app.MapPost("Authorize",
                async (
                    RequestAuthorizeUserDto authorizeDto,
                    JwtSecurityTokenHandler tokenHandler
                    ) =>
                {
                    return Results.Ok();
                }
                );
    }
}