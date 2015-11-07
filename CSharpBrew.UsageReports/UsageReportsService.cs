using CSharpBrew.UsageReports.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Filters;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBrew.UsageReports
{
    internal static class UsageReportsService
    {
        internal static IEnumerable<PlugInReportItem> GetPluginReport()
        {
            return from ass in AppDomain.CurrentDomain.GetAssemblies()
                   from type in ass.GetTypes()
                   where !ass.FullName.StartsWith("EPiServer")
                   from attrib in type.GetCustomAttributes(true).OfType<PlugInAttribute>()
                   where !(attrib is GuiPlugInAttribute) && !type.FullName.Contains("CSharpBrew.UsageReports")
                   orderby attrib.SortIndex
                   select new PlugInReportItem
                   {
                       DisplayName = attrib.DisplayName ?? type.Name,
                       Description = attrib.Description,
                       FullName = type.FullName,
                       SortIndex = attrib.SortIndex
                   };
        }

        internal static IEnumerable<BackgroundTasksReportItem> GetBackgroundTasksReport()
        {
            var scheduledJobRepo = ServiceLocator.Current.GetInstance<ScheduledJobRepository>();
            return from job in scheduledJobRepo.List()
                   let plugInDescriptor = PlugInDescriptor.Load(job.TypeName, job.AssemblyName)
                   where plugInDescriptor != null
                   let attrib = GetAttribute(plugInDescriptor)
                   where attrib != null
                   select new BackgroundTasksReportItem
                   {
                       BackGroundTaskName = attrib.DisplayName,
                       FullName = plugInDescriptor.GetType().FullName,
                       Description = attrib.Description,
                       SortIndex = attrib.SortIndex,
                       LastExecution = job.LastExecution,
                       NextExecution = job.NextExecution,
                       IsRunning = job.IsRunning,
                       IsEnabled = job.IsEnabled,
                       IntervalType = job.IntervalType,
                       IntervalLength = job.IntervalLength,
                       ID = job.ID
                   };
        }

        internal static IEnumerable<ContentPropertiesReportItem> GetPagePropertiesReport()
        {
            var contentTypeRepo = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var templateModelRepo = ServiceLocator.Current.GetInstance<TemplateModelRepository>();
            //propertyTypeRepo.Load()
            return from t in contentTypeRepo.List().OrderBy(t => t.SortOrder)
                          let ct = t.ModelType
                          where ct != null
                          let avt = ct.GetType().GetCustomAttributes(true).OfType<AvailableContentTypesAttribute>().FirstOrDefault()
                          let aa = ct.GetType().GetCustomAttributes(true).OfType<AccessAttribute>().FirstOrDefault()
                          let iua = ct.GetType().GetCustomAttributes(true).OfType<ImageUrlAttribute>().FirstOrDefault()
                          select new ContentPropertiesReportItem
                          {
                              ID = t.ID,
                              GUID = t.GUID,
                              ContentTypeName = t.Name,
                              ModelType = t.ModelType.Name,
                              GroupName = t.GroupName,
                              Available = t.IsAvailable,
                              Description = t.Description,
                              SortOrder = t.SortOrder,
                              ReadOnly = t.IsReadOnly,
                              ImageUrl = iua?.Path,
                              SupportedTemplates = templateModelRepo.List(t.ModelType).Select(s => s.DisplayName ?? s.Name).ToArray(),
                              ACL = t.ACL.Select(acl => $"{acl.Key} has {acl.Value.Access.ToString()}"),
                              Access = aa?.Access.ToString(),
                              Users = aa?.Users,
                              Roles = aa?.Roles,
                              VisitorGroups = aa?.VisitorGroups,
                              Availability = avt?.Availability.ToString(),
                              ExcludeContentTypeNames = avt?.Exclude.OrderBy(o => o.Name).Select(o => o.Name).ToArray(),
                              ExcludeOnContentTypeNames = avt?.ExcludeOn.OrderBy(o => o.Name).Select(o => o.Name).ToArray(),
                              IncludedContentTypeNames = avt?.Include.OrderBy(o => o.Name).Select(o => o.Name).ToArray(),
                              IncludedOnContentTypeNames = avt?.IncludeOn.OrderBy(o => o.Name).Select(o => o.Name).ToArray(),
                              Properties = (from p in t.PropertyDefinitions
                                            select new ContentTypePropertyReportItem
                                            {
                                                PropertyName = p.Name,
                                                HelpText = p.HelpText,
                                                DefaultValue = p.DefaultValue,
                                                DefaultValueType = p.DefaultValueType.ToString(),
                                                DisplayEditUI = p.DisplayEditUI,
                                                Required = p.Required,
                                                Searchable = p.Searchable,
                                                FieldOrder = p.FieldOrder,
                                                TabName = p.Tab?.Name,
                                                TypeName = p.Type.LocalizedName,
                                                ID = p.ID,
                                                DynamicProperty = p.IsDynamicProperty,
                                                LanguageSpecific = p.LanguageSpecific
                                            }).ToArray()
                          };
        }

        internal static IEnumerable<ContentTypeUsageReportItem> GetContentTypesUsage()
        {
            var contentTypeRepo = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
            var contentModelUsage = ServiceLocator.Current.GetInstance<IContentModelUsage>();

            return from contentType in contentTypeRepo.List().OrderBy(t => t.SortOrder)
                          let usages = contentRepo.GetItems(
                                (from u in contentModelUsage.ListContentOfContentType(contentType)
                                 select u.ContentLink.ToReferenceWithoutVersion()).Distinct()
                                , LanguageSelector.AutoDetect())
                          let pages = usages.OfType<PageData>()
                          let IsPage = (contentType is PageType)
                          let IsBlock = (contentType is BlockType)
                          let IsMedia = (!IsPage && IsBlock)
                          orderby !IsBlock
                          orderby !IsPage
                          select new ContentTypeUsageReportItem
                          {
                              ContentTypeName = contentType.Name,
                              Available = contentType.IsAvailable,
                              VisibleInMenu = (contentType as PageType)?.Defaults.VisibleInMenu,
                              Description = contentType.Description,
                              SortOrder = contentType.SortOrder,
                              Published = IsPage ? pages.Count(p => p.Status == VersionStatus.Published && p.StopPublish > DateTime.Now && !p.IsDeleted) : (int?)null,
                              Expired = IsPage ? pages.Count(p => p.Status == VersionStatus.Published && p.StopPublish < DateTime.Now && !p.IsDeleted) : (int?)null,
                              DelayedPublish = IsPage ? pages.Count(p => p.Status == VersionStatus.DelayedPublish && !p.IsDeleted) : (int?)null,
                              ReadyToPublish = IsPage ? pages.Count(p => p.Status == VersionStatus.CheckedIn && !p.IsDeleted) : (int?)null,
                              Total = usages.Count(),
                              Deleted = usages.Count(p => p.IsDeleted),
                              IsPage = IsPage,
                              IsBlock = IsBlock,
                              IsMedia = IsMedia
                          };
        }

        internal static IEnumerable<GuiPlugInReportItem> GetGuiPlugInReport()
        {
            return from ass in AppDomain.CurrentDomain.GetAssemblies()
                   from type in ass.GetTypes()
                   where !ass.FullName.StartsWith("EPiServer")
                   from attrib in type.GetCustomAttributes(true).OfType<GuiPlugInAttribute>()
                   orderby attrib.Area
                   select new GuiPlugInReportItem
                   {
                       DisplayName = attrib.DisplayName ?? type.Name,
                       Description = attrib.Description,
                       PlugInArea = attrib.Area.ToString(),
                       Category = attrib.Category,
                       Url = SafeGet(() => attrib.Url),
                       AccessLevel = attrib.RequiredAccess.ToString(),
                       FullName = type.FullName
                   };
        }

        internal static IEnumerable<ContentByTypeReportItem> GetPagesByTypeReport(ContentType contentType)
        {
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var virtualPathArguments = new VirtualPathArguments { ContextMode = ContextMode.Preview };
            return from p in GetContentByType(contentType).OfType<IContent>()
                   select new ContentByTypeReportItem
                   {
                       Name = p.Name,
                       LinkUrl = GetEditUrl(p.ContentLink,"en"),
                       //StartPublish = p.StartPublish,
                       //StopPublish = p.StopPublish,
                       Deleted = p.IsDeleted
                   };
        }

        private static string GetEditUrl(ContentReference contentReference, string language = "en")
        {
            return $"/EPiServer/Cms/#viewsetting=viewlanguage:///{language}&context=epi.cms.contentdata:///{contentReference.ID}";
        }

        private static IEnumerable<PageData> GetPagesByTypeName(string pageTypeName)
        {
            var pageCriteriaQueryService = ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>();

            return pageCriteriaQueryService.FindPagesWithCriteria(
                PageReference.RootPage,
                new PropertyCriteriaCollection
                {
                    new PropertyCriteria
                        {
                            Name = "PageTypeName",
                            Value = pageTypeName,
                            Condition = CompareCondition.Equal,
                            Type = PropertyDataType.String
                        }
                });
        }

        private static IEnumerable<IContentData> GetContentByType(ContentType contentType)
        {
            var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
            var contentModelUsage = ServiceLocator.Current.GetInstance<IContentModelUsage>();
            return contentRepo.GetItems(
                                   (from u in contentModelUsage.ListContentOfContentType(contentType)
                                    select u.ContentLink.ToReferenceWithoutVersion()).Distinct()
                                   , LanguageSelector.AutoDetect());
        }

        private static T SafeGet<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default(T);
            }
        }

        private static ScheduledPlugInAttribute GetAttribute(PlugInDescriptor plugInDescriptor)
        {
            try
            {
                return plugInDescriptor.GetAttribute(typeof(ScheduledPlugInAttribute)) as ScheduledPlugInAttribute;
            }
            catch
            {
                return null;
            }
        }
    }
}
