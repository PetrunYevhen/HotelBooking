namespace Infrastructure.Emails;

public sealed record SmtpSettings(string Host, int Port, string From, string? User, string? Password);
