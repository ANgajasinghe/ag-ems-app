using ag.ems.application.Common;
using ag.ems.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ag.ems.infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationConfig _config;

        public EmailService(ApplicationConfig config)
        {
            _config = config;
        }

        public Task<Result> SendPasswordResetEmailAsync(string link, string receiver)
        {
            throw new NotImplementedException();
        }

        public Result SendUserInvitationEmail(string password, string receiver, string role)
        {
            var subject = "Password";
            var body = $"<h2> Your password is</h2> <h1>{password}</h1>";
            var mail = new MailMessage(_config.EmailSettings.FromAddress, receiver, subject, body);
            mail.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient(_config.EmailSettings.SmtpServer, _config.EmailSettings.SmtpPort);
            smtpClient.EnableSsl = false;

            try
            {
                smtpClient.Send(mail);
                return Result.Success();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send email: " + ex.Message);
            }
            finally
            {
                mail.Dispose();
                smtpClient.Dispose();
            }
        }
    }
}
