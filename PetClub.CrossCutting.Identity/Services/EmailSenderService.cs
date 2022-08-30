using PetClub.CrossCutting.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.CrossCutting.Identity.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private string Host { get; set; }
        private int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        private bool EnableSSL { get; set; }

        public EmailSenderService(string host, int port, string userName, string password, bool enableSsl)
        {
            Host = host;
            Port = port;
            UserName = userName;
            Password = password;
            EnableSSL = enableSsl;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(Host, Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(UserName, Password),
                DeliveryMethod = SmtpDeliveryMethod.Network

            };

            return client.SendMailAsync(
                new MailMessage(UserName, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}
