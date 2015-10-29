using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Saga;
using OCRMessages.Commands;
using OCRMessages.Events;

namespace OCRManager.Sagas
{
    public class OCRJob : Saga<OCRJobData>,
        IAmStartedByMessages<PerformOCRJob>,
        IHandleTimeouts<CheckPendingJobTimeout>
    {
        protected override void ConfigureHowToFindSaga( SagaPropertyMapper<OCRJobData> mapper )
        {
            mapper.ConfigureMapping<PerformOCRJob>(m => m.RequestId).ToSaga(s => s.RequestId);
        }

        public void Handle( PerformOCRJob message )
        {
            this.Data.RequestId = message.RequestId;
            this.Data.JobId = Guid.NewGuid();
            this.Data.StartedAt = DateTime.Now;
            this.Data.SourceImagePath = message.SourceImagePath;

            try
            {
                using (OCRWorkerService.WorkerServiceClient client = new OCRWorkerService.WorkerServiceClient())
                {
                    client.StartJob(this.Data.JobId, this.Data.SourceImagePath);
                }

                this.RequestTimeout( TimeSpan.FromSeconds( 5 ), new CheckPendingJobTimeout() );
            }
            catch //handle ony WCF errors instead of all
            {
                this.Bus.Publish<IOCRJobFailed>( e =>
                {
                    e.JobId = this.Data.JobId;
                    e.RequestId = this.Data.RequestId;
                    e.Error = "Cannot connect to the OCR worker.";
                } );

                this.MarkAsComplete();
            }
        }

        public void Timeout(CheckPendingJobTimeout state)
        {
            try
            {
                using( OCRWorkerService.WorkerServiceClient client = new OCRWorkerService.WorkerServiceClient() )
                {
                    var status = client.GetJobStaus( this.Data.JobId );

                    switch (status.Status)
                    {
                        case OCRWorkerService.Status.Completed:

                            this.Bus.Publish<IOCRJobCompleted>( e =>
                            {
                                e.JobId = this.Data.JobId;
                                e.RequestId = this.Data.RequestId;
                            } );

                            this.MarkAsComplete();

                            break;

                        case OCRWorkerService.Status.InProgress:

                            this.Bus.Publish<IOCRJobInProgress>( e =>
                            {
                                e.JobId = this.Data.JobId;
                                e.RequestId = this.Data.RequestId;
                            } );

                            this.RequestTimeout( TimeSpan.FromSeconds( 5 ), new CheckPendingJobTimeout() );

                            break;

                        case OCRWorkerService.Status.Unknown:

                            this.Bus.Publish<IOCRJobFailed>( e =>
                            {
                                e.JobId = this.Data.JobId;
                                e.RequestId = this.Data.RequestId;
                                e.Error = "Cannot connect to the OCR worker.";
                            } );

                            this.MarkAsComplete();

                            break;

                        case OCRWorkerService.Status.Failed:

                            this.Bus.Publish<IOCRJobFailed>( e =>
                            {
                                e.JobId = this.Data.JobId;
                                e.RequestId = this.Data.RequestId;
                                e.Error = status.Error;
                            } );

                            this.MarkAsComplete();

                            break;
                    }
                }

                this.RequestTimeout( TimeSpan.FromSeconds( 5 ), new CheckPendingJobTimeout() );
            }
            catch //handle ony WCF errors instead of all
            {
                this.Bus.Publish<IOCRJobFailed>( e =>
                {
                    e.JobId = this.Data.JobId;
                    e.RequestId = this.Data.RequestId;
                    e.Error = "Cannot connect to the OCR worker.";
                } );

                this.MarkAsComplete();
            }
        }
    }
}
