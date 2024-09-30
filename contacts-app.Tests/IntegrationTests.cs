using contacts_app.Common;
using contacts_app.Contacts.AddContact.Dto;
using contacts_app.Contacts.GetContacts.Dto;
using contacts_app.Users.AuthorizeUser;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace contacts_app.Tests
{
    public class IntegrationTests : IClassFixture<ContactsApplication<Program, ContactsDbContext>>
    {
        private readonly ContactsApplication<Program, ContactsDbContext> _contactsApplication;
        private HttpClient _client;

        public const string initUsers = "init-users.sql";
        public const string initContacts = "init-contacts.sql";
        public const string clearDb = "clear-db.sql";

        private ContactsDbContext _context;

        public IntegrationTests(ContactsApplication<Program, ContactsDbContext> app)
        {
            try
            {
                _contactsApplication = app;
                _client = app.CreateClient();
                var options = new DbContextOptionsBuilder<ContactsDbContext>()
                  .UseNpgsql(_contactsApplication._postgreSqlContainer.GetConnectionString())
                  .UseSnakeCaseNamingConvention()
                  .Options;

                _context = new ContactsDbContext(options);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Guid> AuthorizeUser()
        {
            RequestAuthorizeUserDto userDto = new RequestAuthorizeUserDto
            (
                email: "john.doe@example.com",
                password: "password"
            );

            var serializedContent = JsonConvert.SerializeObject(userDto);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            var authResponse = await _client.PostAsync("api/user/authorize", httpContent);

            var authResponsePayload = JsonConvert.DeserializeObject<ResponseAuthorizeUserDto>(await authResponse.Content.ReadAsStringAsync());
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponsePayload.jwt);

            return authResponsePayload.id;
        }

        #region AuthTests

        [Fact]
        public async Task GivenValidEmailAdressAndPassword_AuthorizeUser_WillReturn_200()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);

            RequestAuthorizeUserDto userDto = new RequestAuthorizeUserDto
           (
               email: "john.doe@example.com",
               password: "password"
           );

            var serializedContent = JsonConvert.SerializeObject(userDto);

            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/user/authorize", httpContent);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);

            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        public async Task GivenValidEmailAdressAndPassword_AuthorizeUser_WillReturnAJwtToken()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);

            RequestAuthorizeUserDto userDto = new RequestAuthorizeUserDto
           (
               email: "john.doe@example.com",
               password: "password"
           );

            var serializedContent = JsonConvert.SerializeObject(userDto);

            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/user/authorize", httpContent);

            var responsePayload = JsonConvert.DeserializeObject<ResponseAuthorizeUserDto>(await response.Content.ReadAsStringAsync());

            Assert.True(responsePayload!.jwt != null);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        public async Task GivenInvalidEmail_AuthorizeUser_WillReturn_400()
        {
            RequestAuthorizeUserDto userDto = new RequestAuthorizeUserDto
            (
                email: "Test",
                password: "password"
            );

            var serializedContent = JsonConvert.SerializeObject(userDto);

            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/user/authorize", httpContent);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GivenInvalidPassword_AuthorizeUser_WillReturn403()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            RequestAuthorizeUserDto userDto = new RequestAuthorizeUserDto
           (
               email: "john.doe@example.com",
               password: "passwordasdf"
           );

            var serializedContent = JsonConvert.SerializeObject(userDto);

            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/user/authorize", httpContent);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Forbidden);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        #endregion AuthTests

        #region get all contact tests

        [Fact]
        public async Task GivenValidBearerTokenAndDataInDatabase_GetContacts_ShouldReturnMoreThanOneContact()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            RequestAuthorizeUserDto userDto = new RequestAuthorizeUserDto
           (
               email: "john.doe@example.com",
               password: "password"
           );
            var serializedContent = JsonConvert.SerializeObject(userDto);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            var authResponse = await _client.PostAsync("api/user/authorize", httpContent);

            var authResponsePayload = JsonConvert.DeserializeObject<ResponseAuthorizeUserDto>(await authResponse.Content.ReadAsStringAsync());

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponsePayload.jwt);
            var response = await _client.GetAsync("api/contact");

            var responsePayload = JsonConvert.DeserializeObject<List<GetContactsDto>>(await response.Content.ReadAsStringAsync());

            Assert.True(responsePayload!.Count > 0);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        public async Task GivenNoBearerToken_GetContacts_ShouldReturn401()
        {
            var response = await _client.GetAsync("api/contact");

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        #endregion get all contact tests

        #region insert contact test

        [Fact]
        public async Task GivenNoBearerToken_InsertContact_ShouldReturn401()
        {
            var response = await _client.PostAsync("api/contact", null);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        private async Task GivenBearerTokenAndValidPayload_AddContacts_ShouldReturn200()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await AuthorizeUser();
            RequestAddContactDto dto = new RequestAddContactDto("Aleksa Kuzman", "+3816322333");

            var serializedContent = JsonConvert.SerializeObject(dto);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/contact", httpContent);

            var responsePayload = JsonConvert.DeserializeObject<ResponseAddContactDto>(await response.Content.ReadAsStringAsync());

            Assert.True(response.IsSuccessStatusCode);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        private async Task GivenBearerTokenAndInvalidPhoneNumber_AddContacts_ShouldReturn400()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await AuthorizeUser();

            RequestAddContactDto dto = new RequestAddContactDto("Aleksa Kuzman", "asdf");

            var serializedContent = JsonConvert.SerializeObject(dto);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/contact", httpContent);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        private async Task GivenBearerTokenAndValidPayload_AddContacts_ShouldUpdateDatabase()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            var userId = await AuthorizeUser();
            RequestAddContactDto dto = new RequestAddContactDto("Ivan Kuzman", "+3816322333");

            var serializedContent = JsonConvert.SerializeObject(dto);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/contact", httpContent);
            var responsePayload = JsonConvert.DeserializeObject<ResponseAddContactDto>(await response.Content.ReadAsStringAsync());

            var expectedResult = _context
                .Contacts
                .Where(m => m.Name == "Ivan Kuzman")
                .FirstOrDefault();

            Assert.NotNull(expectedResult);
            Assert.True(expectedResult.Name == dto.Name
                && expectedResult.PhoneNumber == dto.PhoneNumber
                && expectedResult.UserId == userId);

            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        #endregion insert contact test

        #region update contact test

        [Fact]
        private async Task GivenNoBearerToken_UpdateContact_ShouldReturn401()
        {
            var response = await _client.PatchAsync("api/contact", null);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        private async Task GivenBearerTokenAndValidPayload_UpdateContact_ShouldReturn200()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            await AuthorizeUser();
            RequestAddContactDto dto = new RequestAddContactDto("Ivan Kuzman", "+3816322333");

            var serializedContent = JsonConvert.SerializeObject(dto);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync("api/contact?id={cc8cc779-3df6-44e1-8813-06defe9a2540}", httpContent);
            var responsePayload = JsonConvert.DeserializeObject<ResponseAddContactDto>(await response.Content.ReadAsStringAsync());

            Assert.True(response.IsSuccessStatusCode);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        private async Task GivenBearerTokenAndValidPayload_UpdateContact_ShouldUpdateDatabase()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            await AuthorizeUser();
            RequestAddContactDto dto = new RequestAddContactDto("Ivan Kuzman", "+3816322333");

            var serializedContent = JsonConvert.SerializeObject(dto);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync("api/contact?id={aeed9052-02fd-4c82-b9cb-f2d11781a28c}", httpContent);
            var responsePayload = JsonConvert.DeserializeObject<ResponseAddContactDto>(await response.Content.ReadAsStringAsync());

            var expectedResult = _context
                .Contacts
                .Where(m => m.Id == Guid.Parse("aeed9052-02fd-4c82-b9cb-f2d11781a28c"))
                .FirstOrDefault();

            Assert.NotNull(expectedResult);
            Assert.True(expectedResult.Name == "Ivan Kuzman" && expectedResult.PhoneNumber == "+3816322333");
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        private async Task GivenBearerTokenAndValidPayloadWithUserNotOwningContact_UpdateContact_ShouldReturn403()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            await AuthorizeUser();
            RequestAddContactDto dto = new RequestAddContactDto("Ivan Kuzman", "+3816322333");

            var serializedContent = JsonConvert.SerializeObject(dto);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync("api/contact?id={d490f7a6-eb62-448e-bfa5-9825f526122e}", httpContent);
            var responsePayload = JsonConvert.DeserializeObject<ResponseAddContactDto>(await response.Content.ReadAsStringAsync());

            var expectedResult = _context
                .Contacts
                .Where(m => m.Id == Guid.Parse("d490f7a6-eb62-448e-bfa5-9825f526122e"))
                .FirstOrDefault();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        #endregion update contact test

        #region delete contact test

        ///TSET
        ///

        [Fact]
        private async Task GivenNoBearerToken_DeleteContact_ShouldReturn401()
        {
            var response = await _client.DeleteAsync("api/contact?id={aeed9052-02fd-4c82-b9cb-f2d11781a28c}");

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        private async Task GivenBearerTokenAndValidId_DeleteContact_ShouldReturn200()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            await AuthorizeUser();

            var response = await _client.DeleteAsync("api/contact?id={aeed9052-02fd-4c82-b9cb-f2d11781a28c}");
            var responsePayload = JsonConvert.DeserializeObject<ResponseAddContactDto>(await response.Content.ReadAsStringAsync());

            Assert.True(response.IsSuccessStatusCode);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        private async Task GivenBearerTokenAndValidId_DeleteContact_ShouldDeleteTheRecordFromDatabase()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            await AuthorizeUser();

            var response = await _client.DeleteAsync("api/contact?id={aeed9052-02fd-4c82-b9cb-f2d11781a28c}");
            var responsePayload = JsonConvert.DeserializeObject<ResponseAddContactDto>(await response.Content.ReadAsStringAsync());

            var recordFromDb = _context
                .Contacts
                .Where(m => m.Id == Guid.Parse("aeed9052-02fd-4c82-b9cb-f2d11781a28c"))
                .FirstOrDefault();

            Assert.Null(recordFromDb);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        [Fact]
        private async Task GivenBearerTokenAndNotExistingId_DeleteContact_ShouldReturn404()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            await AuthorizeUser();

            var response = await _client.DeleteAsync("api/contact?id={c09dc06b-36eb-4258-b7f0-03237477fb75}");

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        private async Task GivenNonOwnerOfContactIsAuthorized_DeleteContact_ShouldReturn403()
        {
            await _contactsApplication.InitializeDataForTest(initUsers);
            await _contactsApplication.InitializeDataForTest(initContacts);

            await AuthorizeUser();

            var response = await _client.DeleteAsync("api/contact?id={cc8cc779-3df6-44e1-8813-06defe9a2540}");

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Forbidden);
            await _contactsApplication.InitializeDataForTest(clearDb);
        }

        #endregion delete contact test
    }
}