using CSharpBrew.UsageReports.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data.Dynamic;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.PlugIn;
using EPiServer.Security;
using EPiServer.ServiceLocation;
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