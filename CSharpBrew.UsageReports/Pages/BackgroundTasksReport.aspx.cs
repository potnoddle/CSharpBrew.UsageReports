using CSharpBrew.UsageReports.ViewModels;
using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBrew.UsageReports.Pages
{
    [GuiPlugIn(Area = PlugInArea.ReportMenu,
            DisplayName = "Background Tasks Report",
            Description = "Lists of a Background Tasks",
            Category = "Usage Reports",
            SortIndex = 2100,
            RequiredAccess = AccessLevel.Administer,
            UrlFromModuleFolder = "Pages/BackgroundTasksReport.aspx")]
    public partial class BackgroundTasksReport : WebFormsBase
    {
        public IEnumerable<BackgroundTasksReportItem> ReportItems { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReportItems = UsageReportsService.GetBackgroundTasksReport();

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