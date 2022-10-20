using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Models.Redmine
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public bool Admin { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mail { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public DateTime? PasswdChangedOn { get; set; }
    }
}
