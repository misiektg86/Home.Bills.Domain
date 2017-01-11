using System;
using Frameworks.Light.Ddd;

namespace Home.Bills.Payments.Domain.TariffAggregate
{
    public class Tariff : AggregateRoot<Guid>
    {
        private readonly decimal _tariffValue;
        public DateTime Created { get; }
        public DateTime? ValidTo { get; private set; }

        public bool Revoked { get; private set; }

        /// <summary>
        /// Gets tariff value.
        /// </summary>
        /// <exception cref="TariffExpiredException">Throws exception when tariff is expired.</exception>
        public decimal TariffValue
        {
            get
            {
                if (Revoked)
                {
                    throw new TariffRevokedException(Id.ToString());
                }

                if (ValidTo.HasValue && DateTime.Now >= ValidTo)
                {
                    throw new TariffExpiredException(Id.ToString());
                }

                return _tariffValue;
            }
        }

        internal Tariff() { }

        internal Tariff(Guid tariffId, DateTime created, DateTime? validTo, decimal tariffValue)
        {
            _tariffValue = tariffValue;
            Created = created;
            ValidTo = validTo;
            Id = tariffId;
        }

        /// <summary>
        /// Renews tariff.
        /// </summary>
        /// <param name="validTo">Tariff expiration date.</param>
        /// <exception cref="TariffExpiredException">Throws exception if tariff expiration date is less than current dateTime.</exception>
        public void RenewTariff(DateTime validTo)
        {
            if(validTo < DateTime.Now)
                throw new TariffExpiredException(Id.ToString());

            ValidTo = validTo;
        }

        public void RevokeTariff()
        {
            Revoked = true;

            Publish(new TariffRevoked {TariffId = Id});
        }

    }
}