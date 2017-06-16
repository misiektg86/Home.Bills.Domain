using System.Net.Mail;
using System.Threading.Tasks;
using Home.Bills.Notifications.Messages;
using Marten;

namespace Home.Bills.Notification.Infrastructure
{
    public class SmtpAccountDataAccess
    {
        private readonly IDocumentStore _documentStore;

        public SmtpAccountDataAccess(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<SmtpAccount> GetSmtpAccount()
        {
            using (var lightSession = _documentStore.LightweightSession())
            {
                return await lightSession.Query<SmtpAccount>().FirstOrDefaultAsync();
            }
        }

        public async Task AddSmtpAccount(SmtpAccount account)
        {
            using (var lightSession = _documentStore.LightweightSession())
            {
                lightSession.Store(account);

                await lightSession.SaveChangesAsync();
            }
        }

        public async Task EditSmtpAccount(SmtpAccount account)
        {
            using (var lightSession = _documentStore.LightweightSession())
            {
                lightSession.Delete(account);
                lightSession.Store(account);

                await lightSession.SaveChangesAsync();
            }
        }
    }
}