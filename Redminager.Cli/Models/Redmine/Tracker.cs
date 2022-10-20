using System.Collections.Generic;

namespace Redminager.Cli.Models.Redmine
{
    public class Tracker
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Status Default_Status { get; set; }
        public string Description { get; set; }
        public List<string> Enabled_Standard_Fields { get; set; }
    }
}
