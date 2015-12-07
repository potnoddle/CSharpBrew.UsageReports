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
            DisplayName = "Scheduled Jobs Report",
            Description = "Lists of a scheduled jobs",
            Category = "Usage Reports",
            SortIndex = 2100,
            RequiredAccess = AccessLevel.Administer,
            UrlFromModuleFolder = "Pages/ScheduledJobs.aspx")]
    public partial class ScheduledJobs : WebFormsBase
    {
        public IEnumerable<ScheduledJobsReportItem> ReportItems { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReportItems = UsageReportsService.GetScheduledJobs();
        }
    }
}