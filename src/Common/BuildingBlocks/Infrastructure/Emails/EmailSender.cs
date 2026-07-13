using Application.Emails;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Infrastructure.Emails;

public class EmailSender : IEmailSender
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _from;
    private readonly string? _user;
    private readonly string? _password;

    public EmailSender(string host, int port, string from, string? user, string? password)
    {
        _host = host;
        _port = port;
        _from = from;
        _user = user;
        _password = password;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken ct)
    {
        var mimeMessage = new MimeMessage();
        
        mimeMessage.From.Add(MailboxAddress.Parse(_from));
        mimeMessage.To.Add(MailboxAddress.Parse(message.To));
        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = new TextPart("plain") { Text = message.Body };

        using var client = new SmtpClient();
        
        await client.ConnectAsync(_host, _port, SecureSocketOptions.Auto, ct);

        if (!string.IsNullOrWhiteSpace(_user))
            await client.AuthenticateAsync(_user, _password, ct);

        await client.SendAsync(mimeMessage, ct);
        await client.DisconnectAsync(true, ct);
    }
}