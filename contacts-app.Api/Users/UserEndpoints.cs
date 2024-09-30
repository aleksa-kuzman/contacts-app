using contacts_app.Users.AuthorizeUser;

namespace contacts_app.Users
{
    public static class UserEndpoints
    {
        public static void MapUsers(this IEndpointRouteBuilder app)
        {
            app.MapAuthorizeUserEndpoint();
        }
    }
}