using Employment.Infrastructure.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Comments
    {
        public int id {  get; set; }    
        public string UserID { get; set; }
        public int PostID { get; set; }
        public string Comment{ get; set; }
        public DateTime CreatedAt{ get; set; }
        public AppUser User { get; set; }
        public Posts Post { get; set; }
        public ICollection<LikesOnComments> Likes { get; set; }= new List<LikesOnComments>();
    }
}
