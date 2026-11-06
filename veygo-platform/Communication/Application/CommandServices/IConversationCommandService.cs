using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Shared.Application.Model;

namespace Veygo.Platform.Communication.Application.CommandServices;

/// <summary>
///     Command service for conversations.
/// </summary>
public interface IConversationCommandService
{
    /// <summary>Starts a conversation between a renter and an owner.</summary>
    Task<Result<Conversation>> Handle(CreateConversationCommand command, CancellationToken cancellationToken = default);

    /// <summary>Updates the preview of the last message.</summary>
    Task<Result<Conversation>> Handle(UpdateConversationLastMessageCommand command,
        CancellationToken cancellationToken = default);
}
