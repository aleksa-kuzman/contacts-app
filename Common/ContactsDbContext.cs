using contacts_app.Contacts.Model;
using contacts_app.Users.Model;
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

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        private const string Schema = "contacts";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}