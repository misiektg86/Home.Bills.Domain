using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Home.Bills.Domain.MeterReadAggregate;
using Marten;

namespace Home.Bills.DataAccess
{
    public class UsageDataProvider : IUsageDataProvider
    {
        private IDocumentSession _documentSession;

        public UsageDataProvider(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<IEnumerable<Dto.Usage>> GetLastUsages(Guid addressId)
        {
            var metrRead =
            await _documentSession.Query<MeterRead>()
                    .Where(i => i.AddressId == addressId).OrderByDescending(read => read.ReadBeginDateTime).Take(1).ToListAsync();

            return metrRead?.SelectMany(i => i.Usages).Select(
                i =>
                    new Dto.Usage()
                    {
                        AddressId = i.AddressId,
                        UsageId = i.Id,
                        MeterId = i.MeterId,
                        ReadDateTime = i.ReadDateTime,
                        Value = i.Value,
                        CurrentRead = i.CurrentRead,
                        PrevioudRead = i.PrevioudRead
                    }).ToList();
        }

        public async Task<IEnumerable<Dto.Usage>> GetUsages(Guid addressId)
        {
            var meterRead = await _documentSession.Query<MeterRead>()
                .Where(i => i.AddressId == addressId).ToListAsync();

            return meterRead?.SelectMany(read => read.Usages).Select(i => new Dto.Usage()
            {
                AddressId = i.AddressId,
                UsageId = i.Id,
                MeterId = i.MeterId,
                ReadDateTime = i.ReadDateTime,
                Value = i.Value,
                CurrentRead = i.CurrentRead,
                PrevioudRead = i.PrevioudRead
            });
        }
    }
}