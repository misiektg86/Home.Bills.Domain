using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Home.Bills.Client;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.Consumers;
using Home.Bills.Payments.Domain.PaymentAggregate;
using Home.Bills.Payments.Domain.RegistratorAgregate;
using Home.Bills.Payments.Domain.RentAggregate;
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
        private readonly IRepository<Rent, Guid> _rentRepository;
        private readonly IRepository<Address, Guid> _addresRepository;
        private readonly IServiceClient _serviceClient;

        public PaymentDomainService(IAggregateFactory<Payment, PaymentFactoryInput, Guid> aggregateFactory,
            IRepository<Registrator, Guid> registratorRepository, IRepository<Tariff, Guid> tariffRepository,
            IRepository<Payment, Guid> paymentRepository, IRepository<Rent, Guid> rentRepository, IRepository<Address, Guid> addresRepository, IServiceClient serviceClient)
        {
            _aggregateFactory = aggregateFactory;
            _registratorRepository = registratorRepository;
            _tariffRepository = tariffRepository;
            _paymentRepository = paymentRepository;
            _rentRepository = rentRepository;
            _addresRepository = addresRepository;
            _serviceClient = serviceClient;
        }

        public async Task CreatePayment(Guid paymentId, Guid addressId, IEnumerable<RegisteredUsage> registeredUsages)
        {
            var payment =
                _aggregateFactory.Create(new PaymentFactoryInput() { AddressId = addressId, PaymentId = paymentId });

            var lastUsage = (await _serviceClient.GetLastUsageForAddress(payment.AddressId))?.ToList();

            var address = await _addresRepository.Get(addressId);

            if (address == null)
            {
                throw new AddressNotFoundException(addressId.ToString());
            }

            if (address.RentId.HasValue)
            {
                var rent = await _rentRepository.Get(address.RentId.Value);

                if (rent == null)
                {
                    throw new RentNotFoundException(address.RentId.ToString());
                }

                foreach (var rentItem in rent.RentItems)
                {
                    switch (rentItem.RentUnit)
                    {
                        case RentUnit.Person:
                            payment.AddPaymentItem(new PaymentItem(rentItem.Description, rentItem.AmountPerUnit * address.Persons));
                            break;
                        case RentUnit.SquareMeter:
                            payment.AddPaymentItem(new PaymentItem(rentItem.Description, rentItem.AmountPerUnit * new decimal(address.SquareMeters)));
                            break;
                        case RentUnit.Constant:
                            payment.AddPaymentItem(new PaymentItem(rentItem.Description, rentItem.AmountPerUnit));
                            break;
                    }
                }
            }

            foreach (var registeredUsage in registeredUsages)
            {
                var registrator = await _registratorRepository.Get(registeredUsage.MeterId);

                var registratorDetails = lastUsage.Single(i => i.MeterId == registrator.Id); // TODO autonomy !!

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

                PaymentItem item = new PaymentItem($"Stan licznika: {registratorDetails.CurrentRead} m3 ({tariff.Description}) {registrator.Description} - Zużycie : {registratorDetails.Value} m3", amountToPay); //TODO m3 :(

                payment.AddPaymentItem(item);
            }

            _paymentRepository.Add(payment);
        }
    }
}