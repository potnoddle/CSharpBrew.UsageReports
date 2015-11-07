using CSharpBrew.UsageReports.ViewModels;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.Security;
using EPiServer.Shell.WebForms;
using Newtonsoft.Json;
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

            if (!string.IsNullOrEmpty(Request["r"]))
            {
                switch (Request["r"])
                {
                    case "json":
                        Response.Write(JsonConvert.SerializeObject(ReportItems));
                        Response.ContentType = "application/json";
                        Response.End();
                        break;
                }
            }
        }
    }
}