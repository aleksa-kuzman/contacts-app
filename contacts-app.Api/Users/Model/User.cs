﻿using contacts_app.Api.Contacts.Model;

namespace contacts_app.Api.Users.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}