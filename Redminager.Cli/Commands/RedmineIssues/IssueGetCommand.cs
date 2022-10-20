using Redminager.Cli.Models.Redmine;
using Redminager.Cli.Services;
//using Redmine.Net.Api.Types;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Commands.RedmineIssues
{
    public class IssueGetCommand : Command<IssueGetCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<ID>")]
            [Description("Id used to find redmine issue.")]
            public int Id { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            Issue issue = client.GetIssueAsync(settings.Id, null).Result;
            if (issue != null)
            {

                var status = issue.Status != null ? issue.Status.Name : string.Empty;
                var tracker = issue.Tracker != null ? issue.Tracker.Name : string.Empty;
                var priority = issue.Priority != null ? issue.Priority.Name : string.Empty;
                var category = issue != null ? issue.Category.Name : string.Empty;
                var targetVersion = issue.Fixed_Version != null ? issue.Fixed_Version.Name : string.Empty;
                var assignee = issue.Assigned_To != null ? issue.Assigned_To.Name : string.Empty;
                var author = issue.Author != null ? issue.Author.Name : string.Empty;
                var project = issue.Project != null ? issue.Project.Name : string.Empty;
                var startdate = issue.Start_Date != null ? issue.Start_Date.ToString() : string.Empty;
                var dueDate = issue.Due_Date != null ? issue.Due_Date.ToString() : string.Empty;
                var createdOn = issue.Created_On != null ? issue.Created_On.ToString() : string.Empty;
                var updatedOn = issue.Updated_On != null ? issue.Updated_On.ToString() : string.Empty;
                var closedOn = issue.Closed_On != null ? issue.Closed_On.ToString() : string.Empty;

                var tbl = new Table()
                    .Title($"{tracker} #{issue.Id}")
                    .Border(TableBorder.Horizontal)
                    .AddColumn(new TableColumn("[orangered1][u]Property[/][/]"))
                    .AddColumn(new TableColumn("[orangered1][u]Value[/][/]"));



                tbl.AddRow($"[yellow4_1]Subject[/]", $"[darkseagreen2_1]{issue.Subject}[/]");
                tbl.AddRow($"[yellow4_1]Status[/]", $"[darkseagreen2_1]{status}[/]");
                tbl.AddRow($"[yellow4_1]Priority[/]", $"[darkseagreen2_1]{priority}[/]");                
                tbl.AddRow($"[yellow4_1]Category[/]", $"[darkseagreen2_1]{category}[/]");

                tbl.AddRow($"[yellow4_1]Assigned To[/]", $"[darkseagreen2_1]{assignee}[/]");
                tbl.AddRow($"[yellow4_1]Author[/]", $"[darkseagreen2_1]{author}[/]");
                tbl.AddRow($"[yellow4_1]Target Version[/]", $"[darkseagreen2_1]{targetVersion}[/]");
               
                tbl.AddRow($"[yellow4_1]Done Ratio[/]", $"[darkseagreen2_1]{issue.Done_Ratio}%[/]");                
                tbl.AddRow($"[yellow4_1]Estimated Hours[/]", $"[darkseagreen2_1]{issue.Estimated_Hours}[/]");
                tbl.AddRow($"[yellow4_1]Spent Hours[/]", $"[darkseagreen2_1]{issue.Spent_Hours}[/]");
                
                tbl.AddRow($"[yellow4_1]Created On[/]", $"[darkseagreen2_1]{createdOn}[/]");
                tbl.AddRow($"[yellow4_1]Updated On[/]", $"[darkseagreen2_1]{updatedOn}[/]");
                tbl.AddRow($"[yellow4_1]Due Date[/]", $"[darkseagreen2_1]{dueDate}[/]");
                tbl.AddRow($"[yellow4_1]Start Date[/]", $"[darkseagreen2_1]{startdate}[/]");
                tbl.AddRow($"[yellow4_1]Closed On[/]", $"[darkseagreen2_1]{closedOn}[/]");
                
                
                tbl.AddRow($"[yellow4_1]Project[/]", $"[darkseagreen2_1]{project}[/]");
                tbl.AddRow($"[yellow4_1]Description[/]", $"[darkseagreen2_1]{issue.Description}[/]");
                AnsiConsole.Write(tbl);

            }
            else
            {
                AnsiConsole.WriteLine($"Issue with Id:{settings.Id} could not be found.");
            }
            
            return 0;
        }
    }
}

