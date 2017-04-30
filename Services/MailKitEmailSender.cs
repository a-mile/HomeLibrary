using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace HomeLibrary.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("HomeLibrary", "und3fnd@outlook.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.live.com", 587, false).ConfigureAwait(false);
                await client.AuthenticateAsync("und3fnd@outlook.com","gkafvumcrsft11").ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}