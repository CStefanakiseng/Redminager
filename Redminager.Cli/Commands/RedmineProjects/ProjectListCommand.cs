using Redminager.Cli.Services;
using Redmine.Net.Api;
using Redminager.Cli.Models.Redmine;
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

namespace Redminager.Cli.Commands.RedmineProjects
{
    public class ProjectListCommand : Command<ProjectListCommand.Settings>
    {
        public class Settings : CommandSettings
        {

            [CommandOption("-l|--like <LIKE>")]
            [Description("filter for project name.")]
            public string Like { get; set; }
        }
        
        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            var projects = new List<Project>();
            var allProjects = client.GetProjectsAsync().Result.Projects;
            projects = (!string.IsNullOrEmpty(settings.Like)) ? allProjects.Where(p => p.Name.ToLower().Contains(settings.Like.ToLower())).ToList() : allProjects.ToList();
                        
            
            var tbl = new Table()
                .Border(TableBorder.Horizontal)
                .AddColumn(new TableColumn("[orangered1][u]#[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Name[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]IsPublic[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Description[/][/]"));

            foreach (var project in projects)
            {
                var descr = string.IsNullOrEmpty(project.Description) ? "-" : project.Description;
                tbl.AddRow($"[springgreen3]{project.Id}[/]",
                            $"[springgreen3]{project.Name}[/]",
                            $"[springgreen3]{project.Is_Public}[/]");
                            //$"[springgreen3]{descr}[/]");
            }

            AnsiConsole.Write(tbl);
            return 0;

        }
    }
}
