using EPiServer.DataAbstraction;

namespace CSharpBrew.UsageReports.ViewModels
{
    public class ContentTypePropertyReportItem
    {
        public int ID { get; internal set; }

        public string PropertyName { get; set; }

        public string HelpText { get; internal set; }

        public string TypeName { get; internal set; }

        public virtual object DefaultValue { get; set; }

        public virtual string DefaultValueType { get; set; }

        public virtual bool DisplayEditUI { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool Searchable { get; set; }

        public virtual int FieldOrder { get; set; }

        public virtual string TabName { get; set; }

        public bool DynamicProperty { get; internal set; }

        public bool LanguageSpecific { get; internal set; }
    }
}