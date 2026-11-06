using System.ComponentModel.DataAnnotations;

namespace Veygo.Platform.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Data required to start a conversation.
/// </summary>
public record CreateConversationResource(
    [Range(1, int.MaxValue)] int RenterId,
    [Range(1, int.MaxValue)] int OwnerId,
    int? VehicleId);
