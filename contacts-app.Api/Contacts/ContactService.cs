using contacts_app.Api.Common;
using contacts_app.Api.Common.Exceptions;
using contacts_app.Api.Contacts.AddContact.Dto;
using contacts_app.Api.Contacts.DeleteContact.Dto;
using contacts_app.Api.Contacts.GetContacts.Dto;
using contacts_app.Api.Contacts.Model;
using contacts_app.Api.Contacts.UpdateContact.Dto;
using FluentValidation;
using Mapster;

namespace contacts_app.Api.Contacts
{
    public class ContactService
    {
        private readonly UnitOfWork _uow;
        private IValidator<RequestAddContactDto> _validator;
        private IHttpContextAccessor _contextAccessor;

        public ContactService(
            UnitOfWork uow,
            IValidator<RequestAddContactDto> validator,
            IHttpContextAccessor contextAccessor
           )
        {
            _uow = uow;
            _validator = validator;
            _contextAccessor = contextAccessor;
        }

        private Guid GetUserId()
        {
            var userIdString = _contextAccessor.HttpContext!.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                throw new Exception("Internal server error");
            }

            return Guid.Parse(userIdString);
        }

        /// <summary>
        /// Gets All Contacts
        /// </summary>
        /// <returns></returns>
        public IList<GetContactsDto> GetAllContacts()
        {
            var userId = GetUserId();
            var contactsFromDb = _uow.ContactsRepository.GetAllContactsByUserId(userId);
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
                throw new BadRequestException("Invalid payload", validationResult.Errors);
            }

            var contact = contactDto.Adapt<Contact>();

            contact.UserId = GetUserId();
            var insertedContact = _uow.ContactsRepository.AddContact(contact);

            _uow.Save();

            return insertedContact.Adapt<GetContactsDto>();
        }

        public ResponseUpdateContactDto UpdateContact(RequestUpdateContactDto dto, Guid id)
        {
            var userId = GetUserId();

            var contact = _uow.ContactsRepository.GetContactById(id, userId);
            if (contact == null)
            {
                throw new NotFoundException("Not Found");
            }
            contact.PhoneNumber = dto.PhoneNumber;
            contact.Name = dto.Name;

            _uow.Save();

            var response = contact.Adapt<ResponseUpdateContactDto>();

            return response;
        }

        public DeleteContactDto DeleteContact(Guid id)
        {
            Contact contact = _uow.ContactsRepository.DeleteContact(id);

            var contactResponse = contact.Adapt<DeleteContactDto>();

            _uow.Save();

            return contactResponse;
        }
    }
}