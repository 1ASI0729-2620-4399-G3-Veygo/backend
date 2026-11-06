using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="IMessageRepository" />.
/// </summary>
public class MessageRepository(AppDbContext context) : BaseRepository<Message>(context), IMessageRepository
{
    public async Task<IEnumerable<Message>> FindByConversationIdAsync(int conversationId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<Message>().AsNoTracking()
            .Where(message => message.ConversationId == conversationId)
            .OrderBy(message => message.CreatedAt)
            .ThenBy(message => message.Id)
            .ToListAsync(cancellationToken);
}
