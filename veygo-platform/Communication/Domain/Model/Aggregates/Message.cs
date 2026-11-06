using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Shared.Domain.Model.Aggregates.Internal;
using Veygo.Platform.Shared.Domain.Model.Entities;

namespace Veygo.Platform.Communication.Domain.Model.Aggregates;

/// <summary>
///     A message sent by a participant of a conversation. Its <see cref="CreatedAt" /> is the time it was sent.
/// </summary>
public class Message : AggregateRoot, IAuditableEntity
{
    /// <summary>Maximum length of a message.</summary>
    public const int MaxContentLength = 1000;

    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    protected Message()
    {
        Content = string.Empty;
    }

    /// <summary>
    ///     Creates a message from the <see cref="CreateMessageCommand" />.
    /// </summary>
    /// <exception cref="ArgumentException">When the message is not valid.</exception>
    public Message(CreateMessageCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.ConversationId <= 0)
            throw new ArgumentException("A message must belong to a conversation.", nameof(command));
        if (command.SenderId <= 0) throw new ArgumentException("A message must have a sender.", nameof(command));
        var content = command.Content?.Trim() ?? string.Empty;
        if (content.Length == 0) throw new ArgumentException("The message cannot be empty.", nameof(command));
        if (content.Length > MaxContentLength)
            throw new ArgumentException($"The message must have at most {MaxContentLength} characters.",
                nameof(command));

        ConversationId = command.ConversationId;
        SenderId = command.SenderId;
        Content = content;
        Read = false;
    }

    public int Id { get; private set; }
    public int ConversationId { get; private set; }
    public int SenderId { get; private set; }
    public string Content { get; private set; }
    public bool Read { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Marks the message as read by the receiver.</summary>
    public void MarkAsRead() => Read = true;
}
