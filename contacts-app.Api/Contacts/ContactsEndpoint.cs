using contacts_app.Api.Contacts.AddContact;
using contacts_app.Api.Contacts.DeleteContact;
using contacts_app.Api.Contacts.GetContacts;
using contacts_app.Api.Contacts.UpdateContact;

namespace contacts_app.Api.Contacts
{
    public static class ContactsEndpoint
    {
        public static void MapContacts(this IEndpointRouteBuilder app)
        {
            app.MapGetContactsEndpoint();
            app.MapAddContactEndpoint();
            app.MapUpdateContactEndpoint();
            app.MapDeleteContactEndpoint();
        }
    }
}