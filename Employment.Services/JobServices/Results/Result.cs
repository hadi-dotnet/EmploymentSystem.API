using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Results
{
    public class Result
    {
        public bool Success { get; set; }

        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

    }
}
