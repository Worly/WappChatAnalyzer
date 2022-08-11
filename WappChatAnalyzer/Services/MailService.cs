using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mail);
    }

    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            var mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
            if (webHostEnvironment.IsProduction() && Environment.GetEnvironmentVariable("MAIL_MAIL") != null)
            {
                var mail = Environment.GetEnvironmentVariable("MAIL_MAIL");
                var displayName = Environment.GetEnvironmentVariable("MAIL_DISPLAY_NAME");
                if (displayName == null)
                    throw new ArgumentNullException("MAIL_DISPLAY_NAME");

                var password = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
                if (password == null)
                    throw new ArgumentNullException("MAIL_PASSWORD");

                var host = Environment.GetEnvironmentVariable("MAIL_HOST");
                if (host == null)
                    throw new ArgumentNullException("MAIL_HOST");

                var port = Environment.GetEnvironmentVariable("MAIL_PORT");
                if (port == null)
                    throw new ArgumentNullException("MAIL_PORT");

                if (!int.TryParse(port, out int portInt))
                    throw new ArgumentException("Invalid format", "MAIL_PORT");

                mailSettings = new MailSettings()
                {
                    Mail = mail,
                    DisplayName = displayName,
                    Password = password,
                    Host = host,
                    Port = portInt
                };
            }

            if (mailSettings == null)
                throw new ArgumentException("Missing configuration", "MailSettings");

            this._mailSettings = mailSettings;
        }

        public async Task SendEmailAsync(MailRequest mail)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mail.ToEmail));
            email.Subject = mail.Subject;
            var builder = new BodyBuilder();
            if (mail.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mail.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mail.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}

public class MailSettings
{
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
