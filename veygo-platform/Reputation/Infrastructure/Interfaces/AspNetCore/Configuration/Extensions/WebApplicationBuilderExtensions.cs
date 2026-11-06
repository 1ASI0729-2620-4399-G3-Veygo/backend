using Veygo.Platform.Reputation.Application.CommandServices;
using Veygo.Platform.Reputation.Application.Internal.CommandServices;
using Veygo.Platform.Reputation.Application.Internal.OutboundServices;
using Veygo.Platform.Reputation.Application.Internal.QueryServices;
using Veygo.Platform.Reputation.Application.QueryServices;
using Veygo.Platform.Reputation.Domain.Repositories;
using Veygo.Platform.Reputation.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Reputation.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Dependency injection configuration of the Reputation bounded context.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    public static void AddReputationContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IReviewCommandService, ReviewCommandService>();
        builder.Services.AddScoped<IReviewQueryService, ReviewQueryService>();
        builder.Services.AddScoped<ExternalBookingService>();
    }
}
