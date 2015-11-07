<%@ Page Language="C#" CodeBehind="BackgroundTasksReport.aspx.cs" Inherits="CSharpBrew.UsageReports.Pages.BackgroundTasksReport" %>

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
                           Background Task
                       </th>
                        <th style="display:none;">
                           Description
                       </th>                           
                       <th>
                           Enabled
                       </th>
                       <th>
                           Running
                       </th>
                       <th>
                           Last Execution
                       </th>
                       <th>
                           Next Execution
                       </th>                      
                        <th>
                           Interval
                       </th>
                       </tr>
                   </thead>
        <% foreach (var item in ReportItems)
           { %>
                   <tbody>
                   <tr>
                       <td><%= item.BackGroundTaskName %></td>
                       <td style="display:none;"><%= item.Description %></td>
                       <td><%= item.IsEnabled %></td>
                       <td><%= item.IsRunning %></td>
                       <td><%= item.LastExecution %></td>
                       <td><%= item.NextExecution %></td>
                       <td><%= item.IntervalLength.ToString() %>
                           <%= item.IntervalType.ToString() %></td>
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
