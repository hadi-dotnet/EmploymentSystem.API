using Job.Services.JobServices.DTOs.JobDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Results
{
    public class PageResult<T>
    {
       
        public int PageNumber { get; set; }    
        public int PageSize { get; set; }       
        public List<T> Items { get; set; }      
    }

}
