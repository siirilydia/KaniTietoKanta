using System;
using System.Collections.Generic;

namespace KaniTietoKantaAPI.Models
{
    public partial class Poikueet
    {
        public Poikueet()
        {
            Kani = new HashSet<Kani>();
            Poikastiedot = new HashSet<Poikastiedot>();
        }

        public int PoikueId { get; set; }
        public int? EmäId { get; set; }
        public string EmäVnimi { get; set; }
        public string EmäKnimi { get; set; }
        public string EmäVk { get; set; }
        public int? IsäId { get; set; }
        public string IsäVnimi { get; set; }
        public string IsäKnimi { get; set; }
        public string IsäVk { get; set; }
        public int? Poikaslkm { get; set; }
        public DateTime? Syntymäpv { get; set; }
        public string Rotu { get; set; }
        public string Tietoja { get; set; }
        public string Email { get; set; }

        public virtual Kani Emä { get; set; }
        public virtual Kani Isä { get; set; }
        public virtual ICollection<Kani> Kani { get; set; }
        public virtual ICollection<Poikastiedot> Poikastiedot { get; set; }
    }
}
