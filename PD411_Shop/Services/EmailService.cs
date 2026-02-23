using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using PD411_Shop.Settings;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.JavaScript;

namespace PD411_Shop.Services
{
    public class EmailService : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _email;
        private readonly string _password;

        private readonly SmtpClient _smtpClient;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            try
            {
                var settings = smtpOptions.Value;

                _host = settings.Host;
                _port = settings.Port;
                _email = settings.Email;
                _password = settings.Password;

                _smtpClient = new SmtpClient(_host, _port);
                _smtpClient.Credentials = new NetworkCredential(_email, _password);
                _smtpClient.EnableSsl = true;
            }
            catch (Exception)
            {
            }
            
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_email);
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                mailMessage.Body = htmlMessage;
                mailMessage.IsBodyHtml = true;

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {

            }           
        }
    }
}
