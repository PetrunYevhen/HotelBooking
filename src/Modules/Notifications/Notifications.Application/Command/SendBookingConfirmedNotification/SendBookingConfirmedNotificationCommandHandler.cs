using Application.Emails;
using BuildingBlock.Domain;
using MediatR;
using Notifications.Domain.Entities;
using Notifications.Domain.Entities.Enums;
using Notifications.Domain.RepositoryContract;

namespace Notifications.Application.Command.SendBookingConfirmedNotification;

public class SendBookingConfirmedNotificationCommandHandler 
    : IRequestHandler<SendBookingConfirmedNotificationCommand, Result>
{
    private readonly IEmailSender _emailSender;
    private readonly INotificationRepository _notificationRepository;

    public SendBookingConfirmedNotificationCommandHandler(IEmailSender emailSender, INotificationRepository notificationRepository)
    {
        _emailSender = emailSender;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result> Handle(SendBookingConfirmedNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(
            request.UserId,
            request.RecipientEmail,
            NotificationType.Email,
            "Booking Confirmed",
            $"Your booking {request.BookingId} has been confirmed.");
        
        if (notification.IsFailure)
            return Result.Failure(notification.Error);

        await _notificationRepository.AddAsync(notification.Value, cancellationToken);

        try
        {
            await _emailSender.SendEmailAsync(
                new EmailMessage(
                    request.RecipientEmail,
                    "Booking Confirmed",
                    $"Your booking {request.BookingId} has been confirmed."),
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