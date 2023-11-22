using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace TreinShop.Util.Mail
{
    public interface IEmailSend
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
    public class EmailSend : IEmailSend
    {
        private readonly EmailSettings _emailSettings;
        public EmailSend(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(
            string email, string subject, string message)
        {
            var mail = new MailMessage();  // aanmaken van een mail‐object
            mail.To.Add(new MailAddress(email));
            mail.From = new
                    MailAddress("cdw_mvccore@gmail.com");  // hier komt jullie Gmail‐adres //dit wordt nog overschreven in *hier
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            //mail.Attachments = //een attachment
            try
            {
                using (var smtp = new SmtpClient(_emailSettings.MailServer))
                {
                    smtp.Port = _emailSettings.MailPort;
                    smtp.EnableSsl = true;
                    smtp.Credentials =
                        new NetworkCredential(_emailSettings.Sender, //<---- *hier
                                                _emailSettings.Password);
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
