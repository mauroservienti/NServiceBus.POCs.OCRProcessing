using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OCRWorker
{
    class Program
    {
        public static void Main()
        {
            using( ServiceHost host = new ServiceHost( typeof( WorkerService ) ) )
            {
                host.Open();

                Console.WriteLine( "WCF Host started..." );
                Console.Read();

                host.Close();
            }
        }
    }

    [ServiceContract]
    public interface IWorkerService
    {
        [OperationContract]
        void StartJob( Guid jobId, String sourceImagePath );

        [OperationContract]
        JobStatus GetJobStaus( Guid jobId );
    }

    public enum Status
    {
        Completed,
        Failed,
        InProgress,
        Unknown
    }

    public class JobStatus
    {
        public Status Status { get; set; }

        public String Error { get; set; }
    }

    public class WorkerService : IWorkerService
    {
        public void StartJob( Guid jobId, string sourceImagePath )
        {
            Console.WriteLine("Start job request: " + jobId);
        }

        public JobStatus GetJobStaus( Guid jobId )
        {
            Thread.Sleep(1500);

            var second = DateTime.Now.Second;
            if (second < 15)
            {
                Console.WriteLine( "Status Completed.");

                return new JobStatus()
                {
                    Status = Status.Completed
                };
            }
            else if( second < 30 )
            {
                Console.WriteLine( "Status Unknown." );

                return new JobStatus()
                {
                    Status = Status.Unknown
                };
            }
            else if( second < 45 )
            {
                Console.WriteLine( "Status In progress." ); 
                return new JobStatus()
                {
                    Status = Status.InProgress
                };
            }
            else
            {
                Console.WriteLine( "Status Failed." );
                return new JobStatus()
                {
                    Status = Status.Failed,
                    Error = "I'm sorry, I screwed... :-)"
                };
            }
        }
    }
}
