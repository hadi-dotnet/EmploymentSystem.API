using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.ConnectionsDTO
{
    public enum EnumConnectStatusDTO
    {
        cancel = 0 , accept = 1
    }

    public class GetConnectionRequstDTO
    {
        public int id { get; set; }
        public string UserRequstID { get; set; }
        public string UserRequstName { get; set; }

    }
}
