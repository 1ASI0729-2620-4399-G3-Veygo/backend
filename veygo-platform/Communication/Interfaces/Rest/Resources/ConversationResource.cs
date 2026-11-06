namespace Veygo.Platform.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Conversation between a renter and an owner.
/// </summary>
public record ConversationResource(
    int Id,
    int RenterId,
    int OwnerId,
    int? VehicleId,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? LastMessageAt,
    string LastMessagePreview);
