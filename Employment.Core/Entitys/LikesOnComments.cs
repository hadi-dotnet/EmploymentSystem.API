using Employment.Infrastructure.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class LikesOnComments
    {
        public int id {  get; set; }
        public string UserID { get; set; }
        public int CommentID { get; set; }
        public Comments Comment { get; set; }
        public AppUser User { get; set; }
    }
}
