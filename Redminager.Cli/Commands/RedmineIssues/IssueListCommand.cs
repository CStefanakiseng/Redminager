using Redminager.Cli.Services;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Commands.RedmineIssues
{
    public class IssueListCommand : Command<IssueListCommand.Settings>
    {       
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<PROJECTID>")]
            [Description("Project Id used to filter redmine issues.")]
            public int ProjectId { get; set; }

            [CommandOption("--createdOn <DATE>")]//2022-09-28|2022-09-28 
            [Description("Issue created date.")]
            public string CreatedOnDate { get; set; }

            [CommandOption("-v|--version <VERSION>")]
            [Description("Issue target version.")]
            public string TargetVersion { get; set; }

            [CommandOption("-a|--assignedTo <ASSIGNEDTO>")]
            [Description("Issue Asignee")]
            public string AssigneeName { get; set; }

            [CommandOption("--author <AUTHOR>")]
            [Description("Issue Author")]
            public string Author { get; set; }           

            [CommandOption("-s|--status <STATUS>")]
            [Description("Issue Status")]
            public string Status { get; set; }

            [CommandOption("-t|--tracker <TRACKER>")]
            [Description("Issue Tracker")]
            public string Tracker { get; set; }

            [CommandOption("-p|--priority <PRIORITY>")]
            [Description("Issue Priority")]
            public string Priority { get; set; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            var usersResp = client.GetUsersAsync(null).Result;
            

            //build criteria
            //query params
            var queryParamsDict = new Dictionary<string, string>();            

            //-> project
            if (settings.ProjectId == 0)
                throw new Exception("Invalid project id argument parameter.");
            
            //-->status
            if (!string.IsNullOrEmpty(settings.Status))
            {
                var statuses = client.GetStatuses(null).Result;
                var status = statuses.Issue_Statuses.Where(s => s.Name == settings.Status).FirstOrDefault();
                if (status != null)
                    queryParamsDict.Add("status_id", $"{status.Id}");
                else
                    throw new Exception("Invalid status option parameter.");
            }
            
            // --> createdOn
            if (settings.CreatedOnDate != null)
            {
                queryParamsDict.Add("created_on", $"%3E%3C{settings.CreatedOnDate}");
            }
                       

            //-> version
            if(!string.IsNullOrEmpty(settings.TargetVersion))
            {
                var versionResp = client.GetVersions(settings.ProjectId, null).Result;
                var version = versionResp.Versions.FirstOrDefault(v=>v.Name == settings.TargetVersion);
                if (version != null)
                {
                    queryParamsDict.Add("fixed_version_id", $"{version.Id}");
                }
                else
                    throw new Exception("Invalid version option parameter.");
            }

            //-> Assignee
            if (!string.IsNullOrEmpty(settings.AssigneeName))
            {
                //build name
                var comp = settings.AssigneeName.Split(' ');
                var lastName = comp[comp.Length - 1].Trim();
                var firstName = string.Empty;
                if (comp.Length > 1)
                {
                    for (int i = 0; i < comp.Length - 1; i++)
                    {
                        firstName += comp[i] + ' ';
                    }
                }
                var user = usersResp.Users.FirstOrDefault(u => u.Firstname == firstName.Trim() && u.Lastname == lastName.Trim());
                if (user != null)
                {
                    queryParamsDict.Add("assigned_to_id", $"{user.Id}");
                }
                else
                    throw new Exception("Invalid assignee name option parameter.");
            }

            //-> Author
            if (!string.IsNullOrEmpty(settings.Author))
            {
                //build name
                var comp = settings.Author.Split(' ');
                var lastName = comp[comp.Length - 1].Trim();
                var firstName = string.Empty;
                if (comp.Length > 1)
                {
                    for (int i = 0; i < comp.Length - 1; i++)
                    {
                        firstName += comp[i] + ' ';
                    }
                }
                var user = usersResp.Users.FirstOrDefault(u => u.Firstname == firstName.Trim() && u.Lastname == lastName.Trim());
                if (user != null)
                {
                    queryParamsDict.Add("auth_source_id", $"{user.Id}");
                }
                else
                    throw new Exception("Invalid assignee name option parameter.");
            }
            

            //-> Tracker
            if (!string.IsNullOrEmpty(settings.Tracker))
            {
                var trackersResp = client.GetTrackers(null).Result;
                var tracker = trackersResp.Trackers.FirstOrDefault(t => t.Name == settings.Tracker);

                if (tracker != null)
                    queryParamsDict.Add("tracker_id", $"{tracker.Id}");
                else
                    throw new Exception("Invalid tracker option parameter.");
            }

            //-> Priority
            if (!string.IsNullOrEmpty(settings.Priority))
            {
                var prioritiesResp = client.GetIssuePriorities(null).Result;
                var priority = prioritiesResp.Issue_Priorities.FirstOrDefault(p => p.Name == settings.Priority);

                if (priority != null)
                    queryParamsDict.Add("priority_id", $"{priority.Id}");
                else
                    throw new Exception("Invalid priority option parameter.");
            }


            //Fetch data
            var data = client.GetIssuesAsync(settings.ProjectId, queryParamsDict).Result.Issues;
            float completionRatio = CalculateDoneRatio(data);
            //Print table
            var tbl = new Table()
                .Border(TableBorder.Horizontal)
                //.Title($"[yellow]{dataQuery.Name.ToUpper()}[/]")
                .AddColumn(new TableColumn("[orangered1][u]#[/][/]").Footer("[orangered1]Total[/]"))
                .AddColumn(new TableColumn("[orangered1][u]Issue Id[/][/]").Footer($"[orangered1]{data.Count()}[/]"))
                .AddColumn(new TableColumn("[orangered1][u]Status[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Tracker[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Priority[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]AssignedTo[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Target Version[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Subject[/][/]").Footer("[orangered1]Completed[/]"))
                .AddColumn(new TableColumn("[orangered1][u]Done%[/][/]").Footer($"[orangered1]{completionRatio}%[/]"));

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
                             $"[tan]{issue.Subject}[/]",
                             $"[tan]{issue.Done_Ratio}[/]");
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
                            $"[darkseagreen2_1]{issue.Subject}[/]",
                            $"[darkseagreen2_1]{issue.Done_Ratio}[/]");
                }

            }            

            AnsiConsole.Write(tbl);
            return 0;
            
        }

        private float CalculateDoneRatio(List<Models.Redmine.Issue> data)
        {
            float result;
            int numerator = 0;
            int denominator = data.Count;

            foreach (var item in data)
            {
                if (item.Status.Is_Closed == true)
                {
                    numerator += 100;
                }
                else
                {
                    numerator += item.Done_Ratio;
                
                }
                    
                if (item.Status.Id == 6)
                    denominator -= 1;
            }
            if (denominator < 0)
                return 0;

            result =((float)numerator / (float)denominator);
            return result;
        }
    }
}
