using Redminager.Cli.Services;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using RT = Redmine.Net.Api.Types;
using System.Runtime.Serialization.Json;
using Redminager.Cli.Models;
using RM = Redminager.Cli.Models.Redmine;

namespace Redminager.Cli.Commands.ReleaseNotes
{
    public class ReleaseNotesBuildCommand : Command<ReleaseNotesBuildCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("-q|--queryId <QUERY_ID>")]
            [Description("The queryId for the query defined in redmine")]
            public string QueryId { get; set; }

            [CommandOption("-p|--projectId <PROJECT_ID>")]
            [Description("The projectId in which the query is defined in redmine")]
            public string ProjectId { get; set; }


            [CommandOption("-e|--extract <EXTRACT_PATH>")]
            [Description("The output path the release notes will be saved to (fill this only if file extraction is needed")]
            public string ExtractPath { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            QueryListResponse queriesTotalResp = new QueryListResponse();

            var query = client.GetQueryAsync(int.Parse(settings.QueryId)).Result;
           
            if (query != null)
            {

                //query params
                var queryParamsDict = new Dictionary<string, string>();
                queryParamsDict.Add("query_id", settings.QueryId);                

                var issuesResp = client.GetIssuesAsync(int.Parse(settings.ProjectId),queryParamsDict).Result;

                
                if (issuesResp.Issues != null)
                {
                    IEnumerable<RM.Issue> issuesFiltered = issuesResp.Issues.Where(i => i.Status.Id!= 6);


                    var tbl = new Table()
                       .Border(TableBorder.Horizontal)
                       .AddColumn(new TableColumn("[orangered1][u]#[/][/]"))
                       .AddColumn(new TableColumn("[orangered1][u]Tracker[/][/]"))
                       .AddColumn(new TableColumn("[orangered1][u]Status[/][/]"))
                       .AddColumn(new TableColumn("[orangered1][u]Release Note[/][/]"));


                    foreach (var issue in issuesFiltered)
                    {
                        var status = issue.Status == null ? string.Empty : issue.Status.Name;
                        var tracker = issue.Tracker == null ? string.Empty : issue.Tracker.Name;
                        var releaseContent = string.Empty;
                        var field = issue.Custom_Fields.FirstOrDefault(f => f.Name.Equals("Release Notes Content"));
                        if (field != null)
                        {
                            releaseContent = field.Value.ToString();
                        }

                        tbl.AddRow($"[darkseagreen2_1]{issue.Id}[/]",
                                     $"[darkseagreen2_1]{tracker}[/]",
                                     $"[darkseagreen2_1]{status}[/]",
                                     $"[darkseagreen2_1]{releaseContent}[/]");
                    }

                    AnsiConsole.Write(tbl);

                    if (!string.IsNullOrEmpty(settings.ExtractPath))
                    {
                        string fileName = $"Release_Notes_{query.Name}.txt";
                        string fileFullPath = Path.Combine(settings.ExtractPath, fileName);
                        using (StreamWriter sw = new StreamWriter(fileFullPath))
                        {
                            sw.WriteLine($"Docutracks Release Notes for {query.Name}");
                            sw.WriteLine();
                            foreach (var issue in issuesFiltered)
                            {
                                var tracker = issue.Tracker == null ? string.Empty : issue.Tracker.Name;
                                var releaseContent = string.Empty;
                                var releaseNotes = string.Empty;

                                var releaseNotesField = issue.Custom_Fields.FirstOrDefault(f => f.Name.Equals("Release Notes"));
                                if (releaseNotesField != null && releaseNotesField.Value.Equals("Yes"))
                                {
                                    var releasefield = issue.Custom_Fields.FirstOrDefault(f => f.Name.Equals("Release Notes Content"));
                                    if (releasefield != null)
                                    {
                                        releaseContent = releasefield.Value.ToString();
                                    }

                                    var scrNr = string.Format($"SN:-");
                                    var snField = issue.Custom_Fields.FirstOrDefault(f => f.Name.Equals("DB Script Nr"));
                                    if (snField != null)
                                    {
                                        scrNr = string.Format($"SN:{snField.Value}");
                                    }
                                    //if(issue.R)
                                    sw.WriteLine($"{issue.Id} - {tracker} - {scrNr} - {releaseContent}");

                                }

                            }
                        }

                    }

                }

            }  

            return 0;
        }     
    }
}