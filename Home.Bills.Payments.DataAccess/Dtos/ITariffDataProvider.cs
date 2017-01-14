using System;
using System.Threading.Tasks;
using Marten;

namespace Home.Bills.Payments.DataAccess.Dtos
{
    public interface ITariffDataProvider
    {
        Task<Tariff> GetTariff(Guid tariffId);
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

        private Tariff ToDto(Domain.TariffAggregate.Tariff tariff)
        {
            if (tariff == null)
                return null;
            return new Tariff
            {
                ValidTo = tariff.ValidTo,
                Created = tariff.Created,
                Description = tariff.Description,
                Revoked = tariff.Revoked
            };
        }
    }
}