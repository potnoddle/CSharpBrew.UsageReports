using CSharpBrew.UsageReports.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.PlugIn;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.WebForms;
using EPiServer.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpBrew.UsageReports.Pages
{
    [GuiPlugIn(Area = PlugInArea.ReportMenu,
            DisplayName = "Content by Type Report",
            Description = "Lists content by their type",
            Category = "Usage Reports",
            SortIndex = 2000,
            RequiredAccess = AccessLevel.Administer,
            UrlFromModuleFolder = "Pages/ContentByTypeReport.aspx")]
    public partial class ContentByTypeReport : WebFormsBase
    {
        protected IEnumerable<ContentByTypeReportItem> ReportItems { get; private set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.OnLoad(e);
            if (IsPostBack)
            {
                var contentTypes = UsageReportsService.GetContentTypes().OrderBy(t => t.SortOrder);
                ContentTypes.DataSource = contentTypes.Select(t => new { Value = t.Name, Text = t.DisplayName ?? t.Name });
                ContentTypes.DataBind();

                if (!string.IsNullOrEmpty(Request.QueryString["pageId"]))
                {
                    var pageType = contentTypes.FirstOrDefault(p => p.ID.ToString() == Request.QueryString["pageId"]);
                    if (pageType != null)
                    {
                        ReportItems = UsageReportsService.GetPagesByTypeReport(pageType);
                    }
                }
            }

            var contentTypeName = Request[ContentTypes.UniqueID];
            if (!string.IsNullOrEmpty(contentTypeName))
            {
                var contentType = UsageReportsService.GetContentTypes().First(c => c.Name == contentTypeName);
                ReportItems = UsageReportsService.GetPagesByTypeReport(contentType);
            }
        }
    }
}