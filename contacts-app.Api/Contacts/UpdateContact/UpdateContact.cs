using contacts_app.Api.Contacts.Model;
using contacts_app.Api.Contacts.UpdateContact.Dto;
using Microsoft.AspNetCore.Mvc;

namespace contacts_app.Api.Contacts.UpdateContact
{
    public static class UpdateContact
    {
        internal static void MapUpdateContactEndpoint(this IEndpointRouteBuilder app) =>
           app.MapPatch("api/contact", (
               [FromBody] RequestUpdateContactDto dto,
               [FromQuery] Guid id,
               ContactService contactService
               ) =>
           {
               var response = contactService.UpdateContact(dto, id);
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