namespace Veygo.Platform.Engagement.Domain.Model.Commands;

/// <summary>
///     Command to remove a vehicle from the favorites.
/// </summary>
public record DeleteFavoriteCommand(int FavoriteId);
