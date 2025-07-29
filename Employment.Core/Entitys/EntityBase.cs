using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Core.Entitys
{
    public abstract class EntityBase
    {
        public int ID { get; }
        public DateTime CreatedAT { get; set; }
        public DateTime UpdatedAT { get; set; }
    }
}
