﻿using Redminager.Cli.Models.Redmine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redminager.Cli.Models
{
    public class IssueListResponse:ListResponse
    {
        public List<Issue> Issues { get; set; }
       
    }
}
