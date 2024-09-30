using contacts_app.Api.Users.AuthorizeUser;

namespace contacts_app.Api.Users
{
    public static class UserEndpoints
    {
        public static void MapUsers(this IEndpointRouteBuilder app)
        {
            app.MapAuthorizeUserEndpoint();
        }
    }
}