using Employment.Infrastructure.Entitys;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Posts
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public AppUser User { get; set; }
        public ICollection<Comments> Comments { get; set; } = new List<Comments>(); 
        public ICollection<Likes> Likes { get; set; } = new List<Likes>();
    }
}
