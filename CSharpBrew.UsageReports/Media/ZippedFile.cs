using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace CSharpBrew.UsageReports.Media
{
    [ContentType(GUID = "{1BDDB51F-83AA-4D4B-8664-E40AE58DF2C6}")]
    [MediaDescriptor(ExtensionString = ".zip")]
    public class ZippedFile : MediaData
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public virtual string Description { get; set; }
    }
}
