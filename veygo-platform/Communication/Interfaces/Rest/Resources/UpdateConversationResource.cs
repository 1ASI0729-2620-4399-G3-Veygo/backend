namespace Veygo.Platform.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Preview of the last message of a conversation.
/// </summary>
public record UpdateConversationResource(string? LastMessagePreview, DateTimeOffset? LastMessageAt);
