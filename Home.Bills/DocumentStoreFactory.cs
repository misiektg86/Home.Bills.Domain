using System.Reflection;
using Autofac;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.MeterReadAggregate;
using Marten;
using Marten.Services;
using MassTransit;

namespace Home.Bills
{
    public class DocumentStoreFactory
    {
        public static IDocumentStore Create(IComponentContext componentContext)
        {
            return DocumentStore
                .For(_ =>
                {
                    _.DatabaseSchemaName = "home_bills";
                    _.Schema.For<MeterRead>().UseOptimisticConcurrency(true);
                    _.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                    _.Connection("host=dev-machine;database=home_test;password=admin;username=postgres");

                    var serializer = new JsonNetSerializer();

                    var dcr = new ContractResolver(componentContext.Resolve<IBus>, componentContext.Resolve<IPublishRecorder>);

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