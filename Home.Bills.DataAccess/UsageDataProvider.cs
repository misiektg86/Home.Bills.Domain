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

        public async Task<IEnumerable<Dto.Usage>> GetUsages(Guid addressId)
            =>
                await _documentSession.Query<MeterRead>()
                    .Where(i => i.AddressId == addressId)
                    .SelectMany(i=>i.
                        i =>
                            new Dto.Usage()
                            {
                                AddressId = i.AddressId,
                                UsageId = i.Id,
                                MeterId = i.MeterId,
                                ReadDateTime = i.ReadDateTime,
                                Value = i.Value
                            })
                    .ToListAsync();

        public async Task<Dto.Usage> GetLastUsage(Guid address)
            => await _documentSession.Query<Usage>().Where(i => i.AddressId == address).Select(i => new Dto.Usage()
            {
                AddressId = i.AddressId,
                UsageId = i.Id,
                MeterId = i.MeterId,
                ReadDateTime = i.ReadDateTime,
                Value = i.Value,
                CurrentRead = i.CurrentRead
            }).FirstOrDefaultAsync();
    }
}