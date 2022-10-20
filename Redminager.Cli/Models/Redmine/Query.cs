namespace Redminager.Cli.Models.Redmine
{
    public class Query
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Is_Public { get; set; }
        public int? Project_Id { get; set; }
    }
}
