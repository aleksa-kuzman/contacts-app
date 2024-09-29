using contacts_app.Contacts.AddContact.Dto;
using FluentValidation;

namespace contacts_app.Contacts.AddContact
{
    public class RequestAddContactDtoValidator : AbstractValidator<RequestAddContactDto>
    {
        /// <summary>
        /// Example of valid numbers:
        /// +1 (123) 456-7890
        /// 123-456-7890
        /// +44 1234 567890
        /// +91 98765-43210
        /// (123) 456-7890
        /// </summary>
        public RequestAddContactDtoValidator()
        {
            RuleFor(m => m.PhoneNumber)
                .Matches("^\\+?(\\d{1,3})?[-. ]?(\\(?\\d{1,4}\\)?)?[-. ]?\\d{1,4}[-. ]?\\d{1,4}[-. ]?\\d{1,9}$")
                .NotEmpty();
        }
    }
}