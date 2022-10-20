using Moq;
using Redminager.Cli.Commands.RedmineFiles;
using Redminager.Cli.Commands.RedmineIssues;
using Redminager.Cli.Commands.RedmineProjects;
using Redminager.Cli.Commands.RedmineReports;
using Redminager.Cli.Commands.ReleaseNotes;
using Spectre.Console.Cli;
using System;
using Xunit;

namespace Redminager.Cli.Tests
{
    public class UnitTest1
    {
        private readonly IRemainingArguments _remainingArgs = new Mock<IRemainingArguments>().Object;
        [Fact]
        public void BuildReportCommandTest()
        {
            BuildReportCommand cm = new BuildReportCommand();
            var context = new CommandContext(_remainingArgs, "list", null);
            BuildReportCommand.Settings settings = new BuildReportCommand.Settings() { ProjectId = "59", QueryId = "265" };
            var result = cm.Execute(context, settings);
        }

        [Fact]
        public void ProjectListCommandTest()
        {
            ProjectListCommand cm = new ProjectListCommand();
            var context = new CommandContext(_remainingArgs, "list", null);

            //IssueListCommand.Settings settings = new IssueListCommand.Settings() { ProjectId = 59, Status = "New", CreatedOnDate = "2022-09-28|2022-10-28" };
            //IssueListCommand.Settings settings = new IssueListCommand.Settings() 
            //{ 
            //    ProjectId = 59, 
            //    Status="Assigned", 
            //    AssigneeName = "George Gegos", 
            //    CreatedOnDate = "2022-10-05|2022-10-05", 
            //    TargetVersion = "Release 2.4.4.1" , 
            //    Author = "Lida Zarkada", 
            //    Priority="Immediate",
            //    Tracker = "DevTask"
            //};

            ProjectListCommand.Settings settings = new ProjectListCommand.Settings()
            {
                //Like = "Intern"
            };
            var result = cm.Execute(context, settings);
        }
        [Fact]
        public void IssueListCommandTest()
        {
            IssueListCommand cm = new IssueListCommand();
            var context = new CommandContext(_remainingArgs, "list", null);

            //IssueListCommand.Settings settings = new IssueListCommand.Settings() { ProjectId = 59, Status = "New", CreatedOnDate = "2022-09-28|2022-10-28" };
            //IssueListCommand.Settings settings = new IssueListCommand.Settings() 
            //{ 
            //    ProjectId = 59, 
            //    Status="Assigned", 
            //    AssigneeName = "George Gegos", 
            //    CreatedOnDate = "2022-10-05|2022-10-05", 
            //    TargetVersion = "Release 2.4.4.1" , 
            //    Author = "Lida Zarkada", 
            //    Priority="Immediate",
            //    Tracker = "DevTask"
            //};

            IssueListCommand.Settings settings = new IssueListCommand.Settings()
            {
                ProjectId = 59,
                //Status = "Assigned",
                AssigneeName = "George Gegos",
                //CreatedOnDate = "2022-10-05|2022-10-05",
                TargetVersion = "Release 2.4.4.1",
                //Author = "Lida Zarkada",
                //Priority = "Immediate",
                //Tracker = "DevTask"
            };
            var result = cm.Execute(context, settings);
        }

        [Fact]
        public void ReleaseNotesBuildCommandTest()
        {
            ReleaseNotesBuildCommand cm = new ReleaseNotesBuildCommand();
            var context = new CommandContext(_remainingArgs, "list", null);
            ReleaseNotesBuildCommand.Settings settings = new ReleaseNotesBuildCommand.Settings() {QueryId = "272", ProjectId="59", ExtractPath =@"C:\Temp\" };
            var result = cm.Execute(context, settings);
        }

        [Fact]
        public void UploadRedmineFileCommandTest()
        {
            UploadRedmineFileCommand cm = new UploadRedmineFileCommand();
            var context = new CommandContext(_remainingArgs, "list", null);

            //IssueListCommand.Settings settings = new IssueListCommand.Settings() { ProjectId = 59, Status = "New", CreatedOnDate = "2022-09-28|2022-10-28" };
            //IssueListCommand.Settings settings = new IssueListCommand.Settings() 
            //{ 
            //    ProjectId = 59, 
            //    Status="Assigned", 
            //    AssigneeName = "George Gegos", 
            //    CreatedOnDate = "2022-10-05|2022-10-05", 
            //    TargetVersion = "Release 2.4.4.1" , 
            //    Author = "Lida Zarkada", 
            //    Priority="Immediate",
            //    Tracker = "DevTask"
            //};

            UploadRedmineFileCommand.Settings settings = new UploadRedmineFileCommand.Settings()
            {
                ProjectId = 59,
                FullFilePath = @"C:\Temp\test.zip"
            };
            var result = cm.Execute(context, settings);
        }
    }
}
