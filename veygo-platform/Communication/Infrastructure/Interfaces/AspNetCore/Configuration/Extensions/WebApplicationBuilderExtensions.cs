using Veygo.Platform.Communication.Application.CommandServices;
using Veygo.Platform.Communication.Application.Internal.CommandServices;
using Veygo.Platform.Communication.Application.Internal.OutboundServices;
using Veygo.Platform.Communication.Application.Internal.QueryServices;
using Veygo.Platform.Communication.Application.QueryServices;
using Veygo.Platform.Communication.Domain.Repositories;
using Veygo.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Communication.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Dependency injection configuration of the Communication bounded context.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    public static void AddCommunicationContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<IConversationCommandService, ConversationCommandService>();
        builder.Services.AddScoped<IConversationQueryService, ConversationQueryService>();
        builder.Services.AddScoped<IMessageCommandService, MessageCommandService>();
        builder.Services.AddScoped<IMessageQueryService, MessageQueryService>();
        builder.Services.AddScoped<ExternalFleetService>();
    }
}
