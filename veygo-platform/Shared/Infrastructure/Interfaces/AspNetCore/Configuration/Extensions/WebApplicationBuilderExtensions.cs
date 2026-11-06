using System.Text.RegularExpressions;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Shared.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Seeding;
using Veygo.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace Veygo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Dependency injection configuration of the Shared bounded context (persistence, mediator, error handling, CORS).
/// </summary>
public static partial class WebApplicationBuilderExtensions
{
    /// <summary>Name of the CORS policy for the Veygo web application.</summary>
    public const string WebApplicationCorsPolicy = "AllowVeygoWebApplication";

    public static void AddSharedContextServices(this WebApplicationBuilder builder)
    {
        // RFC 9457 Problem Details for unhandled exceptions
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        // CORS for the web application (origins in Cors:AllowedOrigins)
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        builder.Services.AddCors(options => options.AddPolicy(WebApplicationCorsPolicy,
            policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

        // Database: MySQL through EF Core. Domain events are dispatched after each successful save.
        builder.Services.AddScoped<DispatchDomainEventsInterceptor>();
        builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionStringTemplate))
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
            var missingVariable = UnresolvedVariableRegex().Match(connectionString);
            if (missingVariable.Success)
                throw new InvalidOperationException(
                    $"Connection string 'DefaultConnection' uses the environment variable {missingVariable.Value}, which is not defined.");

            options.UseMySQL(connectionString)
                .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
                .AddInterceptors(serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>())
                .EnableDetailedErrors();

            if (builder.Environment.IsDevelopment())
                options.EnableSensitiveDataLogging();
        });

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<SampleDataSeeder>();

        // Cortex Mediator: publishes domain events to their handlers (IEventHandler<TEvent>)
        builder.Services.AddCortexMediator([typeof(Program)]);
    }

    [GeneratedRegex("%[A-Za-z_][A-Za-z0-9_]*%")]
    private static partial Regex UnresolvedVariableRegex();
}
