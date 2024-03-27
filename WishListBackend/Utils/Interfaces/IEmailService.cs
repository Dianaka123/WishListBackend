using WishListBackend.Views;

namespace WishListBackend.Utils.Interfaces
{
    public interface IEmailService : IHostedService
    {
        public Task SendEmailAsync(Message message);
    }
}
