using Microsoft.EntityFrameworkCore;

namespace contacts_app
{
    public class ContactsDbContext(DbContextOptions options) : Microsoft.EntityFrameworkCore.DbContext(options)
    {
        private const string Schema = "Contacts";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
        }
    }
}