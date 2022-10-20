using Redminager.Cli.Models.Redmine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Models
{
    public class UploadFileResponse
    {
        public File File { get; set; }
        public int ProjectId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
