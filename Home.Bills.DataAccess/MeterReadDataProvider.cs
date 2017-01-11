using System;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.DataAccess.Dto;
using Marten;

namespace Home.Bills.DataAccess
{
    public class MeterReadDataProvider : IMeterReadDataProvider
    {
        private readonly IDocumentSession _documentSession;

        public MeterReadDataProvider(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<MeterRead> GetMeterRead(Guid meterReadId)
        {
            var meterRead = await _documentSession.LoadAsync<Domain.MeterReadAggregate.MeterRead>(meterReadId);

            if (meterRead == null)
                return null;

            return new MeterRead()
            {
                MeterReadId = meterRead.Id,
                AddressId = meterRead.AddressId,
                IsCompleted = meterRead.IsCompleted,
                Meters = meterRead.Meters.ToList(),
                ReadBeginDateTime = meterRead.ReadBeginDateTime,
                Usages = meterRead.Usages.Select(ToDto).ToList()
            };
        }

        private Usage ToDto(Domain.MeterReadAggregate.Usage entity)
        {
            return new Usage
            {
                AddressId = entity.AddressId,
                CurrentRead = entity.CurrentRead,
                MeterReadId = entity.MeterReadId,
                MeterId = entity.MeterId,
                PrevioudRead = entity.PrevioudRead,
                ReadDateTime = entity.ReadDateTime,
                Value = entity.Value
            };
        }
    }
}