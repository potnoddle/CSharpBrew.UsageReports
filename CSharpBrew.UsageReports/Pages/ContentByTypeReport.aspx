<%@ Page Language="C#" CodeBehind="ContentByTypeReport.aspx.cs" Inherits="CSharpBrew.UsageReports.Pages.ContentByTypeReport" EnableViewState="true" %>

<asp:content contentplaceholderid="FullRegion" runat="Server">
    <div class="epi-contentContainer epi-padding epi-contentArea">
        <h1 class="EP-prefix">
            List pages by specific type
        </h1>
        <asp:DropDownList ID="ContentTypes" runat="server" DataTextField="Text" DataValueField="Value"/>

        <span class="epi-cmsButton">
            <input type="submit" value="List pages">
        </span>
        <% if (ReportItems != null && ReportItems.Any())
            { %> 
                <div class="epi-marginVertical-small">
            <table Class="epi-padding">
                       <thead>
                           <tr>
                           <th>
                               Page Name
                           </th>
                            <th>
                               Start Publish
                           </th>
                            <th>
                               Stop Publish
                           </th>
                           <th>
                               Deleted
                           </th>
                           </tr>
                       </thead>
                <% foreach (var item in ReportItems)
                    { %>    
                        <tbody>
                            <tr <%= !string.IsNullOrEmpty(item.LinkUrl)? "onclick=\"window.open('" + item.LinkUrl + "','previewPage')\" style=\"cursor: pointer;\"":string.Empty%>>
                                <td><%= item.Name%></td>
                                <td><%= item.StartPublish %></td>
                                <td><%= item.StopPublish %></td>
                                <td><%= item.Deleted %></td>
                            </tr>
                        </tbody>
                <% } %>
            </table>  
        </div>
        <% } %>
    </div>
</asp:content>
