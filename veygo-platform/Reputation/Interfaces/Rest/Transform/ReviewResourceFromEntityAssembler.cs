using Veygo.Platform.Reputation.Domain.Model.Aggregates;
using Veygo.Platform.Reputation.Interfaces.Rest.Resources;

namespace Veygo.Platform.Reputation.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="ReviewResource" /> from a <see cref="Review" />.
/// </summary>
public static class ReviewResourceFromEntityAssembler
{
    public static ReviewResource ToResourceFromEntity(Review entity) =>
        new(entity.Id, entity.BookingId, entity.VehicleId, entity.OwnerId, entity.RenterId, entity.Rating,
            entity.Comment, entity.CreatedAt);
}
