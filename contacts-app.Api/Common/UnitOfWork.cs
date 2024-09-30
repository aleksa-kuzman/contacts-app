using contacts_app.Contacts;
using contacts_app.Users;

namespace contacts_app.Common
{
    public class UnitOfWork
    {
        private ContactsDbContext _context;

        private UserRepository _userRepository;
        private ContactsRepository _contactRepository;

        public UnitOfWork(ContactsDbContext context)
        {
            _context = context;
        }

        public ContactsRepository ContactsRepository
        {
            get
            {
                if (_contactRepository == null)
                {
                    _contactRepository = new ContactsRepository(_context);
                }
                return _contactRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual void SaveAsync()
        {
            _context.SaveChangesAsync();
        }
    }
}