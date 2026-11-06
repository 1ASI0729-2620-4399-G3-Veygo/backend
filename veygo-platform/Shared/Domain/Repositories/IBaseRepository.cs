namespace Veygo.Platform.Shared.Domain.Repositories;

/// <summary>
///     Base repository interface for all repositories.
/// </summary>
/// <remarks>
///     This interface defines the basic CRUD operations for all repositories.
/// </remarks>
/// <typeparam name="TEntity">The entity type for the repository.</typeparam>
public interface IBaseRepository<TEntity>
{
    /// <summary>
    ///     Adds an entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds an entity by its id (tracked, so it can be modified).
    /// </summary>
    /// <param name="id">The id of the entity to find.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The entity if found, otherwise null.</returns>
    Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds an entity by its id without change tracking (read only).
    /// </summary>
    /// <param name="id">The id of the entity to find.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The entity if found, otherwise null.</returns>
    Task<TEntity?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Marks an entity as updated.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    ///     Removes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    void Remove(TEntity entity);

    /// <summary>
    ///     Lists all entities in the repository.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of all entities in the repository.</returns>
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Lists all entities in the repository without change tracking.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of all entities in the repository.</returns>
    Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(CancellationToken cancellationToken = default);
}
