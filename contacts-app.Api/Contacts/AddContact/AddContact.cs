using contacts_app.Contacts.AddContact.Dto;
using contacts_app.Contacts.Model;
using Microsoft.AspNetCore.Mvc;

namespace contacts_app.Contacts.AddContact
{
    public static class AddContact
    {
        internal static void MapAddContactEndpoint(this IEndpointRouteBuilder app) =>
            app.MapPost("api/contact", (
                [FromBody] RequestAddContactDto dto,
                ContactService contactService

                ) =>
            {
                var response = contactService.AddContact(dto);
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