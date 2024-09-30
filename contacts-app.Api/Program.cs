using contacts_app.Api.Common;
using contacts_app.Api.Common.ExceptionHandlers;
using contacts_app.Api.Contacts;
using contacts_app.Api.Contacts.AddContact;
using contacts_app.Api.Contacts.AddContact.Dto;
using contacts_app.Api.Users;
using contacts_app.Api.Users.AuthorizeUser;
using contacts_app.Api.Users.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(JWTSecurityTokenHelper));
builder.Services.AddTransient(typeof(UnitOfWork));
builder.Services.AddTransient(typeof(UserService));
builder.Services.AddTransient(typeof(ContactService));

builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<ForbiddenExceptionHandler>();
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddScoped<IPasswordHasher<User>, BcryptHasher<User>>();
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection(AppConfiguration.configName));

var key = builder.Configuration.GetValue<string>("App:Key");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

///Validators
builder.Services.AddScoped<IValidator<RequestAuthorizeUserDto>, RequestAuthorizeUserDtoValidator>();
builder.Services.AddScoped<IValidator<RequestAddContactDto>, RequestAddContactDtoValidator>();

builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var configuration = builder.Configuration;

builder.Services.AddDbContext<ContactsDbContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("Connection"))
.UseSnakeCaseNamingConvention());

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
    try
    {
        Log.Logger.Information("Trying to migrate database");

        var db = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
        db.Database.Migrate();
        Log.Logger.Information("Successfully migrated database");
    }
    catch (Exception ex)
    {
        Log.Logger.Error("An error occured while migrating database", ex.Message);
        throw;
    }
}
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();

app.MapUsers();
app.MapContacts();
//app.UseHttpsRedirection();

app.Run();

public partial class Program
{ }