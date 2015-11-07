<%@ Page Language="C#" CodeBehind="PlugInReport.aspx.cs" Inherits="CSharpBrew.UsageReports.Pages.PlugInReport" %>
<asp:content contentplaceholderid="FullRegion" runat="Server">
<div class="epi-contentContainer epi-padding epi-contentArea">
    <h1 class="EP-prefix">
        A report on Plugins.
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
                    FullName
                </th>
                </tr>
            </thead>
    <% foreach (var item in ReportItems)
        { 
            %>
            <tbody>
                <tr>
                    <td><%= item.DisplayName %></td>
                    <td style="display:none;"><%= item.Description %></td>
                    <td><%= item.FullName %></td>
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
