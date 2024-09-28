﻿using contacts_app.Users.AuthorizeUser.Model;
using Microsoft.EntityFrameworkCore;

namespace contacts_app.Common
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext()
        { }

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
        }

        private const string Schema = "Contacts";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseNpgsql("Connection")
            .UseSnakeCaseNamingConvention();
        }
    }
}