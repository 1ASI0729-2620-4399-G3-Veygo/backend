using Veygo.Platform.Communication.Application.QueryServices;
using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Queries;
using Veygo.Platform.Communication.Domain.Repositories;

namespace Veygo.Platform.Communication.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for conversations.
/// </summary>
public class ConversationQueryService(IConversationRepository conversationRepository) : IConversationQueryService
{
    public async Task<Conversation?> Handle(GetConversationByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await conversationRepository.FindByIdAsNoTrackingAsync(query.ConversationId, cancellationToken);

    public async Task<IEnumerable<Conversation>> Handle(GetConversationsByRenterIdQuery query,
        CancellationToken cancellationToken = default) =>
        await conversationRepository.FindByRenterIdAsync(query.RenterId, cancellationToken);

    public async Task<IEnumerable<Conversation>> Handle(GetConversationsByOwnerIdQuery query,
        CancellationToken cancellationToken = default) =>
        await conversationRepository.FindByOwnerIdAsync(query.OwnerId, cancellationToken);
}
