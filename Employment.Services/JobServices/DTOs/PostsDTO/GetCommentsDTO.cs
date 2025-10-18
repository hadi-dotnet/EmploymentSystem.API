using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.PostsDTO
{
    public class GetCommentsDTO
    {
        public int id { get; set; }
        public string UserID { get; set; }
        public string FillName { get; set; }
        public string Comment { get; set; }
        public int TotalLikes { get; set; }
        public DateTime Created { get; set; }
    }
}
