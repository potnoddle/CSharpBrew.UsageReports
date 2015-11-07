using EPiServer.PlugIn;
using EPiServer.Security;

namespace CSharpBrew.UsageReports.ViewModels
{
    public class GuiPlugInReportItem : PlugInReportItem 
    {
        public string PlugInArea { get; set; }

        public string Category { get; set; }

        public string AccessLevel { get; set; }

        public string Url { get; set; }
    }
}
