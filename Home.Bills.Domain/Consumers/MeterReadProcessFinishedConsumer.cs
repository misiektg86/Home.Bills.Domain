﻿using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.Messages;
using Home.Bills.Domain.MeterReadAggregate;
using Marten;
using MassTransit;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadProcessFinishedConsumer : IConsumer<IMeterReadProcessFinished>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IRepository<MeterRead, Guid> _meterReadRepository;
        public MeterReadProcessFinishedConsumer(IDocumentSession documentSession, IRepository<MeterRead, Guid> meterReadRepository)
        {
            _documentSession = documentSession;
            _meterReadRepository = meterReadRepository;
        }
        public async Task Consume(ConsumeContext<IMeterReadProcessFinished> context)
        {
            var meterRead = await _meterReadRepository.Get(context.Message.MeterReadId);

            meterRead.CompleteMeterRead();

            _meterReadRepository.Update(meterRead);

            await _documentSession.SaveChangesAsync();
        }
    }
}