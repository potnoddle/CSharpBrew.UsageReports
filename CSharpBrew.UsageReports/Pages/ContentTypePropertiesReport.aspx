<%@ Page Language="C#" CodeBehind="ContentTypePropertiesReport.aspx.cs" Inherits="CSharpBrew.UsageReports.Pages.ContentTypePropertiesReport" %>
<%@ Import Namespace="System.Linq" %>
<asp:content contentplaceholderid="FullRegion" runat="Server">
    <div class="epi-contentContainer epi-padding epi-contentArea">
        <h1 class="EP-prefix">
            A report on all Page and their properties.
        </h1>             
        <% if (ReportItems != null && ReportItems.Any())
            { %> <table>
                   <thead>
                       <tr>
                       <th>
                           Page Type
                       </th>
                       <th>
                           Group Name
                       </th>
                        <th style="display:none;">
                           Description
                       </th>
                       <th>
                           Available
                       </th>
                       </tr>
                    </thead>
        <% foreach (var item in ReportItems)
            { %>
                   <tbody>
                       <tr>
                       <td><%= item.ContentTypeName %></td>
                       <td><%= item.GroupName %></td>
                       <td style="display:none;"><%= item.Description %></td>
                       <td><%= item.Available %></td>  
                           </tr>
                        <tr>
                       <td colspan="10">
        <% if (ReportItems != null && ReportItems.Any())
            { %> <table>
                   <thead>
                       <tr>
                       <th>
                           Name
                       </th>
                        <th style="display:none;">
                           EditCaption
                       </th>
                       <th>
                           Group Name
                       </th>
                       </tr>
                       </thead>
                        <% foreach (var prop in item.Properties)
                            { %>
                                   <tbody>
                                       <tr>
                                       <td><%= prop.PropertyName%></td>
                                       <td style="display:none;"><%= prop.DisplayEditUI %></td>
                                       <td><%= prop.TabName %></td>
                                       </tr>
                                   </tbody>
                        <% } %>
                            </table>
                            <% } %>
                        </td>  
                    </tr>
                </tbody>
                <% }%>
            </table>
        <% } %>
    </div>
</asp:content>
