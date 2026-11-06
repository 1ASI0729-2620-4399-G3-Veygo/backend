using Veygo.Platform.Communication.Application.CommandServices;
using Veygo.Platform.Communication.Application.Internal.OutboundServices;
using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Communication.Domain.Model.Errors;
using Veygo.Platform.Communication.Domain.Repositories;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Communication.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for conversations.
/// </summary>
public class ConversationCommandService(
    IConversationRepository conversationRepository,
    ExternalFleetService externalFleetService,
    IUnitOfWork unitOfWork)
    : IConversationCommandService
{
    /// <inheritdoc />
    public async Task<Result<Conversation>> Handle(CreateConversationCommand command,
        CancellationToken cancellationToken = default)
    {
        var conversation = new Conversation(command);
        if (conversation.VehicleId.HasValue &&
            !await externalFleetService.ExistsVehicleAsync(conversation.VehicleId.Value, cancellationToken))
            return Result<Conversation>.Failure(CommunicationErrors.VehicleNotFound);
        if (await conversationRepository.ExistsByParticipantsAsync(conversation.RenterId, conversation.OwnerId,
                conversation.VehicleId, cancellationToken))
            return Result<Conversation>.Failure(CommunicationErrors.DuplicateConversation);

        await conversationRepository.AddAsync(conversation, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Conversation>.Success(conversation);
    }

    /// <inheritdoc />
    public async Task<Result<Conversation>> Handle(UpdateConversationLastMessageCommand command,
        CancellationToken cancellationToken = default)
    {
        var conversation = await conversationRepository.FindByIdAsync(command.ConversationId, cancellationToken);
        if (conversation is null) return Result<Conversation>.Failure(CommunicationErrors.ConversationNotFound);

        conversation.RecordMessage(command.Preview, command.LastMessageAt);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Conversation>.Success(conversation);
    }
}
