using contacts_app.Common;

namespace contacts_app.Users
{
    public class UserService
    {
        private readonly UnitOfWork _uow;

        public UserService(UnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
    }
}