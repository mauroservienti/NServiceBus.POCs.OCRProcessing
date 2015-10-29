using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRMessages.Events
{
    public interface IOCRJobCompleted
    {
        Guid RequestId { get; set; }
        Guid JobId { get; set; }

        string TextLocationPath { get; set; }
    }
}
