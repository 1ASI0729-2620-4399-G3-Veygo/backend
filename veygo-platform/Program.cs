using Veygo.Platform.Booking.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Veygo.Platform.Communication.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Veygo.Platform.Engagement.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Veygo.Platform.Fleet.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Veygo.Platform.Notification.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Veygo.Platform.Reputation.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Veygo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Seeding;
using SharedConfiguration = Veygo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions.WebApplicationBuilderExtensions;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Lowercase URLs and kebab-case controller routes (e.g. /api/v1/blocked-dates)
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Localization of error messages (Shared/Resources/Errors/ErrorMessages*.resx)
builder.Services.AddLocalization();

// OpenAPI document (/openapi/v1.json) and Scalar API reference (/scalar)
builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    document.Info.Title = "Veygo Platform API";
    document.Info.Description =
        "RESTful API of Veygo (Zyntra): vehicle rental marketplace. Bounded contexts: Fleet, Booking, Engagement, Reputation, Communication and Notification.";
    return Task.CompletedTask;
}));

// Bounded contexts
builder.AddSharedContextServices();
builder.AddFleetContextServices();
builder.AddBookingContextServices();
builder.AddEngagementContextServices();
builder.AddReputationContextServices();
builder.AddCommunicationContextServices();
builder.AddNotificationContextServices();

var app = builder.Build();

// Create the database schema and load the sample data (Database:SeedSampleData)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    if (app.Configuration.GetValue<bool>("Database:SeedSampleData"))
        await scope.ServiceProvider.GetRequiredService<SampleDataSeeder>()
            .SeedAsync(Path.Combine(AppContext.BaseDirectory, "SampleData", "veygo-sample-data.json"));
}

// Localization runs first so errors and documentation follow the Accept-Language header
string[] supportedCultures = ["en", "en-US", "es", "es-PE"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseExceptionHandler();
app.UseCors(SharedConfiguration.WebApplicationCorsPolicy);

app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();

app.Run();
