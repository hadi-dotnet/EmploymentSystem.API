using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.PostsDTO
{
    public class GetLikesDTO
    {
        public int id { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
    }
}
