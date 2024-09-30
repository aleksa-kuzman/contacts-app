using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace contacts_app.Api.Contacts.Model
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasDefaultValueSql("(gen_random_uuid())");

            builder.HasOne(m => m.User)
                .WithMany(m => m.Contacts)
                .HasForeignKey(m => m.UserId);
        }
    }
}