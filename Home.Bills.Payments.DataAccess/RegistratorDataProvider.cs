using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Payments.DataAccess.Dtos;
using Marten;

namespace Home.Bills.Payments.DataAccess
{
    public class RegistratorDataProvider : IRegistratorDataProvider
    {
        private readonly IDocumentStore _documentStore;
        public RegistratorDataProvider(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<IEnumerable<Registrator>> GetRegistrators()
        {
            using (var lwSession = _documentStore.LightweightSession())
            {
                return
                    (await lwSession.Query<Domain.RegistratorAgregate.Registrator>().ToListAsync()).Select(ToDto)
                    .ToList();
            }
        }

        private Registrator ToDto(Domain.RegistratorAgregate.Registrator source)
        {
            return new Registrator()
            {
                AddressId = source.AddressId,
                Description = source.Description,
                RegistratorId = source.Id,
                TariffId = source.TariffId
            };
        }
    }
}