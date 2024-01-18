using ag.ems.application.Common;

namespace ag.ems.application.Interfaces;

public interface IEmailService
{
    Result SendUserInvitationEmail(string password, string receiver,string role);
    Task<Result> SendPasswordResetEmailAsync(string link, string receiver);
  

}