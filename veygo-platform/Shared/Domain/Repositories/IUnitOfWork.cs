namespace Veygo.Platform.Shared.Domain.Repositories;

/// <summary>
///     Unit of work interface for all repositories.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Commits all pending changes to the database in a single transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task CompleteAsync(CancellationToken cancellationToken = default);
}
