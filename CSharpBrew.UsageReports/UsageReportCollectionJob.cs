using CSharpBrew.UsageReports.Media;
using EPiServer;
using EPiServer.BaseLibrary.Scheduling;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace CSharpBrew.UsageReports
{
    /// <summary>
    /// Initialization Job that must be executed on first time run the site to index content
    /// </summary>
    [ScheduledPlugIn(DisplayName = "Usage Report Collection Job", 
        Description = "A exports of usage reports in Json from for processing", 
        SortIndex =10001)]
    public class UsageReportCollectionJob : JobBase
    {
        /// </summary>
        public override string Execute()
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var blobFactory = ServiceLocator.Current.GetInstance<BlobFactory>();
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var assetRoot = SystemDefinition.Current.GlobalAssetsRoot;

            var reportName = $"usagereport-{DateTime.Now:dd-MM-yyy}.zip";
            var usageReport = contentRepository.GetChildren<ZippedFile>(assetRoot).Where(f => f.Name == reportName).FirstOrDefault();
            if (usageReport != null)
            {
                contentRepository.Delete(usageReport.ContentLink, true,EPiServer.Security.AccessLevel.NoAccess);
            }
            using (ZipFile loanZip = new ZipFile())
            {
                loanZip.AddEntry("scheduledjobs-report.json", 
                    JsonConvert.SerializeObject(UsageReportsService.GetScheduledJobs(),
                    Formatting.Indented),
                    Encoding.UTF8);

                loanZip.AddEntry("content-report.json", 
                    JsonConvert.SerializeObject(UsageReportsService.GetContentTypesUsage(), 
                    Formatting.Indented), 
                    Encoding.UTF8);

                loanZip.AddEntry("guiplugin-report.json", 
                    JsonConvert.SerializeObject(UsageReportsService.GetGuiPlugInReport(),
                    Formatting.Indented), 
                    Encoding.UTF8);

                loanZip.AddEntry("contentproperties-report.json",
                    JsonConvert.SerializeObject(UsageReportsService.GetContentPropertiesReport(), 
                    Formatting.Indented),
                    Encoding.UTF8);

                loanZip.AddEntry("plugIn-report.json", 
                    JsonConvert.SerializeObject(UsageReportsService.GetPluginReport(), 
                    Formatting.Indented), 
                    Encoding.UTF8);

                var newReport = contentRepository.GetDefault<ZippedFile>(assetRoot);
                newReport.Name = reportName;
                newReport.Description = "A zipped collection of usage reports";

                var blob = blobFactory.CreateBlob(newReport.BinaryDataContainer, ".zip");
                using (var stream = blob.OpenWrite())
                {
                    loanZip.Save(stream);
                }

                newReport.BinaryData = blob;
                var reference = contentRepository.Save(newReport, SaveAction.Publish);
                return $"Report Completed <a href='{urlResolver.GetUrl(reference)}'>{reportName}</a>";
            }
        }
    }
}