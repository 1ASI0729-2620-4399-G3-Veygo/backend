using Veygo.Platform.Engagement.Domain.Model.Aggregates;
using Veygo.Platform.Engagement.Domain.Model.Commands;
using Veygo.Platform.Shared.Application.Model;

namespace Veygo.Platform.Engagement.Application.CommandServices;

/// <summary>
///     Command service for favorites.
/// </summary>
public interface IFavoriteCommandService
{
    /// <summary>Saves a vehicle in the favorites of a renter.</summary>
    Task<Result<Favorite>> Handle(CreateFavoriteCommand command, CancellationToken cancellationToken = default);

    /// <summary>Removes a favorite.</summary>
    Task<Result<Favorite>> Handle(DeleteFavoriteCommand command, CancellationToken cancellationToken = default);
}
