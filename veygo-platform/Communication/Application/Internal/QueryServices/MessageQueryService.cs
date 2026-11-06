using Veygo.Platform.Communication.Application.QueryServices;
using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Queries;
using Veygo.Platform.Communication.Domain.Repositories;

namespace Veygo.Platform.Communication.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for messages.
/// </summary>
public class MessageQueryService(IMessageRepository messageRepository) : IMessageQueryService
{
    public async Task<IEnumerable<Message>> Handle(GetMessagesByConversationIdQuery query,
        CancellationToken cancellationToken = default) =>
        await messageRepository.FindByConversationIdAsync(query.ConversationId, cancellationToken);

    public async Task<Message?> Handle(GetMessageByIdQuery query, CancellationToken cancellationToken = default) =>
        await messageRepository.FindByIdAsNoTrackingAsync(query.MessageId, cancellationToken);
}
