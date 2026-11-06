namespace Veygo.Platform.Communication.Domain.Model.Commands;

/// <summary>
///     Command to start a conversation between a renter and an owner (optionally about a vehicle).
/// </summary>
public record CreateConversationCommand(int RenterId, int OwnerId, int? VehicleId);
