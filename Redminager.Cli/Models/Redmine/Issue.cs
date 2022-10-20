using System;
using System.Collections.Generic;

namespace Redminager.Cli.Models.Redmine
{
    public class Issue
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public Tracker Tracker { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Category Category { get; set; }
        public Author Author { get; set; }
        public AssignedTo Assigned_To { get; set; }
        public FixedVersion Fixed_Version { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? Due_Date { get; set; }
        public int Done_Ratio { get; set; }
        public bool Is_Private { get; set; }
        public double? Estimated_Hours { get; set; }
        public float? Spent_Hours { get; set; }
        public List<CustomField> Custom_Fields { get; set; }
        public DateTime? Created_On { get; set; }
        public DateTime? Updated_On { get; set; }
        public DateTime? Closed_On { get; set; }
    }
}
