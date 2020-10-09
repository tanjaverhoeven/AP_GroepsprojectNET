using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace DotnetAcademy.DAL.Services {
    public class EmailService : IIdentityMessageService {
        public Task SendAsync(IdentityMessage message) {
            string username = "ebillingdotnet@gmail.com";
            string password = "eBilling123";

            SmtpClient client = new SmtpClient {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password)
            };

            return client.SendMailAsync(username, message.Destination, message.Subject, message.Body);
        }
    }
}