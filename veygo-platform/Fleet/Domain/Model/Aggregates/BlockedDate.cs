using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Shared.Domain.Model.Aggregates.Internal;
using Veygo.Platform.Shared.Domain.Model.Entities;

namespace Veygo.Platform.Fleet.Domain.Model.Aggregates;

/// <summary>
///     A day in which the owner made a vehicle unavailable for rentals.
/// </summary>
public class BlockedDate : AggregateRoot, IAuditableEntity
{
    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    protected BlockedDate()
    {
    }

    /// <summary>
    ///     Creates a blocked day from the <see cref="CreateBlockedDateCommand" />.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <exception cref="ArgumentException">When the vehicle is not valid.</exception>
    public BlockedDate(CreateBlockedDateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.VehicleId <= 0)
            throw new ArgumentException("A blocked date must reference a vehicle.", nameof(command));
        VehicleId = command.VehicleId;
        Date = command.Date;
    }

    public int Id { get; private set; }
    public int VehicleId { get; private set; }
    public DateOnly Date { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the blocked day falls inside the given rental period.
    /// </summary>
    /// <param name="startDate">First day of the period.</param>
    /// <param name="endDate">Last day of the period.</param>
    public bool FallsWithin(DateOnly startDate, DateOnly endDate) => Date >= startDate && Date <= endDate;
}
