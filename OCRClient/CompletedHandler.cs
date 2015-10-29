using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using OCRMessages.Events;
using Topics.Radical;

namespace OCRClient
{
    public class CompletedHandler : IHandleMessages<IOCRJobCompleted>
    {
        public void Handle( IOCRJobCompleted message )
        {
            using (ConsoleColor.Green.AsForegroundColor())
            {
                Console.WriteLine($"Job {message.JobId} completed!!!");
            }
        }
    }
}
