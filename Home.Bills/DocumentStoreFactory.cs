using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Domain;
using Marten;
using Marten.Services;
using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Home.Bills
{
    public class DocumentStoreFactory
    {
        public static IDocumentStore Create(IComponentContext componentContext)
        {
            return DocumentStore
                .For(_ =>
                {
                    _.MappingFor(typeof(Payments.Domain.AddressAggregate.Address)).DatabaseSchemaName = "Bills_Payments";
                    _.MappingFor(typeof(Payment)).DatabaseSchemaName = "Bills_Payments";
                    _.Schema.For<Payments.Domain.AddressAggregate.Address>().DocumentAlias("Bills_Payments_Address");
                    _.Connection("host=dev-machine;database=home_bills;password=admin;username=postgres");

                    var serializer = new JsonNetSerializer();

                    var dcr = new ContractResolver();

                    dcr.DefaultMembersSearchFlags |= BindingFlags.NonPublic | BindingFlags.Instance;

                    serializer.Customize(i =>
                    {
                        i.ContractResolver = dcr;
                    });
                    _.Serializer(serializer);
                });
        }
    }

    public class ContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType == typeof(IBus) && property.PropertyName == "MessageBus")
            {
                property.Writable = true;
                property.Readable = true;
         
                property.ShouldSerialize = value => true;
                property.Converter = new MessageBusConverter();
                property.ValueProvider = new StaticValueProvider();
            }

            return property;
        }
    }

    public class StaticValueProvider : IValueProvider
    {

        public void SetValue(object target, object value)
        {
            (target as Entity<Guid>).MessageBus = MessageBusConverter.Bus;
        }

        public object GetValue(object target)
        {
            return "";
        }
    }
}