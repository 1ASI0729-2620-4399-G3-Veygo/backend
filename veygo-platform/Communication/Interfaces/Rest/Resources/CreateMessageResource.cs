using System.ComponentModel.DataAnnotations;

namespace Veygo.Platform.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Data required to send a message.
/// </summary>
public record CreateMessageResource(
    [Range(1, int.MaxValue)] int ConversationId,
    [Range(1, int.MaxValue)] int SenderId,
    [Required] [MaxLength(1000)] string Content);
