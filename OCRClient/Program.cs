using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using OCRMessages.Commands;

namespace OCRClient
{
    class Program
    {
        static void Main( string[] args )
        {
            var cfg = new BusConfiguration();

            cfg.EnableInstallers();
            cfg.UsePersistence<InMemoryPersistence>();

            cfg.Conventions()
                .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
                .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

            using( var bus = Bus.Create( cfg ).Start() )
            {
                Logic.Run( setup =>
                {
                    setup.DefineAction( ConsoleKey.S, "Send OCR Request", () =>
                    {
                        bus.Send(new PerformOCRJob()
                        {
                            RequestId = Guid.NewGuid(),
                            SourceImagePath = ""
                        });
                    } );
                } );
            }
        }
    }
}
