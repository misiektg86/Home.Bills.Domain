using System.Reflection;
using Autofac;
using Frameworks.Light.Ddd;
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
                    _.AutoCreateSchemaObjects = AutoCreate.CreateOnly;

                    _.DatabaseSchemaName = "home_bills_payments";

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