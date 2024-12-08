using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Envía un correo electrónico de forma asincrónica.
        /// </summary>
        /// <param name="recipientEmail">Correo del destinatario.</param>
        /// <param name="subject">Asunto del correo.</param>
        /// <param name="body">Cuerpo del correo, en HTML o texto plano.</param>
        /// <returns>Una tarea que representa la operación de envío.</returns>
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}