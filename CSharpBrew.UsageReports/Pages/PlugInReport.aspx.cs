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
            DisplayName = "PlugIns Report",
            Description = "Lists of a PlugIns",
            Category = "Usage Reports",
            SortIndex = 2120,
            RequiredAccess = AccessLevel.Administer,
            UrlFromModuleFolder = "Pages/PlugInReport.aspx")]
    public partial class PlugInReport : WebFormsBase
    {
        public IEnumerable<PlugInReportItem> ReportItems { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReportItems = UsageReportsService.GetPluginReport();
        }
    }
}