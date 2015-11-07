<%@ Page Language="C#" CodeBehind="ContentTypeUsageReport.aspx.cs" Inherits="CSharpBrew.UsageReports.Pages.ContentTypeUsageReport" %>

<asp:content contentplaceholderid="FullRegion" runat="Server">
    <div class="epi-contentContainer epi-padding epi-contentArea">
        <h1 class="EP-prefix">
            A report of all the Page types and there usage.
        </h1>  
             
        <% if (ReportItems != null && ReportItems.Any())
           { %> <table>
                   <thead>
                       <tr>
                       <th>
                           Page Type
                       </th>
                        <th style="display:none;">
                           Description
                       </th>                           
                       <th>
                           Available
                       </th>
                       <th>
                           Visible In Menu
                       </th>
                       <th>
                           Published
                       </th>
                       <th>
                           ExpiredPages
                       </th>
                        <th>
                           Total
                       </th>
                        <th>
                           Deleted
                       </th>
                       </tr>
                   </thead>
        <% foreach (var item in ReportItems)
           { %>
                   <tbody>
                   <tr>
                       <td><%= item.ContentTypeName %></td>
                       <td style="display:none;"><%= item.Description %></td>
                       <td><%= item.Available %></td>
                       <td><%= item.VisibleInMenu %></td>
                       <td><%= item.Published %></td>
                       <td><%= item.Expired %></td>
                       <td><%= item.Total %></td>
                       <td><%= item.Deleted %></td>
                   </tr>
                       </tbody>
                   <%
           }
               %>
                   </table>       
        <%
           } %>
    </div>
</asp:content>
