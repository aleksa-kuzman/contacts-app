﻿namespace contacts_app.Users.AuthorizeUser
{
    public record ResponseAuthorizeUserDto(string email, string jwt, Guid id);
}