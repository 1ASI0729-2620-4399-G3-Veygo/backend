using Veygo.Platform.Engagement.Application.CommandServices;
using Veygo.Platform.Engagement.Application.Internal.OutboundServices;
using Veygo.Platform.Engagement.Domain.Model.Aggregates;
using Veygo.Platform.Engagement.Domain.Model.Commands;
using Veygo.Platform.Engagement.Domain.Model.Errors;
using Veygo.Platform.Engagement.Domain.Repositories;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Engagement.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for favorites.
/// </summary>
public class FavoriteCommandService(
    IFavoriteRepository favoriteRepository,
    ExternalFleetService externalFleetService,
    IUnitOfWork unitOfWork)
    : IFavoriteCommandService
{
    /// <inheritdoc />
    public async Task<Result<Favorite>> Handle(CreateFavoriteCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!await externalFleetService.ExistsVehicleAsync(command.VehicleId, cancellationToken))
            return Result<Favorite>.Failure(EngagementErrors.VehicleNotFound);
        if (await favoriteRepository.ExistsByRenterIdAndVehicleIdAsync(command.RenterId, command.VehicleId,
                cancellationToken))
            return Result<Favorite>.Failure(EngagementErrors.DuplicateFavorite);

        var favorite = new Favorite(command);
        await favoriteRepository.AddAsync(favorite, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Favorite>.Success(favorite);
    }

    /// <inheritdoc />
    public async Task<Result<Favorite>> Handle(DeleteFavoriteCommand command,
        CancellationToken cancellationToken = default)
    {
        var favorite = await favoriteRepository.FindByIdAsync(command.FavoriteId, cancellationToken);
        if (favorite is null) return Result<Favorite>.Failure(EngagementErrors.FavoriteNotFound);

        favoriteRepository.Remove(favorite);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Favorite>.Success(favorite);
    }
}
