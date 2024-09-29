using contacts_app.Common;
using contacts_app.Users;
using contacts_app.Users.AuthorizeUser;
using contacts_app.Users.Model;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(JWTSecurityTokenHelper));
builder.Services.AddTransient(typeof(UnitOfWork));
builder.Services.AddTransient(typeof(UserService));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IPasswordHasher<User>, BcryptHasher<User>>();

builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection(AppConfiguration.configName));

///Validators
builder.Services.AddScoped<IValidator<RequestAuthorizeUserDto>, RequestAuthorizeUserDtoValidator>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var configuration = builder.Configuration;

builder.Services.AddDbContext<ContactsDbContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("Connection"))
.UseSnakeCaseNamingConvention());

var app = builder.Build();

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

app.MapUsers();
app.UseExceptionHandler();
//app.UseHttpsRedirection();

app.Run();

public partial class Program
{ }