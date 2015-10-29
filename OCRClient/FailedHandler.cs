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
    public class FailedHandler : IHandleMessages<IOCRJobFailed>
    {
        public void Handle( IOCRJobFailed message )
        {
            using (ConsoleColor.Red.AsForegroundColor())
            {
                Console.WriteLine($"Job {message.JobId} failed: {message.Error}");
            }
        }
    }
}
