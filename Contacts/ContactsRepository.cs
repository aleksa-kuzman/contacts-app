using contacts_app.Common;
using contacts_app.Common.Exceptions;
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

        public IList<Contact> GetAllContactsByUserId(Guid UserId)
        {
            var contacts = _context
                .Contacts
                .Where(m => m.UserId == UserId)
                .ToList();
            return contacts;
        }

        public Contact AddContact(Contact contact)
        {
            var contactEntity = _context.Contacts.Add(contact);

            return contactEntity.Entity;
        }

        internal Contact GetContactById(Guid id)
        {
            var contact = _context.Contacts
                .Where(m => m.Id == id)
                .FirstOrDefault();

            return contact;
        }

        internal Contact GetContactById(Guid id, Guid userId)
        {
            var contact = _context.Contacts
                .Where(m => m.Id == id && m.UserId == userId)
                .FirstOrDefault();

            return contact;
        }

        internal Contact DeleteContact(Guid id)
        {
            var contact = GetContactById(id);
            if (contact == null)
            {
                throw new NotFoundException("Not found");
            }

            _context.Contacts.Remove(contact);

            return contact;
        }
    }
}