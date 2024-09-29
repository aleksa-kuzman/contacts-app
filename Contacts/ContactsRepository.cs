using contacts_app.Common;
using contacts_app.Contacts.Model;

namespace contacts_app.Contacts
{
    public class ContactsRepository
    {
        private readonly ContactsDbContext _context;

        public ContactsRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public IList<Contact> GetAllContacts()
        {
            var contacts = _context.Contacts.ToList();
            return contacts;
        }

        public Contact AddContact(Contact contact)
        {
            var contactEntity = _context.Contacts.Add(contact);

            return contactEntity.Entity;
        }
    }
}