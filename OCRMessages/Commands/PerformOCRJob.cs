using System;

namespace OCRMessages.Commands
{
    public class PerformOCRJob
    {
        public Guid RequestId { get; set; }
        public string SourceImagePath { get; set; }
    }
}