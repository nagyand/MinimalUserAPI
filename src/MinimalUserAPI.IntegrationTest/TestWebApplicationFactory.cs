using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalUserAPI.Application.Extensions;
using MinimalUserAPI.Application.Interfaces;
using MinimalUserAPI.Application.Users.Services;
using MinimalUserAPI.Infrastructure.DbContext;
using MinimalUserAPI.Infrastructure.Extensions;
using MinimalUserAPI.Infrastructure.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MinimalUserAPI.IntegrationTest;
public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;

            config
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                config.AddUserSecrets(appAssembly, optional: true);
            }
        });
        builder.ConfigureServices((HostBuilderContext hostContext,IServiceCollection services) =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(UserDbContext));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.ConfigureInfrastructure(hostContext.Configuration);
            services.ConfigureApplication();
            services.AddTransient<IUserService, UserService>();
        });

        return base.CreateHost(builder);
    }
}
