using FluentValidation.Results;

namespace contacts_app.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public IList<ValidationFailure> _errors;

        public BadRequestException(string errorMessage) : base(errorMessage)
        {
        }

        public BadRequestException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {
        }

        public BadRequestException(string errorMessage, List<ValidationFailure> errors) : base(errorMessage)
        {
            _errors = errors;
        }
    }
}