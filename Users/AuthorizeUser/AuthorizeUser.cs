using Microsoft.AspNetCore.Mvc;

namespace contacts_app.Users.AuthorizeUser
{
    public static class AuthorizeUser
    {
        internal static void MapAuthorizeUserEndpoint(this IEndpointRouteBuilder app) =>
            app.MapPost("api/User/Authorize",
                 (
                    UserService userService,

                   [FromBody] RequestAuthorizeUserDto authorizeDto

                    ) =>
                {
                    var response = userService.AuthorizeUser(authorizeDto);
                    return Results.Ok(response);
                }
                );
    }
}