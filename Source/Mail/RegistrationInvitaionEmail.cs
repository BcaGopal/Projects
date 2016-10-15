using Model.Models;
using System.Configuration;
using Mailer;
using Mailer.Model;
using System.Threading.Tasks;

namespace EmailContents
{
    public class RegistrationInvitaionEmail
    {
        public async Task SendUserRegistrationInvitation(string UserId, int ApplicationId, string RefereeId, string ReferralId, string UserType, string CC)
        {
            EmailMessage message = new EmailMessage();
            message.Subject = "Invitation for registration";
            //string temp = (ConfigurationManager.AppSettings["SalesManager"]);
            string domain = ConfigurationManager.AppSettings["LoginDomain"];
            message.To = UserId;
            string link = domain + "/Account/Register?uid=" + UserId + "&aid=" + ApplicationId + "&rfeid=" + RefereeId + "&rflid=" + ReferralId + "&utype=" + UserType;

            SaleOrderHeader doc = new SaleOrderHeader();

            message.Body += "Please use the link to register to the company. <a href='" + link + "' target='_blank'> Click Here </a>";

            if (!string.IsNullOrEmpty(CC))
                message.CC = CC;

            SendEmail se = new SendEmail();
            await se.configSendGridasync(message);
        }

    }
}
