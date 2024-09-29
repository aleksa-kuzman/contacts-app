using contacts_app.Contacts.GetContacts;

namespace contacts_app.Contacts
{
    public static class ContactsEndpoint
    {
        public static void MapContacts(this IEndpointRouteBuilder app)
        {
            app.MapGetContactsEndpoint();
        }
    }
}