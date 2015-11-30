using EPiServer.DataAbstraction;
using System;

namespace CSharpBrew.UsageReports.ViewModels
{
    public class ScheduledJobsReportItem
    {
        public string scheduledjobsJobName { get; set; }

        public string FullName { get; set; }

        public int SortIndex { get; set; }

        public DateTime LastExecution { get; set; }

        public DateTime NextExecution { get; set; }

        public bool IsRunning { get; set; }

        public bool IsEnabled { get; set; }

        public ScheduledIntervalType IntervalType { get; set; }

        public int IntervalLength { get; set; }

        public Guid ID { get; set; }

        public string Description { get; set; }
    }
}
