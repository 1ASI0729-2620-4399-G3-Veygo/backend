using Veygo.Platform.Booking.Application.CommandServices;
using Veygo.Platform.Booking.Application.Internal.CommandServices;
using Veygo.Platform.Booking.Application.Internal.OutboundServices;
using Veygo.Platform.Booking.Application.Internal.QueryServices;
using Veygo.Platform.Booking.Application.QueryServices;
using Veygo.Platform.Booking.Domain.Repositories;
using Veygo.Platform.Booking.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Veygo.Platform.Booking.Interfaces.ACL;
using Veygo.Platform.Booking.Interfaces.ACL.Internal;

namespace Veygo.Platform.Booking.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Dependency injection configuration of the Booking bounded context.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    public static void AddBookingContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IBookingCommandService, BookingCommandService>();
        builder.Services.AddScoped<IBookingQueryService, BookingQueryService>();
        builder.Services.AddScoped<ExternalFleetService>();
        builder.Services.AddScoped<IBookingContextFacade, BookingContextFacade>();
    }
}