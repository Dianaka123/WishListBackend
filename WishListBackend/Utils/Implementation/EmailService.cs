using mailslurp.Api;
using mailslurp.Client;
using mailslurp.Model;
using WishListBackend.Utils.Interfaces;
using WishListBackend.Views;

namespace WishListBackend.Utils.Implementation
{
    public class EmailService : IEmailService
    {
        private const string API_KEY = "0b690e9331fc38dd15da95ae9ce5c41678a32b567d8a99b6c7be6357b3c5f620";

        private InboxControllerApi inboxController;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var configuration = new Configuration();
            configuration.ApiKey.Add("x-api-key", API_KEY);
            inboxController = new InboxControllerApi(configuration);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailOptions = CreateEmailOptions(message);
            var inbox = await inboxController.CreateInboxWithDefaultsAsync();

            await inboxController.SendEmailAndConfirmAsync(inbox.Id, emailOptions);
        }

        private SendEmailOptions CreateEmailOptions(Message message)
        {
            return new SendEmailOptions
            {
                To = new List<string>() { message.To },
                Subject = message.Subject,
                Body = string.Format("<h1>{0}</h1>", message.Content),
                UseInboxName = true
            };
        }
    }
}
