using contacts_app.Common;

namespace contacts_app.Contacts
{
    public class ContactsRepository
    {
        private readonly ContactsDbContext _context;

        public ContactsRepository(ContactsDbContext context)
        {
            _context = context;
        }
    }
}