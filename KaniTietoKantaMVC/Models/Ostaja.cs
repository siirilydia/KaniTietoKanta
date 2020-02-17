using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KaniTietoKantaMVC.Models
{
    public partial class Ostaja
    {
        public Ostaja()
        {
            Poikastiedot = new HashSet<Poikastiedot>();
        }

        public int OstajaId { get; set; }
        public int PoikanenId { get; set; }

        [Required (ErrorMessage = "Syötä ostajan nimi")]
        [StringLength(300, ErrorMessage = "Maksimipituus 300 merkkiä")]
        public string Nimi { get; set; }

        [StringLength(20, ErrorMessage = "Maksimipituus 20 merkkiä")]
        public string Puhnro { get; set; }

        [StringLength(320, ErrorMessage = "Maksimipituus 320 merkkiä")]
        public string OEmail { get; set; }

        [Required(ErrorMessage = "Syötä ostopäivä")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Ostopäivä { get; set; }

        public double? Hinta { get; set; }

        public bool Maksettu { get; set; }

        [StringLength(300, ErrorMessage = "Maksimipituus 300 merkkiä")]
        public string Tietoja { get; set; }

        public string Email { get; set; }

        public virtual Poikastiedot Poikanen { get; set; }
        public virtual ICollection<Poikastiedot> Poikastiedot { get; set; }
    }
}
