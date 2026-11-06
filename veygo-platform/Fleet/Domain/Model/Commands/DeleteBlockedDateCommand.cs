namespace Veygo.Platform.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to release a blocked day.
/// </summary>
public record DeleteBlockedDateCommand(int BlockedDateId);
