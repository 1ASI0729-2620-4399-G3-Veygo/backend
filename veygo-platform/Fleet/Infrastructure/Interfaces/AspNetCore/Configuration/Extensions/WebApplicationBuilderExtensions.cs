using Veygo.Platform.Fleet.Application.CommandServices;
using Veygo.Platform.Fleet.Application.Internal.CommandServices;
using Veygo.Platform.Fleet.Application.Internal.QueryServices;
using Veygo.Platform.Fleet.Application.QueryServices;
using Veygo.Platform.Fleet.Domain.Repositories;
using Veygo.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Veygo.Platform.Fleet.Interfaces.ACL;
using Veygo.Platform.Fleet.Interfaces.ACL.Internal;

namespace Veygo.Platform.Fleet.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Dependency injection configuration of the Fleet bounded context.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    public static void AddFleetContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        builder.Services.AddScoped<IBlockedDateRepository, BlockedDateRepository>();
        builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
        builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();
        builder.Services.AddScoped<IBlockedDateCommandService, BlockedDateCommandService>();
        builder.Services.AddScoped<IBlockedDateQueryService, BlockedDateQueryService>();
        builder.Services.AddScoped<IFleetContextFacade, FleetContextFacade>();
    }
}
