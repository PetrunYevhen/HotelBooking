using Application.Emails;
using BuildingBlock.Domain;
using MediatR;
using Notifications.Domain.Entities;
using Notifications.Domain.Entities.Enums;
using Notifications.Domain.RepositoryContract;

namespace Notifications.Application.Command.SendBookingCompletedNotification;

public class SendBookingCompletedNotificationCommandHandler : IRequestHandler<SendBookingCompletedNotificationCommand, Result>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IEmailSender _emailSender;

    public SendBookingCompletedNotificationCommandHandler(IEmailSender emailSender, INotificationRepository notificationRepository)
    {
        _emailSender = emailSender;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result> Handle(SendBookingCompletedNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(
            request.UserId,
            request.RecipientEmail,
            NotificationType.Email,
            "Booking Completed",
            $"Your booking {request.BookingId} has been completed.");
        
        if (notification.IsFailure)
            return Result.Failure(notification.Error);

        await _notificationRepository.AddAsync(notification.Value, cancellationToken);

        try
        {
            await _emailSender.SendEmailAsync(
                new EmailMessage(
                    request.RecipientEmail,
                    "Booking Completed",
                    $"Your booking {request.BookingId} has been completed."),
                cancellationToken);

            notification.Value.MarkAsSent();
        }
        catch (Exception ex)
        {
            notification.Value.MarkAsFailed(ex.Message);
        }

        return Result.Success();
    }
}