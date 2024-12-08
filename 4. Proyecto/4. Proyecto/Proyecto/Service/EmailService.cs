using Common.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Service
{
    public class EmailService : IEmailService
    {
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SenderEmail = "jeimymorales234@gmail.com";
        private const string SenderPassword = "wwxxktzzhyszqywj";
        private const string SenderName = "jeimy morales";

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            using (var smtp = new SmtpClient(SmtpServer, SmtpPort))
            {
                smtp.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                smtp.EnableSsl = true;

                var mail = new MailMessage
                {
                    From = new MailAddress(SenderEmail, SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(recipientEmail);

                await smtp.SendMailAsync(mail);
            }
        }
    }
}