using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Veygo.Platform.Shared.Domain.Model.Aggregates;

namespace Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

/// <summary>
///     EF Core interceptor that publishes the domain events recorded on aggregate roots
///     <b>after</b> the changes were saved successfully (ADR: events are only dispatched if persistence succeeds).
/// </summary>
/// <remarks>
///     Dispatching after the save guarantees that database generated ids are already assigned and that
///     no handler runs for changes that were finally rolled back.
/// </remarks>
public sealed class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    /// <inheritdoc />
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    /// <inheritdoc />
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context, CancellationToken cancellationToken = default)
    {
        if (context is null) return;

        var aggregateRoots = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(aggregateRoot => aggregateRoot.DomainEvents)
            .ToList();

        foreach (var aggregateRoot in aggregateRoots) aggregateRoot.ClearDomainEvents();

        foreach (var domainEvent in domainEvents) await mediator.PublishAsync(domainEvent, cancellationToken);
    }
}
