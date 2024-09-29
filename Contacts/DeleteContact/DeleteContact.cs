using contacts_app.Contacts.Model;
using Microsoft.AspNetCore.Mvc;

namespace contacts_app.Contacts.DeleteContact
{
    public static class DeleteContact
    {
        internal static void MapDeleteContactEndpoint(this IEndpointRouteBuilder app) =>
            app.MapDelete("api/contact", (
                ContactService contactService,
                [FromQuery] Guid id
                ) =>
            {
                var response = contactService.DeleteContact(id);
                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Returns all existing contacts in the system",
                Description = "Used to retrieve all contacts"
            })
            .Produces<List<Contact>>(statusCode: StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status403Forbidden);
    }
}