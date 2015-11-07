using EPiServer.DataAbstraction;
using System;
using System.Collections.Generic;

namespace CSharpBrew.UsageReports.ViewModels
{
    public class ContentPropertiesReportItem
    {

        public Guid GUID { get; internal set; }

        public int ID { get; internal set; }

        public string ContentTypeName { get; set; }

        public bool ReadOnly { get; internal set; }

        public string Description { get; set; }

        public string GroupName { get; set; }

        public bool Available { get; set; }

        public int SortOrder { get; set; }

        public string ImageUrl;

        public IEnumerable<string> ACL { get; internal set; }

        public string ModelType { get; internal set; }

        public ContentTypePropertyReportItem[] Properties { get; set; } 

        public string[] ExcludeContentTypeNames { get; set; }

        public string[] ExcludeOnContentTypeNames { get; internal set; }

        public string[] IncludedContentTypeNames { get; internal set; }

        public string[] IncludedOnContentTypeNames { get; internal set; }

        public string Access { get; internal set; }

        public string Users { get; internal set; }

        public string VisitorGroups { get; internal set; }

        public string Roles { get; internal set; }

        public string Availability { get; internal set; }

        public string[] SupportedTemplates { get; internal set; }
    }
}