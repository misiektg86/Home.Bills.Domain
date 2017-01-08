using System.Reflection;
using Autofac;
using Home.Bills.Payments.Domain;
using Marten;
using Marten.Services;
using MassTransit;

namespace Home.Bills.Payments
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

                    var dcr = new ContractResolver(componentContext.Resolve<IBus>);

                    dcr.DefaultMembersSearchFlags |= BindingFlags.NonPublic | BindingFlags.Instance;

                    serializer.Customize(i =>
                    {
                        i.ContractResolver = dcr;
                    });
                    _.Serializer(serializer);
                });
        }
    }
}