using Veygo.Platform.Shared.Domain.Model.Errors;

namespace Veygo.Platform.Communication.Domain.Model.Errors;

/// <summary>
///     Error catalog of the Communication bounded context.
/// </summary>
public static class CommunicationErrors
{
    public static readonly Error ConversationNotFound =
        Error.NotFound("Communication.ConversationNotFound", "The conversation was not found.");

    public static readonly Error DuplicateConversation =
        Error.Conflict("Communication.DuplicateConversation",
            "A conversation between these users about this vehicle already exists.");

    public static readonly Error VehicleNotFound =
        Error.NotFound("Communication.VehicleNotFound", "The vehicle of the conversation was not found.");

    public static readonly Error MessageNotFound =
        Error.NotFound("Communication.MessageNotFound", "The message was not found.");

    public static readonly Error SenderNotParticipant =
        Error.Forbidden("Communication.SenderNotParticipant",
            "Only the participants of a conversation can send messages in it.");
}
