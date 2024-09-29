using contacts_app.Common;
using contacts_app.Contacts.AddContact.Dto;
using contacts_app.Contacts.GetContacts.Dto;
using contacts_app.Contacts.Model;
using FluentValidation;
using Mapster;

namespace contacts_app.Contacts
{
    public class ContactService
    {
        private readonly UnitOfWork _uow;
        private IValidator<RequestAddContactDto> _validator;

        public ContactService(UnitOfWork uow, IValidator<RequestAddContactDto> validator)
        {
            _uow = uow;
            _validator = validator;
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

        /// <summary>
        /// Inserts a contact into a database
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public GetContactsDto AddContact(RequestAddContactDto contactDto)
        {
            var validationResult = _validator.Validate(contactDto);

            if (!validationResult.IsValid)
            {
                throw new BadHttpRequestException("Invalid payload");
            }

            var contact = contactDto.Adapt<Contact>();
            var insertedContact = _uow.ContactsRepository.AddContact(contact);

            _uow.Save();

            return insertedContact.Adapt<GetContactsDto>();
        }
    }
}