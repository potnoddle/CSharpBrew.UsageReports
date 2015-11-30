using CSharpBrew.UsageReports.ViewModels;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.Security;
using EPiServer.Shell.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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