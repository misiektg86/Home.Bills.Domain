using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.Consumers;
using Home.Bills.Payments.Domain.PaymentAggregate;
using Home.Bills.Payments.Domain.RegistratorAgregate;
using Home.Bills.Payments.Domain.TariffAggregate;
using Marten;

namespace Home.Bills.Payments.Domain.Services
{
    public class PaymentDomainService
    {
        private readonly IAggregateFactory<Payment, PaymentFactoryInput, Guid> _aggregateFactory;
        private readonly IRepository<Registrator, Guid> _registratorRepository;
        private readonly IRepository<Tariff, Guid> _tariffRepository;
        private readonly IRepository<Payment, Guid> _paymentRepository;

        public PaymentDomainService(IAggregateFactory<Payment, PaymentFactoryInput, Guid> aggregateFactory,
            IRepository<Registrator, Guid> registratorRepository, IRepository<Tariff, Guid> tariffRepository,
            IRepository<Payment, Guid> paymentRepository)
        {
            _aggregateFactory = aggregateFactory;
            _registratorRepository = registratorRepository;
            _tariffRepository = tariffRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task CreatePayment(Guid paymentId, Guid addressId, IEnumerable<RegisteredUsage> registeredUsages)
        {
            var payment =
                _aggregateFactory.Create(new PaymentFactoryInput() { AddressId = addressId, PaymentId = paymentId });

            foreach (var registeredUsage in registeredUsages)
            {
                var registrator = await _registratorRepository.Get(registeredUsage.MeterId);

                if (!registrator.TariffId.HasValue)
                {
                    throw new TariffNotAssignedException(registrator.Id.ToString());
                }

                var tariff = await _tariffRepository.Get(registrator.TariffId.Value);

                if (tariff.Revoked)
                {
                    throw new TariffRevokedException(tariff.Id.ToString());
                }

                if (tariff.ValidTo.HasValue && DateTime.Now >= tariff.ValidTo)
                {
                    throw new TariffExpiredException(tariff.Id.ToString());
                }

                var amountToPay = new decimal(registeredUsage.Value) * tariff.TariffValue;

                PaymentItem item = new PaymentItem($"({tariff.Description}) {registrator.Description}.", amountToPay);

                payment.AddPaymentItem(item);
            }

            _paymentRepository.Add(payment);
        }
    }
}