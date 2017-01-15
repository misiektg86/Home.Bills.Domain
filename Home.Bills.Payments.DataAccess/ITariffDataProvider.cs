using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;

namespace Home.Bills.Payments.DataAccess.Dtos
{
    public interface ITariffDataProvider
    {
        Task<Tariff> GetTariff(Guid tariffId);
        Task<IEnumerable<Tariff>> GetTariffs();
    }

    public class TariffDataProvider : ITariffDataProvider
    {
        private readonly IDocumentStore _documentStore;

        public TariffDataProvider(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<Tariff> GetTariff(Guid tariffId)
        {
            using (var lSession = _documentStore.LightweightSession())
            {
                return
                    ToDto(
                        await lSession.Query<Domain.TariffAggregate.Tariff>()
                            .FirstOrDefaultAsync(tariff => tariff.Id == tariffId));
            }
        }

        public async Task<IEnumerable<Tariff>> GetTariffs()
        {
            using (var lSession = _documentStore.LightweightSession())
            {
                return
                    (await lSession.Query<Domain.TariffAggregate.Tariff>().ToListAsync())?.Select(ToDto).ToList();
            }
        }

        private Tariff ToDto(Domain.TariffAggregate.Tariff tariff)
        {
            if (tariff == null)
                return null;
            return new Tariff
            {
                ValidTo = tariff.ValidTo,
                Created = tariff.Created,
                Description = tariff.Description,
                Revoked = tariff.Revoked,
                TariffId = tariff.Id,
                Price = tariff.TariffValue
            };
        }
    }
}