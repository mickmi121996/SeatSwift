using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace AppGestion.Tools
{
    public static class EmailTools
    {
        /// <summary>
        /// Send an email with SMTP2GO
        /// </summary>
        /// <param name="destinataire">The email of the recipient</param>
        /// <param name="sujet">The subject of the email</param>
        /// <param name="contenu">The content of the email</param>
        public static async Task SendEmailWithSMTP2GO(
            string destinataire,
            string sujet,
            string contenu
        )
        {
            // Create the email
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("SeatSwift", "tcbmtimesheet@outlook.com"));
            message.To.Add(new MailboxAddress("Le destinataire", destinataire));
            message.Subject = sujet;
            message.Body = new TextPart("html") { Text = contenu };

            // Send the email
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("mail.smtp2go.com", 2525, false);
                    await client.AuthenticateAsync("tcbmteam", "TyebTenTyeb12$3");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
