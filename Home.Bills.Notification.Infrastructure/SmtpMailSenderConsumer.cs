using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using Home.Bills.Notifications.Messages;
using MassTransit;

namespace Home.Bills.Notification.Infrastructure
{
    public class SmtpMailSenderConsumer : IConsumer<INotification>
    {
        private readonly SmtpAccountDataAccess _dataAccess;
        public SmtpMailSenderConsumer(SmtpAccountDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task Consume(ConsumeContext<INotification> context)
        {
            var smtpAccount = await _dataAccess.GetSmtpAccount();

            using (SmtpClient smtpServer = new SmtpClient(smtpAccount.Server, smtpAccount.Port))
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(smtpAccount.FromAddress);
                    mail.To.Add(context.Message.ToAddress);
                    mail.Subject = context.Message.Subject;
                    mail.Body = context.Message.Message;
                    mail.IsBodyHtml = true;
                    if (!string.IsNullOrEmpty(context.Message.CcAddress))
                        mail.CC.Add(context.Message.CcAddress);

                    SecureString ss = new SecureString();
                    smtpAccount.Password.ToList().ForEach(c => ss.AppendChar(c));

                    smtpServer.Credentials = new System.Net.NetworkCredential(smtpAccount.UserName, ss);

                    smtpServer.EnableSsl = smtpAccount.EnableSSl;

                    await smtpServer.SendMailAsync(mail);
                }
            }

            await context.Publish<INotificationSent>(new { context.Message.NotificationId });
        }
    }
}