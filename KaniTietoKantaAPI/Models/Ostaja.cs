using System;
using System.Collections.Generic;

namespace KaniTietoKantaAPI.Models
{
    public partial class Ostaja
    {
        public int OstajaId { get; set; }
        public int PoikanenId { get; set; }
        public string Nimi { get; set; }
        public string Puhnro { get; set; }
        public string OEmail { get; set; }
        public DateTime? Ostopäivä { get; set; }
        public double? Hinta { get; set; }
        public bool Maksettu { get; set; }
        public string Tietoja { get; set; }
        public string Email { get; set; }
    }
}
