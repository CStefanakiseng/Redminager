using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
using System.Text.Json;
using Redminager.Cli.Models;
using Redminager.Cli.Models.Redmine;
using Redminager.Cli.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;
using System.Net.Http.Headers;
using RestSharp;
using RestSharp.Serializers;

namespace Redminager.Cli.Services
{
    public class RedmineClientService : IRedmineClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly string _host;
        private readonly string _apiKey;

        public RedmineClientService(IConfiguration config)
        {
            _config = config;
            _host = _config["Redmine:Host"];
            _apiKey = _config["Redmine:ApiKey"];
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", _apiKey);
            _httpClient.BaseAddress = new Uri(_host);
        }
        public RedmineClientService(string host, string apiKey)
        {
            _host = host;
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", _apiKey);
            _httpClient.BaseAddress = new Uri(_host);
        }
        
        public async Task<Project> GetProjectAsync(int id, Dictionary<string, string> queryParms = null)
        {
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            string url = $"projects//{id}//.json";
            string requestUrl = BuildUrl(url, queryParms);
            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectString = await httpResponse.Content.ReadAsStringAsync();
                Project resp = JsonSerializer.Deserialize<Project>(dataObjectString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return resp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<ProjectListResponse> GetProjectsAsync(Dictionary<string, string> parametersDict = null)
        {
            ProjectListResponse totalResp = new ProjectListResponse();

            //query params
            if (parametersDict == null)
                parametersDict = new Dictionary<string, string>();

            if (!parametersDict.ContainsKey("offset"))
            {
                parametersDict.Add("offset", "0");
            }
            if (!parametersDict.ContainsKey("limit"))
            {
                parametersDict.Add("limit", "100");
            }

            string url = $"projects.json";
            string requestUrl = BuildUrl(url, parametersDict);

            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectsString = await httpResponse.Content.ReadAsStringAsync();

                ProjectListResponse resp = JsonSerializer.Deserialize<ProjectListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                totalResp.Projects = resp.Projects;
                while (totalResp.Projects.Count < resp.Total_Count)
                {
                    parametersDict["offset"] = $"{int.Parse(parametersDict["offset"]) + 100}";

                    requestUrl = BuildUrl(url, parametersDict);
                    httpResponse = await _httpClient.GetAsync(requestUrl);
                    dataObjectsString = await httpResponse.Content.ReadAsStringAsync();
                    resp = JsonSerializer.Deserialize<ProjectListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    totalResp.Projects.AddRange(resp.Projects);
                }
                totalResp.Total_Count = resp.Total_Count;
                totalResp.Limit = totalResp.Projects.Count;
                totalResp.Offset = 0;

                return totalResp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<Issue> GetIssueAsync(int id, Dictionary<string, string> queryParms=null)
        {
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            string url = $"issues//{id}//.json";
            string requestUrl = BuildUrl(url, queryParms);
            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectString = await httpResponse.Content.ReadAsStringAsync();
                Issue resp = JsonSerializer.Deserialize<Issue>(dataObjectString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return resp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<IssueListResponse> GetIssuesAsync(int projectId, Dictionary<string, string> parametersDict = null)
        {
            IssueListResponse totalResp = new IssueListResponse();

            //query params
            if (parametersDict == null)
                parametersDict = new Dictionary<string, string>();

            if (!parametersDict.ContainsKey("offset"))
            {
                parametersDict.Add("offset", "0");
            }
            if (!parametersDict.ContainsKey("limit"))
            {
                parametersDict.Add("limit", "100");
            }

            string url = $"projects/{projectId}//issues.json";
            string requestUrl = BuildUrl(url, parametersDict);

            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectsString = await httpResponse.Content.ReadAsStringAsync();

                IssueListResponse resp = JsonSerializer.Deserialize<IssueListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

              
                totalResp.Issues = resp.Issues;
                while (totalResp.Issues.Count < resp.Total_Count)
                {
                    parametersDict["offset"] = $"{int.Parse(parametersDict["offset"]) + 100}";

                    requestUrl = BuildUrl(url, parametersDict);
                    httpResponse = await _httpClient.GetAsync(requestUrl);
                    dataObjectsString = await httpResponse.Content.ReadAsStringAsync();
                    resp = JsonSerializer.Deserialize<IssueListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    totalResp.Issues.AddRange(resp.Issues);
                }
                totalResp.Total_Count = resp.Total_Count;
                totalResp.Limit = totalResp.Issues.Count;
                totalResp.Offset = 0;

                return totalResp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<Query> GetQueryAsync(int id, Dictionary<string, string> queryParms = null)
        {
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            var query = (await GetQueriesAsync(queryParms)).Queries.FirstOrDefault(q => q.Id == id);
            return query;
        }

        public async Task<QueryListResponse> GetQueriesAsync( Dictionary<string, string> parametersDict = null)
        {
            QueryListResponse totalResp = new QueryListResponse();

            //query params
            if (parametersDict == null)
                parametersDict = new Dictionary<string, string>();

            if (!parametersDict.ContainsKey("offset"))
            {
                parametersDict.Add("offset", "0");
            }
            if (!parametersDict.ContainsKey("limit"))
            {
                parametersDict.Add("limit", "100");
            }

            string url = $"queries.json";
            string requestUrl = BuildUrl(url, parametersDict);

            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectsString = await httpResponse.Content.ReadAsStringAsync();

                QueryListResponse resp = JsonSerializer.Deserialize<QueryListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                totalResp.Queries = resp.Queries;
                while (totalResp.Queries.Count < resp.Total_Count)
                {
                    parametersDict["offset"] = $"{int.Parse(parametersDict["offset"]) + 100}";

                    requestUrl = BuildUrl(url, parametersDict);
                    httpResponse = await _httpClient.GetAsync(requestUrl);
                    dataObjectsString = await httpResponse.Content.ReadAsStringAsync();
                    resp = JsonSerializer.Deserialize<QueryListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    totalResp.Queries.AddRange(resp.Queries);
                }
                totalResp.Total_Count = resp.Total_Count;
                totalResp.Limit = totalResp.Queries.Count;
                totalResp.Offset = 0;

                return totalResp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }          

        public async Task<User> GetUserAsync(int id, Dictionary<string, string> queryParms = null)
        {
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            string url = $"users//{id}//.json";
            string requestUrl = BuildUrl(url, queryParms);
            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectString = await httpResponse.Content.ReadAsStringAsync();
                User resp = JsonSerializer.Deserialize<User>(dataObjectString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return resp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<UserListResponse> GetUsersAsync(Dictionary<string, string> queryParms = null)
        {
            UserListResponse totalResp = new UserListResponse();

            //query params
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            if (!queryParms.ContainsKey("offset"))
            {
                queryParms.Add("offset", "0");
            }
            if (!queryParms.ContainsKey("limit"))
            {
                queryParms.Add("limit", "100");
            }

            string url = $"users.json";
            string requestUrl = BuildUrl(url, queryParms);

            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectsString = await httpResponse.Content.ReadAsStringAsync();

                UserListResponse resp = JsonSerializer.Deserialize<UserListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                totalResp.Users = resp.Users;
                while (totalResp.Users.Count < resp.Total_Count)
                {
                    queryParms["offset"] = $"{int.Parse(queryParms["offset"]) + 100}";

                    requestUrl = BuildUrl(url, queryParms);
                    httpResponse = await _httpClient.GetAsync(requestUrl);
                    dataObjectsString = await httpResponse.Content.ReadAsStringAsync();
                    resp = JsonSerializer.Deserialize<UserListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    totalResp.Users.AddRange(resp.Users);
                }
                totalResp.Total_Count = resp.Total_Count;
                totalResp.Limit = totalResp.Users.Count;
                totalResp.Offset = 0;

                return totalResp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<IssuePriorityListResponse> GetIssuePriorities(Dictionary<string, string> queryParms = null)
        {
            IssuePriorityListResponse totalResp = new IssuePriorityListResponse();

            //query params
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            if (!queryParms.ContainsKey("offset"))
            {
                queryParms.Add("offset", "0");
            }
            if (!queryParms.ContainsKey("limit"))
            {
                queryParms.Add("limit", "100");
            }

            string url = $"enumerations/issue_priorities.json";
            string requestUrl = BuildUrl(url, queryParms);

            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectsString = await httpResponse.Content.ReadAsStringAsync();

                IssuePriorityListResponse resp = JsonSerializer.Deserialize<IssuePriorityListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                totalResp.Issue_Priorities = resp.Issue_Priorities;
                while (totalResp.Issue_Priorities.Count < resp.Total_Count)
                {
                    queryParms["offset"] = $"{int.Parse(queryParms["offset"]) + 100}";

                    requestUrl = BuildUrl(url, queryParms);
                    httpResponse = await _httpClient.GetAsync(requestUrl);
                    dataObjectsString = await httpResponse.Content.ReadAsStringAsync();
                    resp = JsonSerializer.Deserialize<IssuePriorityListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    totalResp.Issue_Priorities.AddRange(resp.Issue_Priorities);
                }
                totalResp.Total_Count = resp.Total_Count;
                totalResp.Limit = totalResp.Issue_Priorities.Count;
                totalResp.Offset = 0;

                return totalResp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<StatusListResponse> GetStatuses(Dictionary<string, string> queryParms = null)
        {
            StatusListResponse totalResp = new StatusListResponse();

            //query params
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            if (!queryParms.ContainsKey("offset"))
            {
                queryParms.Add("offset", "0");
            }
            if (!queryParms.ContainsKey("limit"))
            {
                queryParms.Add("limit", "100");
            }

            string url = $"issue_statuses.json";
            string requestUrl = BuildUrl(url, queryParms);

            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectsString = await httpResponse.Content.ReadAsStringAsync();

                StatusListResponse resp = JsonSerializer.Deserialize<StatusListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                totalResp.Issue_Statuses = resp.Issue_Statuses;
                while (totalResp.Issue_Statuses.Count < resp.Total_Count)
                {
                    queryParms["offset"] = $"{int.Parse(queryParms["offset"]) + 100}";

                    requestUrl = BuildUrl(url, queryParms);
                    httpResponse = await _httpClient.GetAsync(requestUrl);
                    dataObjectsString = await httpResponse.Content.ReadAsStringAsync();
                    resp = JsonSerializer.Deserialize<StatusListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    totalResp.Issue_Statuses.AddRange(resp.Issue_Statuses);
                }
                totalResp.Total_Count = resp.Total_Count;
                totalResp.Limit = totalResp.Issue_Statuses.Count;
                totalResp.Offset = 0;

                return totalResp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        
        public async Task<VersionListResponse> GetVersions(int projectId, Dictionary<string, string> queryParms = null)
        {
            VersionListResponse totalResp = new VersionListResponse();

            //query params
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            if (!queryParms.ContainsKey("offset"))
            {
                queryParms.Add("offset", "0");
            }
            if (!queryParms.ContainsKey("limit"))
            {
                queryParms.Add("limit", "100");
            }

            string url = $"projects/{projectId}/versions.json";
            string requestUrl = BuildUrl(url, queryParms);

            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectsString = await httpResponse.Content.ReadAsStringAsync();

                VersionListResponse resp = JsonSerializer.Deserialize<VersionListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                totalResp.Versions = resp.Versions;
                while (totalResp.Versions.Count < resp.Total_Count)
                {
                    queryParms["offset"] = $"{int.Parse(queryParms["offset"]) + 100}";

                    requestUrl = BuildUrl(url, queryParms);
                    httpResponse = await _httpClient.GetAsync(requestUrl);
                    dataObjectsString = await httpResponse.Content.ReadAsStringAsync();
                    resp = JsonSerializer.Deserialize<VersionListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    totalResp.Versions.AddRange(resp.Versions);
                }
                totalResp.Total_Count = resp.Total_Count;
                totalResp.Limit = totalResp.Versions.Count;
                totalResp.Offset = 0;

                return totalResp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<TrackerListResponse> GetTrackers(Dictionary<string, string> queryParms = null)
        {
            TrackerListResponse totalResp = new TrackerListResponse();

            //query params
            if (queryParms == null)
                queryParms = new Dictionary<string, string>();

            if (!queryParms.ContainsKey("offset"))
            {
                queryParms.Add("offset", "0");
            }
            if (!queryParms.ContainsKey("limit"))
            {
                queryParms.Add("limit", "100");
            }

            string url = $"trackers.json";
            string requestUrl = BuildUrl(url, queryParms);

            var httpResponse = await _httpClient.GetAsync(requestUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var dataObjectsString = await httpResponse.Content.ReadAsStringAsync();

                TrackerListResponse resp = JsonSerializer.Deserialize<TrackerListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                totalResp.Trackers = resp.Trackers;
                while (totalResp.Trackers.Count < resp.Total_Count)
                {
                    queryParms["offset"] = $"{int.Parse(queryParms["offset"]) + 100}";

                    requestUrl = BuildUrl(url, queryParms);
                    httpResponse = await _httpClient.GetAsync(requestUrl);
                    dataObjectsString = await httpResponse.Content.ReadAsStringAsync();
                    resp = JsonSerializer.Deserialize<TrackerListResponse>(dataObjectsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    totalResp.Trackers.AddRange(resp.Trackers);
                }
                totalResp.Total_Count = resp.Total_Count;
                totalResp.Limit = totalResp.Trackers.Count;
                totalResp.Offset = 0;

                return totalResp;
            }
            else
            {
                throw new Exception($"Status: {httpResponse.StatusCode.ToString()}");
            }
        }

        public async Task<UploadFileResponse> UploadFileToProject(int projectId, string fullFileName)
        {
            // Convert file into Byte Array
            byte[] byteData; ;
            using (IO.FileStream fileStream = new IO.FileStream(fullFileName, IO.FileMode.Open, IO.FileAccess.Read))
            {
                IO.BinaryReader binaryReader = new IO.BinaryReader(fileStream);
                byteData = binaryReader.ReadBytes((int)fileStream.Length);
            }

            //First we need to post the file to the uploads and receive the upload token

            var httpClient = new HttpClient();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", _apiKey);
            httpClient.BaseAddress = new Uri(_host);

            HttpResponseMessage registerUploadHttpResponse;
            RegisterUploadResponse registerUploadResponse = new RegisterUploadResponse();
            var fileName = IO.Path.GetFileName(fullFileName);
            var registerUrl = $"uploads.json?filename={fileName}";
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                registerUploadHttpResponse = await httpClient.PostAsync(registerUrl, content);
                if (registerUploadHttpResponse.IsSuccessStatusCode)
                {
                    var registerUploadResponseObjectString = await registerUploadHttpResponse.Content.ReadAsStringAsync();
                    //registerUploadResponse = JsonSerializer.Deserialize<RegisterUploadResponse>(registerUploadResponseObjectString,null);
                    registerUploadResponse = JsonSerializer.Deserialize<RegisterUploadResponse>(registerUploadResponseObjectString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
               
            }

            if (registerUploadHttpResponse.IsSuccessStatusCode)
            {
                //Then we use this token to relate the uploaded file with the project we want
                var addToProjectUrl = $"projects/{projectId}/files.json";
                var file = new File()
                {
                    token = registerUploadResponse.Upload.Token,
                    fileName = fileName
                };

                var addFileRequest = new AddFileRequest { file = file };



                var requestData = JsonSerializer.Serialize(addFileRequest /*, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }*/);
                RestResponse addToProjectHttpResponse;

                using (var client = new RestClient(_host))
                {
                    var request = new RestRequest(addToProjectUrl, Method.Post);
                    request.AddHeader("X-Redmine-API-Key", _apiKey);
                    request.AddStringBody(requestData, DataFormat.Json);

                    addToProjectHttpResponse = await client.PostAsync(request);
                }
                var addToProjectResponseObjectString = addToProjectHttpResponse.Content;
                if (addToProjectHttpResponse.IsSuccessStatusCode)
                {
                    return new UploadFileResponse()
                    {
                        Success = true,
                        File = file,
                        ProjectId = projectId,
                        Message = String.Empty
                    };
                }
                else
                {
                    return new UploadFileResponse()
                    {
                        Success = false,
                        File = file,
                        ProjectId = projectId,
                        Message = $"Adding upload with token {file.token} to project with id {projectId} has failed."
                    };
                }

            }
            else 
            {
                return new UploadFileResponse()
                {
                    Success = false,
                    File = null,
                    ProjectId = projectId,
                    Message = $"failed to upload file."
                };
            }
        }

        private string BuildUrl(string url, Dictionary<string, string> queryParamsDict)
        {
            var requestUrl = url;
            if (queryParamsDict.Keys.Count > 0)
            {
                requestUrl += "?";
                foreach (var key in queryParamsDict.Keys)
                {
                    requestUrl += string.Format($"{key}={queryParamsDict[key]}&");
                }
                requestUrl = requestUrl.Remove(requestUrl.Length - 1);
            }

            return requestUrl;
        }

       
    }
}
