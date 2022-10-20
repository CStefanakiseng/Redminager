using Redminager.Cli.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;

namespace Redminager.Cli.Commands.RedmineFiles
{
    public class UploadRedmineFileCommand : Command<UploadRedmineFileCommand.Settings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            if (settings.ProjectId == 0)
            {
                AnsiConsole.WriteLine("Please enter project id");
                return 0;
            }
            if (string.IsNullOrEmpty(settings.FullFilePath))
            {
                AnsiConsole.WriteLine("Please enter full file path");
                return 0;
            }

            if (!IO.File.Exists(settings.FullFilePath))
            {
                AnsiConsole.WriteLine("Given file does not exist.");
                return 0;
            }
            RedmineClientService client = new RedmineClientService(@"https://redmine.dataverse.gr/", @"6abba14f137a25a657c0bd330cf4b8abf75e2a63");

            var res = client.UploadFileToProject(settings.ProjectId, settings.FullFilePath).Result;

            if (res.Success)
            {
                AnsiConsole.WriteLine($"Upload succeed! -File Token: {res.File.token} -FileName: {res.File.fileName}");
                return 0;
            }
            else 
            {
                AnsiConsole.WriteLine($"Error: {res.Message}");
                return 0;
            }
        }



        public class Settings : CommandSettings
        {
            [CommandOption("-p|--projectId <PROJECT_ID>")]
            [Description("Project Id of the project to upload file to.")]
            public int ProjectId { get; set; }

            [CommandOption("-f|--fullFilePath <FULL_FILE_PATH>")]
            [Description("Full file path.")]
            public string FullFilePath { get; set; }
        }

    }
}
