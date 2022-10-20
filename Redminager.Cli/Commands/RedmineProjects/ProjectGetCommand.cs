using Redminager.Cli.Services;
//using Redmine.Net.Api.Types;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Commands.RedmineProjects
{
    public class ProjectGetCommand : Command<ProjectGetCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<ID>")]
            [Description("Id used to find redmine project.")]
            public int Id { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            var project = client.GetProjectAsync(settings.Id, null).Result;

            var tbl = new Table()
                .Border(TableBorder.Horizontal)
                .AddColumn(new TableColumn("[orangered1][u]#[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Name[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]IsPublic[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Description[/][/]"));
            tbl.AddRow($"[darkseagreen2_1]{project.Id}[/]",
                            $"[darkseagreen2_1]{project.Name}[/]",
                            $"[darkseagreen2_1]{project.Is_Public}[/]",
                            $"[darkseagreen2_1]{project.Description}[/]");
            AnsiConsole.Write(tbl);
            return 0;
        }
    }
}