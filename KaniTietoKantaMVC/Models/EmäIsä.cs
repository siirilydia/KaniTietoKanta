using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KaniTietoKantaMVC.Models
{
    public class EmäIsä
    {
        public string Vnimi { get; set; }
        public string Knimi { get; set; }
        //public DateTime SyntymäPv { get; set; }
        public int? KaniId { get; set; }
        //public int? PoikueenId { get; set; }
    }
}
