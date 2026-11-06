namespace Veygo.Platform.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Partial update of a message. Only <c>read: true</c> is supported.
/// </summary>
public record UpdateMessageResource(bool? Read);
