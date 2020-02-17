using System;
using System.Collections.Generic;

namespace KaniTietoKantaAPI.Models
{
    public partial class Kani
    {
        public Kani()
        {
            AstutuksetEmä = new HashSet<Astutukset>();
            AstutuksetIsä = new HashSet<Astutukset>();
            Poikastiedot = new HashSet<Poikastiedot>();
            PoikueetEmä = new HashSet<Poikueet>();
            PoikueetIsä = new HashSet<Poikueet>();
        }

        public int KaniId { get; set; }
        public string Nimi { get; set; }
        public string Kutsumanimi { get; set; }
        public DateTime? Syntymäpäivä { get; set; }

        public bool Kuollut { get; set; }
        public DateTime? Kuolinpäivä { get; set; }
        public string Väri { get; set; }
        public string Ok { get; set; }
        public string Vk { get; set; }
        public string Sukupuoli { get; set; }
        public bool Rokotettu { get; set; }
        public DateTime? Rokotuspv { get; set; }
        public int? PoikueenId { get; set; }
        public string IsänNimi { get; set; }
        public string EmänNimi { get; set; }
        public string Tietoja { get; set; }
        public string Rotu { get; set; }
        public bool Myyty { get; set; }
        public string OstajanTiedot { get; set; }
        public string KasvattajanTiedot { get; set; }
        public string Email { get; set; }

        public virtual Poikueet Poikueen { get; set; }
        public virtual ICollection<Astutukset> AstutuksetEmä { get; set; }
        public virtual ICollection<Astutukset> AstutuksetIsä { get; set; }
        public virtual ICollection<Poikastiedot> Poikastiedot { get; set; }
        public virtual ICollection<Poikueet> PoikueetEmä { get; set; }
        public virtual ICollection<Poikueet> PoikueetIsä { get; set; }
    }
}
