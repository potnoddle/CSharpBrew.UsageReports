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
            DisplayName = "Content Type Properties Report",
            Description = "Lists Content Types and there Properties ",
            Category = "Usage Reports",
            SortIndex = 2010,
            RequiredAccess = AccessLevel.Administer,
            UrlFromModuleFolder = "Pages/ContentTypePropertiesReport.aspx")]
    public partial class ContentTypePropertiesReport : WebFormsBase
    {
        public IEnumerable<ContentPropertiesReportItem> ReportItems { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReportItems =  UsageReportsService.GetPagePropertiesReport();
        }
    }
}