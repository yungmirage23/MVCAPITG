namespace RestaurantMVC.Infrastructure.EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
