using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="IConversationRepository" />.
/// </summary>
public class ConversationRepository(AppDbContext context)
    : BaseRepository<Conversation>(context), IConversationRepository
{
    public async Task<IEnumerable<Conversation>> FindByRenterIdAsync(int renterId,
        CancellationToken cancellationToken = default) =>
        await MostRecentFirst(Context.Set<Conversation>().AsNoTracking()
                .Where(conversation => conversation.RenterId == renterId))
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Conversation>> FindByOwnerIdAsync(int ownerId,
        CancellationToken cancellationToken = default) =>
        await MostRecentFirst(Context.Set<Conversation>().AsNoTracking()
                .Where(conversation => conversation.OwnerId == ownerId))
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByParticipantsAsync(int renterId, int ownerId, int? vehicleId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<Conversation>().AnyAsync(conversation =>
                conversation.RenterId == renterId && conversation.OwnerId == ownerId &&
                conversation.VehicleId == vehicleId,
            cancellationToken);

    private static IQueryable<Conversation> MostRecentFirst(IQueryable<Conversation> conversations) =>
        conversations.OrderByDescending(conversation => conversation.LastMessageAt ?? conversation.CreatedAt);
}
