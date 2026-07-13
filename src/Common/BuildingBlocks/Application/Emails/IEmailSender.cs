using BuildingBlock.Domain;

namespace Application.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage message, CancellationToken ct);
}