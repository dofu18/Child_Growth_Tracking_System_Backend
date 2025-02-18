using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;


namespace InfrastructureLayer.Core.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class MailService : IMailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public MailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            // log to console email and password
            Console.WriteLine($"SMTP Email: {smtpUsername}");
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Child Growth System", _smtpUsername));
            mimeMessage.To.Add(new MailboxAddress("Receiver Name", email));

            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }

}
