using System;
using System.Threading.Tasks;
using Home.Bills.DataAccess.Dto;
using Marten;

namespace Home.Bills.DataAccess
{
    public interface IMeterDataProvider
    {
        Task<Meter> GetMeter(Guid meterId);
    }

    internal class MeterDataProvider : IMeterDataProvider
    {
        private readonly IDocumentSession _session;

        public MeterDataProvider(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<Meter> GetMeter(Guid meterId)
        {
            var meter = await _session.Query<Domain.MeterAggregate.Meter>().FirstOrDefaultAsync(i => i.Id == meterId);

            if (meter == null)
            {
                return null;
            }

            return new Meter
            {
                AddressId = meter.AddressId,
                MeterId = meter.Id,
                SerialNumber = meter.SerialNumber,
                State = meter.State
            };
        }
    }
}