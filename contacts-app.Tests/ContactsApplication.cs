using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace contacts_app.Tests
{
    public class ContactsApplication<TProgram, TContext> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class where TContext : DbContext
    {
        private readonly string _environment;
        public readonly PostgreSqlContainer _postgreSqlContainer;

        /// <summary>
        /// Postgre setup in containers on constructor
        /// </summary>
        public ContactsApplication()
        {
            var initScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "init-integration.sql");

            _environment = "Local";
            _postgreSqlContainer = new PostgreSqlBuilder()
                .WithImage("postgres")
                .WithPortBinding("5432")
                .WithExposedPort("5332")
                .WithDatabase("contacts")
                .WithUsername("postgres")
                .WithPassword("test")
                .WithCleanUp(true)
                .WithAutoRemove(true)
                .Build();
        }

        public async Task InitializeDataForTest(string pathToSql)
        {
            var sqlQuery = await File.ReadAllTextAsync(pathToSql);

            var res1 = await _postgreSqlContainer.ExecScriptAsync(sqlQuery);
        }

        public async Task<ExecResult> ExecuteSqlQuery(string sqlQuery)
        {
            var res = await _postgreSqlContainer.ExecScriptAsync(sqlQuery);

            return res;
        }

        public Task InitializeAsync()
        {
            var task = _postgreSqlContainer.StartAsync();

            return task;
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            try
            {
                builder.UseEnvironment(_environment);

                builder.ConfigureServices(services =>
                {
                    services.RemoveDbContext<TContext>();

                    var test = _postgreSqlContainer.GetConnectionString();

                    services.AddDbContext<TContext>(options =>
                    {
                        options.UseNpgsql(test)
                        .UseSnakeCaseNamingConvention();
                    });

                    //services.EnsureDbCreated<TContext>();
                });

                base.ConfigureWebHost(builder);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}