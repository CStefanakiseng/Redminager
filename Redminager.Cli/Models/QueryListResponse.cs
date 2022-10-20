using Redminager.Cli.Models.Redmine;
using System.Collections.Generic;

namespace Redminager.Cli.Models
{
    public class QueryListResponse : ListResponse
    {
        public List<Query> Queries { get; set; }
    }
}
