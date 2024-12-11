using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SL.Authentication
{
    public class EmailService : IEmailService
    {
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SenderEmail = "jeimymorales234@gmail.com";
        private const string SenderPassword = "wwxxktzzhyszqywj";
        private const string SenderName = "Gestión de productos";

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
