using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;

namespace OCRManager
{
    class Program
    {
        static void Main( string[] args )
        {
            var cfg = new BusConfiguration();

            cfg.EnableInstallers();

            cfg.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions()
                .SetDefaultDocumentStore( new EmbeddableDocumentStore
                {
                    ResourceManagerId = new Guid( "{3CD4D2A6-BFB0-4141-917F-58992F69384C}" ),
                    DataDirectory = @"~\RavenDB\Data"
                }.Initialize() );

            cfg.Conventions()
                .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
                .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

            using( var bus = Bus.Create( cfg ).Start() )
            {
                Console.Read();
            }
        }
    }
}
