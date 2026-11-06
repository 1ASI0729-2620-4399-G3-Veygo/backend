using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Queries;

namespace Veygo.Platform.Communication.Application.QueryServices;

/// <summary>
///     Query service for conversations.
/// </summary>
public interface IConversationQueryService
{
    Task<Conversation?> Handle(GetConversationByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Conversation>> Handle(GetConversationsByRenterIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Conversation>> Handle(GetConversationsByOwnerIdQuery query, CancellationToken cancellationToken = default);
}
