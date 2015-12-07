using CSharpBrew.UsageReports.ViewModels;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.Security;
using EPiServer.Shell.WebForms;
using System;
using System.Collections.Generic;

namespace CSharpBrew.UsageReports.Pages
{
    [GuiPlugIn(Area = PlugInArea.ReportMenu,
            DisplayName = "Content Type Usage Report",
            Description = "Lists Properties by content types",
            Category = "Usage Reports",
            SortIndex = 2020,
            RequiredAccess = AccessLevel.Administer,
            UrlFromModuleFolder = "Pages/ContentTypeUsageReport.aspx")]
    public partial class ContentTypeUsageReport : WebFormsBase
    {
        public IEnumerable<ContentTypeUsageReportItem> ReportItems { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReportItems = UsageReportsService.GetContentTypesUsage();
        }
    }
}