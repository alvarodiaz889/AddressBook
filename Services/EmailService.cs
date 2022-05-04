using AddressBook.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Text;
using System.Text.Encodings.Web;

namespace AddressBook.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailRequest em);
        Task<bool> SendForgotPasswordEmail(string email, string uri);
    }
    public class EmailService : IEmailSender
    {
        private readonly EmailSettingsOptions _settings;
        private readonly ILogger<EmailService> _logger;
        private readonly UserManager<AppUser> _userManager;
        public EmailService(IOptions<EmailSettingsOptions> settings,
            ILogger<EmailService> logger,
            UserManager<AppUser> userManager)
        {
            _settings = settings.Value;
            _logger = logger;
            _userManager = userManager;
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

        public async Task<bool> SendForgotPasswordEmail(string email, string uri)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return await Task.FromResult(false);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var builder = new UriBuilder(uri);
            builder.Path = "Identity/Account/ResetPassword";
            builder.Query = $"code={code}";
            var callbackUrl = builder.ToString();

            var emailRequest = new EmailRequest
            {
                Body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                ToEmail = email,
                Subject = "Reset Password"
            };
            return await SendEmailAsync(emailRequest);
        }
    }
}