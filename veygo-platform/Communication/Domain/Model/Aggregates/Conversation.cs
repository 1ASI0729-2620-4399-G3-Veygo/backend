using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Shared.Domain.Model.Aggregates.Internal;
using Veygo.Platform.Shared.Domain.Model.Entities;

namespace Veygo.Platform.Communication.Domain.Model.Aggregates;

/// <summary>
///     Conversation aggregate root: a chat between a renter and an owner, optionally about a vehicle.
/// </summary>
public class Conversation : AggregateRoot, IAuditableEntity
{
    /// <summary>Maximum length of the preview of the last message.</summary>
    public const int PreviewLength = 120;

    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    protected Conversation()
    {
        LastMessagePreview = string.Empty;
    }

    /// <summary>
    ///     Starts a conversation from the <see cref="CreateConversationCommand" />.
    /// </summary>
    /// <exception cref="ArgumentException">When the participants are not valid.</exception>
    public Conversation(CreateConversationCommand command) : this()
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.RenterId <= 0 || command.OwnerId <= 0)
            throw new ArgumentException("A conversation must have a renter and an owner.", nameof(command));
        if (command.RenterId == command.OwnerId)
            throw new ArgumentException("The participants of a conversation must be different users.",
                nameof(command));
        if (command.VehicleId is <= 0)
            throw new ArgumentException("The vehicle of a conversation is not valid.", nameof(command));

        RenterId = command.RenterId;
        OwnerId = command.OwnerId;
        VehicleId = command.VehicleId;
    }

    public int Id { get; private set; }
    public int RenterId { get; private set; }
    public int OwnerId { get; private set; }
    public int? VehicleId { get; private set; }
    public DateTimeOffset? LastMessageAt { get; private set; }
    public string LastMessagePreview { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Gets a value indicating whether the user is one of the participants.</summary>
    public bool Involves(int userId) => userId == RenterId || userId == OwnerId;

    /// <summary>Gets the other participant of the conversation.</summary>
    public int CounterpartOf(int userId) => userId == RenterId ? OwnerId : RenterId;

    /// <summary>
    ///     Records the last message sent in the conversation (used to sort and preview the inbox).
    /// </summary>
    /// <param name="content">The content of the message.</param>
    /// <param name="sentAt">When the message was sent.</param>
    public void RecordMessage(string content, DateTimeOffset sentAt)
    {
        var preview = (content ?? string.Empty).Trim();
        LastMessagePreview = preview.Length > PreviewLength ? preview[..PreviewLength] : preview;
        LastMessageAt = sentAt;
    }
}
