﻿using System.Reflection;
using Autofac;
using Marten;
using Marten.Services;
using Newtonsoft.Json.Serialization;
using Home.Bills.Domain.MeterReadAggregate;

namespace Home.Bills.Client
{
    public class DocumentStoreFactory
    {
        public static IDocumentStore Create(IComponentContext componentContext)
        {
            return DocumentStore
                .For(_ =>
                {
                    _.DatabaseSchemaName = "home_bills";

                    _.Connection("host=dev-machine;database=home_bills;password=admin;username=postgres");

                    var serializer = new JsonNetSerializer();

                    var dcr = new DefaultContractResolver();

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