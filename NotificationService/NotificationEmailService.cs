using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;

namespace NotificationService
{
    public class NotificationEmailService : INotificationEmailService
    {
        public async Task ProcessMessageAsync(string message)
        {
            var notification = JsonConvert.DeserializeObject<NotificationMessage>(message);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Finance Control", "vitor.financecontrol@outlook.com"));
            emailMessage.To.Add(new MailboxAddress(notification.Email, notification.Email));
            emailMessage.Subject = notification.Subject;
            emailMessage.Body = new TextPart("html") { Text = notification.Body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("vitorgutierrez.silva@gmail.com", "wllf fafw tbff bxnz");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
