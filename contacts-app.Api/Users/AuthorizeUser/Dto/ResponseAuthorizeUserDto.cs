namespace contacts_app.Api.Users.AuthorizeUser
{
    public record ResponseAuthorizeUserDto(string email, string jwt, Guid id);
}