using Veygo.Platform.Engagement.Domain.Model.Aggregates;
using Veygo.Platform.Engagement.Domain.Model.Queries;

namespace Veygo.Platform.Engagement.Application.QueryServices;

/// <summary>
///     Query service for favorites.
/// </summary>
public interface IFavoriteQueryService
{
    Task<IEnumerable<Favorite>> Handle(GetFavoritesByRenterIdQuery query, CancellationToken cancellationToken = default);
    Task<Favorite?> Handle(GetFavoriteByIdQuery query, CancellationToken cancellationToken = default);
}
