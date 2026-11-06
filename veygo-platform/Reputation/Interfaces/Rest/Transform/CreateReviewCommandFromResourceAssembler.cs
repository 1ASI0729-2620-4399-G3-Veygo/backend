using Veygo.Platform.Reputation.Domain.Model.Commands;
using Veygo.Platform.Reputation.Interfaces.Rest.Resources;

namespace Veygo.Platform.Reputation.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="CreateReviewCommand" /> from a <see cref="CreateReviewResource" />.
/// </summary>
public static class CreateReviewCommandFromResourceAssembler
{
    public static CreateReviewCommand ToCommandFromResource(CreateReviewResource resource) =>
        new(resource.BookingId, resource.RenterId, resource.Rating, resource.Comment ?? string.Empty);
}
