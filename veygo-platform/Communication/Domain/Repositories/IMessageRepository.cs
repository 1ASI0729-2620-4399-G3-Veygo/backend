using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Communication.Domain.Repositories;

/// <summary>
///     Repository of messages.
/// </summary>
public interface IMessageRepository : IBaseRepository<Message>
{
    /// <summary>Lists the messages of a conversation, oldest first (read only).</summary>
    Task<IEnumerable<Message>> FindByConversationIdAsync(int conversationId,
        CancellationToken cancellationToken = default);
}
