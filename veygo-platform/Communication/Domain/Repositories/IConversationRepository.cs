using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Communication.Domain.Repositories;

/// <summary>
///     Repository of conversations.
/// </summary>
public interface IConversationRepository : IBaseRepository<Conversation>
{
    /// <summary>Lists the conversations of a renter, most recent activity first (read only).</summary>
    Task<IEnumerable<Conversation>> FindByRenterIdAsync(int renterId, CancellationToken cancellationToken = default);

    /// <summary>Lists the conversations of an owner, most recent activity first (read only).</summary>
    Task<IEnumerable<Conversation>> FindByOwnerIdAsync(int ownerId, CancellationToken cancellationToken = default);

    /// <summary>Checks if the renter and the owner already have a conversation about the vehicle.</summary>
    Task<bool> ExistsByParticipantsAsync(int renterId, int ownerId, int? vehicleId,
        CancellationToken cancellationToken = default);
}
