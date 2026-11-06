namespace Veygo.Platform.Communication.Domain.Model.Commands;

/// <summary>
///     Command to mark a message as read.
/// </summary>
public record MarkMessageAsReadCommand(int MessageId);
