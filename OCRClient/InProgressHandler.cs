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
    public class InProgressHandler : IHandleMessages<IOCRJobInProgress>
    {
        public void Handle( IOCRJobInProgress message )
        {
            using (ConsoleColor.Yellow.AsForegroundColor())
            {
                Console.WriteLine($"Job {message.JobId} in progress");
            }
        }
    }
}
