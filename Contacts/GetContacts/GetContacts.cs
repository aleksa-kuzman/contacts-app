using contacts_app.Contacts.Model;

namespace contacts_app.Contacts.GetContacts
{
    public static class GetContacts
    {
        internal static void MapGetContactsEndpoint(this IEndpointRouteBuilder app) =>
            app.MapGet("api/contact", (
                ContactService contactService

                ) =>
            {
                return Results.Ok();
            }).RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Returns all existing contacts in the system",
                Description = "Used to retrieve all contacts"
            })
            .Produces<List<Contact>>(statusCode: StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status404NotFound);
    }
}