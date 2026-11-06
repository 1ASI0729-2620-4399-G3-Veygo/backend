using Veygo.Platform.Engagement.Domain.Model.Commands;
using Veygo.Platform.Shared.Domain.Model.Aggregates.Internal;
using Veygo.Platform.Shared.Domain.Model.Entities;

namespace Veygo.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
///     A vehicle saved by a renter in their favorites list.
/// </summary>
public class Favorite : AggregateRoot, IAuditableEntity
{
    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    protected Favorite()
    {
    }

    /// <summary>
    ///     Creates a favorite from the <see cref="CreateFavoriteCommand" />.
    /// </summary>
    /// <exception cref="ArgumentException">When the renter or the vehicle are not valid.</exception>
    public Favorite(CreateFavoriteCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.RenterId <= 0) throw new ArgumentException("A favorite must reference a renter.", nameof(command));
        if (command.VehicleId <= 0)
            throw new ArgumentException("A favorite must reference a vehicle.", nameof(command));
        RenterId = command.RenterId;
        VehicleId = command.VehicleId;
    }

    public int Id { get; private set; }
    public int RenterId { get; private set; }
    public int VehicleId { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }
}
