using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Communication.Domain.Model.Aggregates;

namespace Veygo.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     EF Core model configuration of the Communication bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the Communication mappings (conversations and messages).
    /// </summary>
    /// <param name="builder">The model builder.</param>
    public static void ApplyCommunicationConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Conversation>(conversation =>
        {
            conversation.HasKey(c => c.Id);
            conversation.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
            conversation.Property(c => c.RenterId).IsRequired();
            conversation.Property(c => c.OwnerId).IsRequired();
            conversation.Property(c => c.LastMessagePreview).HasMaxLength(Conversation.PreviewLength);
            conversation.HasIndex(c => c.RenterId);
            conversation.HasIndex(c => c.OwnerId);
        });

        builder.Entity<Message>(message =>
        {
            message.HasKey(m => m.Id);
            message.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
            message.Property(m => m.ConversationId).IsRequired();
            message.Property(m => m.SenderId).IsRequired();
            message.Property(m => m.Content).IsRequired().HasMaxLength(Message.MaxContentLength);
            message.Property(m => m.Read).IsRequired();
            message.HasIndex(m => m.ConversationId);
        });
    }
}
