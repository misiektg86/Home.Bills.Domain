using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Marten;
using Marten.Services;
using MassTransit;
using Newtonsoft.Json.Converters;

namespace Home.Bills
{
    public class DocumentStoreFactory
    {
        public static IDocumentStore Create(IComponentContext componentContext)
        {
            return DocumentStore
                .For(_ =>
                {
                    _.AutoCreateSchemaObjects = AutoCreate.CreateOnly;
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