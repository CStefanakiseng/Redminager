using Microsoft.Extensions.Configuration;
using Redminager.Cli.Services.Contracts;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Services
{
    public class RedmineDataService : IRedmineDataService
    {
        private readonly IConfiguration _config;
        private readonly string _host;
        private readonly string _apiKey;
        private readonly RedmineManager _redmineManager;


        public RedmineDataService(IConfiguration config)
        {
            _config = config;
            _host = _config["Redmine:Host"];
            _apiKey = _config["Redmine:ApiKey"];
            _redmineManager = new RedmineManager(_host, _apiKey);

        }
        public RedmineDataService(string host, string apiKey)
        {
            _host = host;
            _apiKey = apiKey;
            _redmineManager = new RedmineManager(_host, _apiKey);
        }

        public T GetObject<T>(string id, NameValueCollection criteria) where T : class, new()
        {
            T result = _redmineManager.GetObject<T>(id, criteria);
            return result;
        }

        

        //public RedmineDataService()
        //{

        //    _host = @"https://redmine.dataverse.gr/";
        //    _apiKey = @"6abba14f137a25a657c0bd330cf4b8abf75e2a63";
        //    _redmineManager = new RedmineManager(_host, _apiKey);

        //}       


        public IEnumerable<T> GetObjects<T>(NameValueCollection criteria) where T : class, new()
        {
            
            if (criteria == null)
            {
                return _redmineManager.GetObjects<T>();
            }

            return _redmineManager.GetObjects<T>(criteria);
        }

       

        public bool HealthCheck()
        {
            NameValueCollection criteria = new NameValueCollection { { Redmine.Net.Api.RedmineKeys.PROJECT_ID, "59" } };

            var project = GetObjects<Project>(criteria);
            return (project != null) ? true : false;

        }
    }
}
