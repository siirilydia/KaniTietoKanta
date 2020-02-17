using System;
using System.Collections.Generic;

namespace KaniTietoKantaAPI.Models
{
    public partial class Astutukset
    {
        public int AstutusId { get; set; }
        public DateTime Astutuspäivä { get; set; }
        public int? IsäId { get; set; }
        public string IsäKnimi { get; set; }
        public string IsäVnimi { get; set; }
        public DateTime? IsäSyntymäpv { get; set; }
        public string IsäVäri { get; set; }
        public string IsäRotu { get; set; }
        public int? EmäId { get; set; }
        public string EmäKnimi { get; set; }
        public string EmäVnimi { get; set; }
        public DateTime? EmäSyntymäpv { get; set; }
        public string EmäVäri { get; set; }
        public string EmäRotu { get; set; }
        public string Lisätietoja { get; set; }
        public string Email { get; set; }

        public virtual Kani Emä { get; set; }
        public virtual Kani Isä { get; set; }
    }
}
