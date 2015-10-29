using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Saga;

namespace OCRManager.Sagas
{
    public class OCRJobData : ContainSagaData
    {
        [Unique]
        public Guid RequestId { get; set; }

        public Guid JobId { get; set; }
        public DateTime StartedAt { get; set; }
        public string SourceImagePath { get; set; }
    }
}
