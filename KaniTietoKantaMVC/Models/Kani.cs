using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KaniTietoKantaMVC.Models
{
    public partial class Kani
    {
        public Kani()
        {
            AstutuksetEmä = new HashSet<Astutukset>();
            AstutuksetIsä = new HashSet<Astutukset>();
            PoikueetEmä = new HashSet<Poikueet>();
            PoikueetIsä = new HashSet<Poikueet>();
        }

        public int KaniId { get; set; }

        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string Nimi { get; set; }

        [Required(ErrorMessage = "Syötä kutsumanimi")]
        [StringLength(50, ErrorMessage = "Maksimipituus 50 merkkiä")]
        public string Kutsumanimi { get; set; }

        [Required(ErrorMessage = "Syötä syntymäpäivä")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Syntymäpäivä { get; set; }
        public bool Kuollut { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Kuolinpäivä { get; set; }
        [StringLength(250, ErrorMessage = "Maksimipituus 250 merkkiä")]

        public string Väri { get; set; }
        public string Ok { get; set; }
        public string Vk { get; set; }
        public string Sukupuoli { get; set; }
        public bool Rokotettu { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Rokotuspv { get; set; }

        public int? PoikueenId { get; set; }
        public string IsänNimi { get; set; }
        public string EmänNimi { get; set; }


        [StringLength(300, ErrorMessage = "Maksimipituus 300 merkkiä")]
        public string Tietoja { get; set; }

        [Required(ErrorMessage = "Syötä kanin rotu")]
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string Rotu { get; set; }

        public bool Myyty { get; set; }

        [StringLength(300, ErrorMessage = "Maksimipituus 300 merkkiä")]
        public string OstajanTiedot { get; set; }

        [StringLength(300, ErrorMessage = "Maksimipituus 300 merkkiä")]
        public string KasvattajanTiedot { get; set; }


        public string Email { get; set; }

        public virtual Poikueet Poikueet { get; set; }
        public virtual ICollection<Astutukset> AstutuksetEmä { get; set; }
        public virtual ICollection<Astutukset> AstutuksetIsä { get; set; }
        public virtual ICollection<Poikueet> PoikueetEmä { get; set; }
        public virtual ICollection<Poikueet> PoikueetIsä { get; set; }


    }
}
