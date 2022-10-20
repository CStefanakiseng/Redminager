using Redminager.Cli.Models.Redmine;
using Redminager.Cli.Services;


using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Commands.RedmineReports
{
    public class BuildReportCommand : Command<BuildReportCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("-q|--queryId <QUERY_ID>")]
            [Description("The queryId for the query defined in redmine")]
            [DefaultValue("265")]
            public string QueryId { get; set; }

            [CommandOption("-p|--projectId <PROJECT_ID>")]
            [Description("The projectId in which the query is defined in redmine")]
            [DefaultValue("59")]
            public string ProjectId { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            AnsiConsole.MarkupLine($"Getting [bold green]results on query {settings.QueryId} for project {settings.ProjectId} [/]!");

            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            //RedmineDataService service = new RedmineDataService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");
            
            var query = client.GetQueryAsync(int.Parse(settings.QueryId), null).Result;

            if (query != null)
            {
                Dictionary<string, string> queryParamsDict = new Dictionary<string, string>();
                queryParamsDict.Add("query_id", settings.QueryId);
                IEnumerable<Issue> data = client.GetIssuesAsync(int.Parse(settings.ProjectId), queryParamsDict).Result.Issues;
                AnsiConsole.Write(CreateTable(data, settings, query));
            }            
            return 0;
        }

        private static Table CreateTable(IEnumerable<Issue> data, Settings settings, Query dataQuery)
        {                        
            var tbl = new Table()
                .Border(TableBorder.Horizontal)
                .Title($"[yellow]{dataQuery.Name.ToUpper()}[/]")
                .AddColumn(new TableColumn("[orangered1][u]#[/][/]").Footer("[orangered1]Total[/]"))
                .AddColumn(new TableColumn("[orangered1][u]Issue Id[/][/]").Footer($"[orangered1]{data.Count()}[/]"))
                .AddColumn(new TableColumn("[orangered1][u]Status[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Tracker[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Priority[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]AssignedTo[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Target Version[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Subject[/][/]"));

            int count = 0;
            foreach (var issue in data)
            {
                count++;
                var status = issue.Status == null ? string.Empty : issue.Status.Name;
                var tracker = issue.Tracker == null ? string.Empty : issue.Tracker.Name;
                var priority = issue.Priority == null ? string.Empty : issue.Priority.Name;
                var assignedTo = issue.Assigned_To == null ? string.Empty : issue.Assigned_To.Name;
                var targetVersion = issue.Fixed_Version == null ? string.Empty : issue.Fixed_Version.Name;
                if (count % 2 == 0)
                {
                    tbl.AddRow($"[tan]{count}[/]",
                             $"[tan]{issue.Id}[/]",
                             $"[tan]{status}[/]",
                             $"[tan]{tracker}[/]",
                             $"[tan]{priority}[/]",
                             $"[tan]{assignedTo}[/]",
                             $"[tan]{targetVersion}[/]",
                             $"[tan]{issue.Subject}[/]");
                }
                else
                {
                    tbl.AddRow($"[darkseagreen2_1]{count}[/]",
                            $"[darkseagreen2_1]{issue.Id}[/]",
                            $"[darkseagreen2_1]{status}[/]",
                            $"[darkseagreen2_1]{tracker}[/]",
                            $"[darkseagreen2_1]{priority}[/]",
                            $"[darkseagreen2_1]{assignedTo}[/]",
                            $"[darkseagreen2_1]{targetVersion}[/]",
                            $"[darkseagreen2_1]{issue.Subject}[/]");
                }                
            }
            return tbl;  
        }
    }
}