using contacts_app.Api.Contacts.Model;
using contacts_app.Api.Users.Model;
using Microsoft.EntityFrameworkCore;

namespace contacts_app.Api.Common
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext()
        { }

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        private const string Schema = "contacts";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
        }
    }
}