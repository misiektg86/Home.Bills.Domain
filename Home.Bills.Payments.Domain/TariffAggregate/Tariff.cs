using System;
using Frameworks.Light.Ddd;
using MassTransit;
using Newtonsoft.Json;

namespace Home.Bills.Payments.Domain.TariffAggregate
{
    public class Tariff : AggregateRoot<Guid>
    {
        private decimal _tariffValue;
        public DateTime Created { get; private set; }
        public DateTime? ValidTo { get; private set; }

        public bool Revoked { get; private set; }

        public string Description { get; private set; }

        [JsonIgnore]
        public decimal TariffValue => _tariffValue;

        internal Tariff() { }

        internal Tariff(Guid tariffId, DateTime created, DateTime? validTo, decimal tariffValue, string description, IBus messageBus) : base(messageBus)
        {
            _tariffValue = tariffValue;
            Created = created;
            ValidTo = validTo;
            Description = description;
            Id = tariffId;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Renews tariff.
        /// </summary>
        /// <param name="validTo">Tariff expiration date.</param>
        /// <exception cref="TariffExpiredException">Throws exception if tariff expiration date is less than current dateTime.</exception>
        public void RenewTariff(DateTime validTo)
        {
            if (validTo < DateTime.Now)
                throw new TariffExpiredException(Id.ToString());

            ValidTo = validTo;
        }

        public void RevokeTariff()
        {
            Revoked = true;

            Publish(new TariffRevoked { TariffId = Id });
        }

    }
}