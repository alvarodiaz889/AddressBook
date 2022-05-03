using AddressBook.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Text.Encodings.Web;

namespace AddressBook.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailRequest em);
        // Task<bool> SendForgotPasswordEmail(string code, string email);
    }
    public class EmailService : IEmailSender
    {
        private readonly EmailSettingsOptions _settings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailSettingsOptions> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(EmailRequest em)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_settings.Email));
                email.To.Add(MailboxAddress.Parse(em.ToEmail));
                email.Subject = em.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = em.Body };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_settings.Email, _settings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(1), ex, ex.ToString());
                return await Task.FromResult(false);
            }
        }
    }
}