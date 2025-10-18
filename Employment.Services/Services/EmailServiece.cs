using Job.Core.Helper;
using Job.Services.JobServices.IServices;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Services
{
    public class EmailServiece:IEmailService
    {
        private readonly SmtpSettings _smtps;
        public EmailServiece(IOptions<SmtpSettings> opt) { _smtps = opt.Value; }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("JobApp", _smtps.From.Trim()));
            msg.To.Add(new MailboxAddress("", email.Trim()));
            msg.Subject = subject;
            msg.Body = new BodyBuilder { HtmlBody = htmlMessage }.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_smtps.Host, _smtps.Port, _smtps.UseSsl);

            if (!string.IsNullOrWhiteSpace(_smtps.User))
                await client.AuthenticateAsync(_smtps.User, _smtps.Password);

            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}
