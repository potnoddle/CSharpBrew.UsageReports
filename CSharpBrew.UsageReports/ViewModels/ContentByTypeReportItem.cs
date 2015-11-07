using System;

namespace CSharpBrew.UsageReports.ViewModels
{
    public class ContentByTypeReportItem
    {
        public bool Deleted { get; internal set; }

        public string LinkUrl { get; internal set; }

        public string Name { get; internal set; }

        public DateTime StartPublish { get; internal set; }

        public DateTime StopPublish { get; internal set; }
    }
}