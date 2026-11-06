using Veygo.Platform.Engagement.Application.QueryServices;
using Veygo.Platform.Engagement.Domain.Model.Aggregates;
using Veygo.Platform.Engagement.Domain.Model.Queries;
using Veygo.Platform.Engagement.Domain.Repositories;

namespace Veygo.Platform.Engagement.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for favorites.
/// </summary>
public class FavoriteQueryService(IFavoriteRepository favoriteRepository) : IFavoriteQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Favorite>> Handle(GetFavoritesByRenterIdQuery query,
        CancellationToken cancellationToken = default) =>
        await favoriteRepository.FindByRenterIdAsync(query.RenterId, cancellationToken);

    /// <inheritdoc />
    public async Task<Favorite?> Handle(GetFavoriteByIdQuery query, CancellationToken cancellationToken = default) =>
        await favoriteRepository.FindByIdAsNoTrackingAsync(query.FavoriteId, cancellationToken);
}
