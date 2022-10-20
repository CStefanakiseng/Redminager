using System;
using System.Collections.Generic;

namespace Redminager.Cli.Models.Redmine
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public bool Is_Public { get; set; }
        public bool Inherit_Members { get; set; }
        public List<CustomField> Custom_Fields { get; set; }
        public int Enable_New_Ticket_Message { get; set; }
        //public object New_Ticket_Message { get; set; }
        public DateTime? Created_On { get; set; }
        public DateTime? Updated_On { get; set; }
        public Parent Parent { get; set; }
    }
}
