using Employment.Infrastructure.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Employees
    {
        public string? UserID {  get; set; }
        public string? FirstName {  get; set; }
        public string? secoundName {  get; set; }
        public string? LastName {  get; set; }
        public string? Address {  get; set; }
        public string? AboutYou {  get; set; }
        public string? UniverCity {  get; set; }
        public string? Image { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<Skills> Skills { get; set; } = new List<Skills>();
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public ICollection<ApplyJob> ApplyJobs { get; set; } = new List<ApplyJob>();
        public ICollection<Connections> ConnectionSender { get; set; } = new List<Connections>();
        public ICollection<Connections> ConnectionReseiver { get; set; } = new List<Connections>();
    }
}
