﻿using System;
using System.Reflection;
using Marten;
using Marten.Services;
using Newtonsoft.Json.Serialization;

namespace Home.Bills.Payments.Domain.Tests
{
    public class MartenDatabaseFixture : IDisposable
    {
        internal DocumentStore DocumentStore { get; }

        public MartenDatabaseFixture()
        {
            DocumentStore = DocumentStore
                .For(_ =>
                {
                    _.Connection("host=dev-machine;database=marten_ut_database;password=admin;username=postgres");

                    var serializer = new JsonNetSerializer();

                    var dcr = new DefaultContractResolver();

                    dcr.DefaultMembersSearchFlags |= BindingFlags.NonPublic;

                    serializer.Customize(i =>
                    {
                        i.ContractResolver = dcr;
                    });
                    _.Serializer(serializer);
                });
        }

        internal Guid Id { get; set; }

        public void Dispose()
        {
            DocumentStore.Advanced.Clean.CompletelyRemoveAll();
        }
    }
}