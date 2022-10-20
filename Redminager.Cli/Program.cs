using Redminager.Cli.Commands.RedmineFiles;
using Redminager.Cli.Commands.RedmineIssues;
using Redminager.Cli.Commands.RedmineProjects;
using Redminager.Cli.Commands.RedmineQueries;
using Redminager.Cli.Commands.RedmineReports;
using Redminager.Cli.Commands.ReleaseNotes;
using Spectre.Console.Cli;
using System;

namespace Redminager.Cli
{
    class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.ValidateExamples();

                config.AddCommand<BuildReportCommand>("buildr")
                    .WithAlias("br")
                    .WithDescription("Builds report based on a specified redmine query")
                    .WithExample(new[] { "buildr", "--queryId", "265", "--projectId", "59" })
                    .WithExample(new[] { "buildr", "--queryId", "265", "-p", "59" })
                    .WithExample(new[] { "buildr", "-q", "265", "--projectId", "59" })
                    .WithExample(new[] { "buildr", "-q", "265", "-p", "59" })
                    .WithExample(new[] { "br", "--queryId", "265", "--projectId", "59" })
                    .WithExample(new[] { "br", "--queryId", "265", "-p", "59" })
                    .WithExample(new[] { "br", "-q", "265", "--projectId", "59" })
                    .WithExample(new[] { "br", "-q", "265", "-p", "59" });

                config.AddCommand<ReleaseNotesBuildCommand>("buildrn")
                    .WithDescription("Builds release notes based on a specified redmine query")
                    .WithAlias("brn")
                    .WithExample(new[] { "buildrn", "--queryId", "272", "--projectId", "59" });

                config.AddCommand<UploadRedmineFileCommand>("ufile")
                    .WithDescription("Uploads file to redmine under specific project")
                    .WithAlias("uf")
                    .WithExample(new[] { "ufile", "--fullFilePath", @"C:\Temp\test.zip", "--projectId", "59" })
                    .WithExample(new[] { "uf", "-f", @"C:\Temp\test.zip", "-p", "59" });              

                config.AddBranch("query", query =>
                {
                    query.SetDescription("view or list redmine queries");
                    
                    query.AddCommand<QueryGetCommand>("get")
                        .WithAlias("g")
                        .WithDescription("Get query information.")
                        .WithExample(new[] { "query", "get", "265" })
                        .WithExample(new[] { "query", "g", "265" });
                    
                    query.AddCommand<QueryListCommand>("list")
                        .WithAlias("l")
                        .WithDescription("Get query list.")
                        .WithExample(new[] { "query", "list" })
                        .WithExample(new[] { "query", "l"});


                });

                config.AddBranch("project", project =>
                {
                    project.SetDescription("view or list redmine queries");

                    project.AddCommand<ProjectGetCommand>("get")
                        .WithAlias("g")
                        .WithDescription("Get project information.")
                        .WithExample(new[] { "project", "get", "265" })
                        .WithExample(new[] { "project", "g", "265" });

                    project.AddCommand<ProjectListCommand>("list")
                        .WithAlias("l")
                        .WithDescription("Get project list.")
                        .WithExample(new[] { "project", "list", "--like", "Intern" })
                        .WithExample(new[] { "project", "list" ,"-l","Intern"})
                        .WithExample(new[] { "project", "l", "--like", "Intern" })
                        .WithExample(new[] { "project", "l", "-l", "Intern" });

                });

                config.AddBranch("issue", issue =>
                {
                    issue.SetDescription("view or list redmine issues");

                    issue.AddCommand<IssueGetCommand>("get")
                       .WithAlias("g")
                       .WithDescription("Get issue information.")
                       .WithExample(new[] { "issue", "get", "33468" })
                       .WithExample(new[] { "issue", "g", "33468" });

                    issue.AddCommand<IssueListCommand>("list")
                        .WithAlias("l")
                        .WithDescription("Get issue list.")
                        .WithExample(new[] { "issue", "list", "59", "--createdOn", "2022-10-05|2022-10-05", "--version", "Release 2.4.4.1", "--assignedTo", "George Gegos", "--author", "Lida Zarkada", "--status", "Assigned", "--priority", "Immediate", "--tracker", "DevTask" })
                        .WithExample(new[] { "issue", "l", "59", "--createdOn", "2022-10-05|2022-10-05", "-v", "Release 2.4.4.1", "-a", "George Gegos", "--author", "Lida Zarkada", "-s", "Assigned", "-p", "Immediate", "-t", "DevTask" });                                                           
                });
                
            });

            

            return app.Run(args);
        }
    }
}
