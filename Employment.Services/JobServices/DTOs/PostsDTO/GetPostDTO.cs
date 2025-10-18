using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.PostsDTO
{
    public class GetPostDTO
    {
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string FillName { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public int TotalLikes { get; set; }
        public int TotalComment { get; set; }
        public DateTime DateCreated { get; set; }
       

    }

  



}
