using MimeKit;

namespace WishListBackend.Views
{
    public record Message(MailboxAddress To,
        string Subject,
        string Content);
}
