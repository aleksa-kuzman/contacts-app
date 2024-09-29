using contacts_app.Contacts.AddContact;
using contacts_app.Contacts.GetContacts;
using contacts_app.Contacts.UpdateContact;

namespace contacts_app.Contacts
{
    public static class ContactsEndpoint
    {
        public static void MapContacts(this IEndpointRouteBuilder app)
        {
            app.MapGetContactsEndpoint();
            app.MapAddContactEndpoint();
            app.MapUpdateContactEndpoint();
        }
    }
}