using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRMessages.Events
{
    public interface IOCRJobFailed
    {
        Guid RequestId { get; set; }
        Guid JobId { get; set; }

        string Error { get; set; }
    }
}
