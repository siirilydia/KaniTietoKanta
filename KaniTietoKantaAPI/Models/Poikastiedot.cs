using System;
using System.Collections.Generic;

namespace KaniTietoKantaAPI.Models
{
    public partial class Poikastiedot
    {
        public int PoikanenId { get; set; }
        public int PoikueId { get; set; }
        public int? KaniId { get; set; }
        public string Nimi { get; set; }
        public DateTime? Syntymäpv { get; set; }
        public string Väri { get; set; }
        public string Sukupuoli { get; set; }
        public string Tietoja { get; set; }
        public int? OstajaId { get; set; }
        public string Email { get; set; }

        public virtual Kani Kani { get; set; }
        public virtual Poikueet Poikue { get; set; }
    }
}
