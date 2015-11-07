namespace CSharpBrew.UsageReports.ViewModels
{
    public class ContentTypeUsageReportItem
    {
        public string ContentTypeName { get; set; }

        public string Description { get; set; }

        public int? Published { get; set; }

        public int? Expired { get; set; }

        public int? DelayedPublish { get; set; }

        public int? ReadyToPublish { get; set; }

        public int SortOrder { get; set; }

        public int Total { get; set; }

        public int Deleted { get; set; }

        public bool? VisibleInMenu { get; set; }

        public bool Available { get; set; }

        public bool IsPage { get; internal set; }

        public bool IsBlock { get; internal set; }

        public bool IsMedia { get; internal set; }
    }
}