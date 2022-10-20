using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Services.Contracts
{
    public interface IRedmineDataService
    {
        bool HealthCheck();
        IEnumerable<T> GetObjects<T>(NameValueCollection criteria) where T : class, new();
        //Task<IEnumerable<T>> GetObjectsAsync<T>(NameValueCollection criteria) where T : class, new();
        T GetObject<T>(string id, NameValueCollection criteria) where T : class, new();
       //Task<T> GetObjectAsync<T>(string id, NameValueCollection criteria) where T : class, new();
    }
}
