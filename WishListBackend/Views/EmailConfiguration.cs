namespace WishListBackend.Views
{
    public record EmailConfiguration(string From,
        string SmtpServer,
        int Port, 
        string UserName, 
        string Password);
}
