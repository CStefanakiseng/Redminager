using Redminager.Cli.Services;
using Redmine.Net.Api.Types;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Commands.RedmineQueries
{
    public class QueryGetCommand : Command<QueryGetCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<ID>")]
            [Description("Id used to find redmine query.")]
            public int Id { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            var query = client.GetQueryAsync(settings.Id, null).Result;            

            var tbl = new Table()
                .Border(TableBorder.Horizontal)
                .AddColumn(new TableColumn("[orangered1][u]#[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Name[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]IsPublic[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]ProjectId[/][/]"));
            tbl.AddRow($"[darkseagreen2_1]{query.Id}[/]",
                            $"[darkseagreen2_1]{query.Name}[/]",
                            $"[darkseagreen2_1]{query.Is_Public}[/]",
                            $"[darkseagreen2_1]{query.Project_Id}[/]");
            AnsiConsole.Write(tbl); 
            return 0;
        }
    }
}

