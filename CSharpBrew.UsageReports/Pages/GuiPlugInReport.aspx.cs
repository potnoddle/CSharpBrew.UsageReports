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
            DisplayName = "Gui PlugIns Report",
            Description = "Lists of a Gui PlugIns",
            Category = "Usage Reports",
            SortIndex = 2110,
            RequiredAccess = AccessLevel.Administer,
            UrlFromModuleFolder = "Pages/GuiPlugInReport.aspx")]
    public partial class GuiPlugInReport : WebFormsBase
    {
        public IEnumerable<GuiPlugInReportItem> ReportItems { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReportItems = UsageReportsService.GetGuiPlugInReport();
        }
    }
}