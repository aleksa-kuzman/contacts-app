using contacts_app.Users;

namespace contacts_app.Common
{
    public class UnitOfWork
    {
        private ContactsDbContext _context;

        private UserRepository _userRepository;

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
    }
}