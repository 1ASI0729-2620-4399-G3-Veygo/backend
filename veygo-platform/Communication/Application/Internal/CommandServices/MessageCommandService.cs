using Veygo.Platform.Communication.Application.CommandServices;
using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Communication.Domain.Model.Errors;
using Veygo.Platform.Communication.Domain.Repositories;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Communication.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for messages.
/// </summary>
public class MessageCommandService(
    IMessageRepository messageRepository,
    IConversationRepository conversationRepository,
    IUnitOfWork unitOfWork)
    : IMessageCommandService
{
    /// <inheritdoc />
    public async Task<Result<Message>> Handle(CreateMessageCommand command, CancellationToken cancellationToken = default)
    {
        var conversation = await conversationRepository.FindByIdAsync(command.ConversationId, cancellationToken);
        if (conversation is null) return Result<Message>.Failure(CommunicationErrors.ConversationNotFound);
        if (!conversation.Involves(command.SenderId))
            return Result<Message>.Failure(CommunicationErrors.SenderNotParticipant);

        var message = new Message(command);
        var sentAt = DateTimeOffset.UtcNow;
        message.CreatedAt = sentAt;
        conversation.RecordMessage(message.Content, sentAt);

        await messageRepository.AddAsync(message, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Message>.Success(message);
    }

    /// <inheritdoc />
    public async Task<Result<Message>> Handle(MarkMessageAsReadCommand command,
        CancellationToken cancellationToken = default)
    {
        var message = await messageRepository.FindByIdAsync(command.MessageId, cancellationToken);
        if (message is null) return Result<Message>.Failure(CommunicationErrors.MessageNotFound);

        message.MarkAsRead();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Message>.Success(message);
    }
}
