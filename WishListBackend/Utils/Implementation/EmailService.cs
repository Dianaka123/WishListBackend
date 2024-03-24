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

        public async Task<bool> SendEmailAsync(Message message)
        {
            var mail = CreateMailMessage(message);
            return await TrySendAsync(mail);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await ConnectToStmp(client);
                    smtpClient = client;
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using (smtpClient) { 
                if(smtpClient != null && smtpClient.IsConnected)
                {
                    await smtpClient.DisconnectAsync(true);
                    smtpClient.Dispose();
                }
            }
        }

        private MimeMessage CreateMailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _configuration.From));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2>{0}</h2>", message.Content) };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task<bool> TrySendAsync(MimeMessage message)
        {
            var isSuccess = true;
            try
            {
                using(smtpClient)
                {
                    await ConnectToStmp(smtpClient);
                    await smtpClient.SendAsync(message);
                }
            }
            catch
            {
                isSuccess = false;
                throw;
            }

            return isSuccess;
        }

        private async Task ConnectToStmp(SmtpClient client)
        {
            if (!client.IsConnected)
            {
                await client.ConnectAsync(_configuration.SmtpServer, _configuration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_configuration.UserName, _configuration.Password);
            }
        }
    }
}
