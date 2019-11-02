using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZipTest.Tests
{
    /// <summary>
    /// A factory for creating test database and service collection instances.
    /// </summary>
    public class TestBase
    {
        public ServiceProvider Provider { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }

        public DbContextFactory ContextFactory { get; private set; }

        protected TestBase()
        {
            ContextFactory = new DbContextFactory();

            var services = new ServiceCollection();
            services.AddMediatR(typeof(Startup));

            services.AddSingleton(ContextFactory.CreateContext());
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());
            services.AddSingleton(Mapper.Instance);
            Provider = services.BuildServiceProvider();

            Configuration = GetIConfigurationRoot("");
        }

        public static IConfigurationRoot GetIConfigurationRoot(string basePath)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
