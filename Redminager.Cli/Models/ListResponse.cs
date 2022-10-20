using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Models
{
    public class ListResponse:IListResponse
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Total_Count { get; set; }
    }
}
