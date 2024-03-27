using MailKit.Net.Smtp;
using MimeKit;
using WishListBackend.Utils.Interfaces;
using WishListBackend.Views;

namespace WishListBackend.Utils.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _configuration;

        private SmtpClient smtpClient;
        public EmailService(EmailConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(Message message)
        {
            var mail = CreateMailMessage(message);
            await SendAsync(mail);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            smtpClient = new SmtpClient();
            await TryConnectToStmp(smtpClient);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(!smtpClient.IsConnected)
            {
                return;
            }

            await smtpClient.DisconnectAsync(true);
            smtpClient.Dispose();
        }

        private MimeMessage CreateMailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _configuration.From));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h1>{0}</h1>", message.Content) };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage message)
        {
            await TryConnectToStmp(smtpClient);
            await smtpClient.SendAsync(message);
        }

        private async Task TryConnectToStmp(SmtpClient client)
        {
            if (client.IsConnected)
            {
                return;
            }

            await client.ConnectAsync(_configuration.SmtpServer, _configuration.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_configuration.UserName, _configuration.Password);
        }
    }
}
