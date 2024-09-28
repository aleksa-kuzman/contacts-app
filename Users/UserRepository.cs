using contacts_app.Common;

namespace contacts_app.Users
{
    public class UserRepository
    {
        private readonly ContactsDbContext _context;

        public UserRepository(ContactsDbContext context)
        {
            _context = context;
        }
    }
}