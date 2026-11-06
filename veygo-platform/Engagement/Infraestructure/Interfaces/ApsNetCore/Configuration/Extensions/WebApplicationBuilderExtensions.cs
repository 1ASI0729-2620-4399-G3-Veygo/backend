using Veygo.Platform.Engagement.Application.CommandServices;
using Veygo.Platform.Engagement.Application.Internal.CommandServices;
using Veygo.Platform.Engagement.Application.Internal.OutboundServices;
using Veygo.Platform.Engagement.Application.Internal.QueryServices;
using Veygo.Platform.Engagement.Application.QueryServices;
using Veygo.Platform.Engagement.Domain.Repositories;
using Veygo.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Engagement.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Dependency injection configuration of the Engagement bounded context.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    public static void AddEngagementContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        builder.Services.AddScoped<IFavoriteCommandService, FavoriteCommandService>();
        builder.Services.AddScoped<IFavoriteQueryService, FavoriteQueryService>();
        builder.Services.AddScoped<ExternalFleetService>();
    }
}
