using Redminager.Cli.Models;
using Redminager.Cli.Models.Redmine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Services.Contracts
{
    public interface IRedmineClientService
    {
        Task<UploadFileResponse> UploadFileToProject(int projectId, string fullFileName);
        Task<TrackerListResponse> GetTrackers(Dictionary<string, string> queryParms = null);
        Task<VersionListResponse> GetVersions(int projectId, Dictionary<string, string> queryParms = null);
        Task<StatusListResponse> GetStatuses(Dictionary<string, string> queryParms = null);
        Task<IssuePriorityListResponse> GetIssuePriorities(Dictionary<string, string> queryParms = null);
        Task<User> GetUserAsync(int id, Dictionary<string, string> queryParms = null);
        Task<UserListResponse> GetUsersAsync(Dictionary<string, string> queryParms = null);
        Task<Project> GetProjectAsync(int id, Dictionary<string, string> queryParms = null);
        Task<ProjectListResponse> GetProjectsAsync(Dictionary<string, string> parametersDict = null);
        Task<Issue> GetIssueAsync(int id, Dictionary<string, string> queryParms = null);
        Task<IssueListResponse> GetIssuesAsync(int projectId, Dictionary<string, string> parametersDict = null);
        Task<Query> GetQueryAsync(int id, Dictionary<string, string> queryParms = null);
        Task<QueryListResponse> GetQueriesAsync(Dictionary<string, string> parametersDict = null);


    }
}
