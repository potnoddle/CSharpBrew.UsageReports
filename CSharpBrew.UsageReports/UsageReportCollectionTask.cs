using CSharpBrew.UsageReports.Media;
using EPiServer;
using EPiServer.BaseLibrary.Scheduling;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Globalization;
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
            ContentReference reference;

            var documentIdentifier = $"usagereport-{DateTime.Now:dd-MM-yyy}.zip";
            var usageReport = contentRepository.GetChildren<ZippedFile>(SystemDefinition.Current.GlobalAssetsRoot).Where(f => f.Name == documentIdentifier).FirstOrDefault();
            
            if (usageReport != null)
            {
                contentRepository.Delete(usageReport.ContentLink, true,EPiServer.Security.AccessLevel.NoAccess);
            }

            var scheduledjobsUsage = UsageReportsService.GetScheduledJobs();
            var contentUsage = UsageReportsService.GetContentTypesUsage();
            var guiPlugIn = UsageReportsService.GetGuiPlugInReport();
            var pageProperties = UsageReportsService.GetPagePropertiesReport();
            var plugIn = UsageReportsService.GetPluginReport();

            using (ZipFile loanZip = new ZipFile())
            {
                loanZip.AddEntry("scheduledjobs-usage-report.json", JsonConvert.SerializeObject(scheduledjobsUsage, Formatting.Indented), Encoding.UTF8);
                loanZip.AddEntry("content-usage-report.json", JsonConvert.SerializeObject(contentUsage, Formatting.Indented), Encoding.UTF8);
                loanZip.AddEntry("guiplugin-usage-report.json", JsonConvert.SerializeObject(guiPlugIn, Formatting.Indented), Encoding.UTF8);
                loanZip.AddEntry("pageproperties-usage-report.json", JsonConvert.SerializeObject(pageProperties, Formatting.Indented), Encoding.UTF8);
                loanZip.AddEntry("plugIn-usage-report.json", JsonConvert.SerializeObject(plugIn, Formatting.Indented), Encoding.UTF8);

                var file1 = contentRepository.GetDefault<ZippedFile>(SystemDefinition.Current.GlobalAssetsRoot);
                file1.Name = documentIdentifier;
                file1.Description = "A ziped collection of usage reports";
                var blob = blobFactory.CreateBlob(file1.BinaryDataContainer, ".zip");
                using (var s = blob.OpenWrite())
                {
                    loanZip.Save(s);
                }
                file1.BinaryData = blob;
                reference = contentRepository.Save(file1, SaveAction.Publish);
            }
            return $"Report Completed <a href='{urlResolver.GetUrl(reference)}'>{documentIdentifier}</a>";
        }
    }
}