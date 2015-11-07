<%@ Page Language="C#" CodeBehind="GuiPlugInReport.aspx.cs" Inherits="CSharpBrew.UsageReports.Pages.GuiPlugInReport" %>

<asp:content contentplaceholderid="FullRegion" runat="Server">
    <div class="epi-contentContainer epi-padding epi-contentArea">
        <h1 class="EP-prefix">
            A report on Gui Plugins.
        </h1>  
             
        <% if (ReportItems != null && ReportItems.Any())
            { %> <table>
                   <thead>
                       <tr>
                           <th>
                               Name
                           </th>
                            <th style="display:none;">
                               Description
                           </th>
                           <th>
                               Area
                           </th>
                           <th>
                               Required Access
                           </th>          
                           <th>
                               FullName
                           </th>
                       </tr>
                   </thead>
        <% foreach (var item in ReportItems)
            { %>
                   <tbody>
                       <tr>
                           <td><%= item.DisplayName %></td>
                           <td style="display:none;"><%= item.Description %></td>                           
                           <td><%= item.PlugInArea %></td>
                            <td><%= item.AccessLevel %></td>
                           <td><%= item.FullName %></td>
                       </tr>
                </tbody>
                   <% }  %>
          </table>    
        <% } %>
    </div>
</asp:content>
