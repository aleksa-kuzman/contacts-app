using contacts_app.Common;
using contacts_app.Contacts.GetContacts.Dto;
using Mapster;

namespace contacts_app.Contacts
{
    public class ContactService
    {
        private readonly UnitOfWork _uow;

        public ContactService(UnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Gets All Contacts
        /// </summary>
        /// <returns></returns>
        public IList<GetContactsDto> GetAllContacts()
        {
            var contactsFromDb = _uow.ContactsRepository.GetAllContacts();
            var contactsRes = contactsFromDb.Adapt<IList<GetContactsDto>>();

            return contactsRes;
        }
    }
}