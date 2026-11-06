using System.Text.Json;
using Veygo.Platform.Notification.Domain.Model.Commands;
using Veygo.Platform.Notification.Domain.Model.ValueObjects;
using Veygo.Platform.Shared.Domain.Model.Aggregates.Internal;
using Veygo.Platform.Shared.Domain.Model.Entities;

namespace Veygo.Platform.Notification.Domain.Model.Aggregates;

/// <summary>
///     Notification aggregate root: an in-app alert for a user.
/// </summary>
/// <remarks>
///     <see cref="Params" /> holds the values used to translate the message (e.g. <c>{"vehicle":"Toyota RAV4"}</c>)
///     and <see cref="Link" /> the route of the web application to open (e.g. <c>{"name":"owner-bookings"}</c>),
///     both as JSON objects.
/// </remarks>
public class Notification : AggregateRoot, IAuditableEntity
{
    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    protected Notification()
    {
        Params = "{}";
    }

    /// <summary>
    ///     Creates a notification from the <see cref="CreateNotificationCommand" />.
    /// </summary>
    /// <exception cref="ArgumentException">When the recipient or the JSON values are not valid.</exception>
    public Notification(CreateNotificationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.UserId <= 0) throw new ArgumentException("A notification must have a recipient.", nameof(command));

        UserId = command.UserId;
        Type = command.Type;
        Params = EnsureJsonObject(command.Params, "params") ?? "{}";
        Link = EnsureJsonObject(command.Link, "link");
        Read = false;
    }

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public ENotificationType Type { get; private set; }
    public string Params { get; private set; }
    public string? Link { get; private set; }
    public bool Read { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Marks the notification as read.</summary>
    public void MarkAsRead() => Read = true;

    private static string? EnsureJsonObject(string? json, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(json) || json.Trim() == "null") return null;
        try
        {
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.ValueKind != JsonValueKind.Object)
                throw new ArgumentException($"The notification {fieldName} must be a JSON object.", fieldName);
            return document.RootElement.GetRawText();
        }
        catch (JsonException)
        {
            throw new ArgumentException($"The notification {fieldName} must be a JSON object.", fieldName);
        }
    }
}
