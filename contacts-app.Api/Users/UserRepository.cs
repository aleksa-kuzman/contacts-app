using contacts_app.Api.Common;
using contacts_app.Api.Users.Model;

namespace contacts_app.Api.Users
{
    public class UserRepository
    {
        private readonly ContactsDbContext _context;

        public UserRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public User? GetUserByEmail(string email)
        {
            var user = _context.Users
                .Where(m => m.EmailConfirmed && m.Email == email)
                .FirstOrDefault();

            return user;
        }
    }
}