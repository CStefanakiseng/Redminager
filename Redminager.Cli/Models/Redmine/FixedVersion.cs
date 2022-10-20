using System;

namespace Redminager.Cli.Models.Redmine
{
    public class FixedVersion
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Due_Date { get; set; }
        public string Sharing { get; set; }
        public string Wiki_Page_Title { get; set; }
        public DateTime Created_On { get; set; }
        public DateTime Updated_On { get; set; }
    }
}
