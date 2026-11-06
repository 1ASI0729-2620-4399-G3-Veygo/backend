using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Shared.Application.Model;

namespace Veygo.Platform.Communication.Application.CommandServices;

/// <summary>
///     Command service for messages.
/// </summary>
public interface IMessageCommandService
{
    /// <summary>Sends a message and updates the last message of its conversation.</summary>
    Task<Result<Message>> Handle(CreateMessageCommand command, CancellationToken cancellationToken = default);

    /// <summary>Marks a message as read.</summary>
    Task<Result<Message>> Handle(MarkMessageAsReadCommand command, CancellationToken cancellationToken = default);
}
