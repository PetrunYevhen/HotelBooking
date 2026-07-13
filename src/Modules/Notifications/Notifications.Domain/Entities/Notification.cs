using BuildingBlock.Domain;
using Notifications.Domain.Entities.Enums;

namespace Notifications.Domain.Entities;

public class Notification
{
    public NotificationId NotificationId { get; private set; }
    public Guid UserId { get; private set; }
    public string RecipientEmail { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationStatus Status { get; private set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? SentAt { get; private set; }
    
    private  Notification() {}

    private Notification(
        Guid userId, 
        string recipientEmail, 
        NotificationType type, 
        string subject,
        string content)
    {
        NotificationId = NotificationId.New();
        UserId = userId;
        RecipientEmail = recipientEmail;
        Type = type;
        Status = NotificationStatus.Pending;
        Subject = subject;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<Notification> Create(
        Guid userId,
        string recipientEmail,
        NotificationType type,
        string subject,
        string content)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Notification>(new Error("Notification.InvalidUserId", "UserId cannot be empty."));

        if (string.IsNullOrWhiteSpace(recipientEmail))
            return Result.Failure<Notification>(new Error("Notification.InvalidEmail", "Email is required."));

        if (string.IsNullOrWhiteSpace(subject))
            return Result.Failure<Notification>(new Error("Notification.InvalidSubject", "Subject is required."));

        if (string.IsNullOrWhiteSpace(content))
            return Result.Failure<Notification>(new Error("Notification.InvalidContent", "Content is required."));
        
        return Result.Success(new Notification(userId, recipientEmail, type, subject, content));
    }
    
    public Result MarkAsSent()
    {
        if (Status == NotificationStatus.Sent)
            return Result.Failure(new Error("Notification.AlreadySent", "Notification already sent."));

        Status = NotificationStatus.Sent;
        SentAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result MarkAsFailed(string failureReason)
    {
        if (Status == NotificationStatus.Sent)
            return Result.Failure(new Error("Notification.AlreadySent", "Cannot fail already sent notification."));

        if (string.IsNullOrWhiteSpace(failureReason))
            return Result.Failure(new Error("Notification.InvalidReason", "Failure failureReason is required."));

        Status = NotificationStatus.Failed;
        FailureReason = failureReason;
        return Result.Success();
    }
}
