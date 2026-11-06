using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Queries;

namespace Veygo.Platform.Communication.Application.QueryServices;

/// <summary>
///     Query service for messages.
/// </summary>
public interface IMessageQueryService
{
    Task<IEnumerable<Message>> Handle(GetMessagesByConversationIdQuery query, CancellationToken cancellationToken = default);
    Task<Message?> Handle(GetMessageByIdQuery query, CancellationToken cancellationToken = default);
}
