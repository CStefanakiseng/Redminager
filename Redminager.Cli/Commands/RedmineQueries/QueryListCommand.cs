using Redminager.Cli.Models.Redmine;
using Redminager.Cli.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Commands.RedmineQueries
{
    public class QueryListCommand :Command
    {
       
        public override int Execute(CommandContext context)
        {
            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            IEnumerable<Query> queries = client.GetQueriesAsync(null).Result.Queries.ToList();

            var tbl = new Table()
                .Border(TableBorder.Horizontal)
                .AddColumn(new TableColumn("[orangered1][u]#[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]Name[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]IsPublic[/][/]"))
                .AddColumn(new TableColumn("[orangered1][u]ProjectId[/][/]"));

            foreach (var query in queries)
            {
                tbl.AddRow($"[darkseagreen2_1]{query.Id}[/]",
                            $"[darkseagreen2_1]{query.Name}[/]",
                            $"[darkseagreen2_1]{query.Is_Public}[/]",
                            $"[darkseagreen2_1]{query.Project_Id}[/]");
            }
            
            AnsiConsole.Write(tbl);
            return 0;
        }
    }
}
