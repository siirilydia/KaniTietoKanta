using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KaniTietoKantaMVC.Models
{
    public partial class Poikastiedot
    {
        public Poikastiedot()
        {
            Ostaja = new HashSet<Ostaja>();
        }

        public int PoikanenId { get; set; }
        public int PoikueId { get; set; }
        public int? KaniId { get; set; }

        [Required(ErrorMessage = "Syötä nimi")]
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string Nimi { get; set; }
        public DateTime? Syntymäpv { get; set; }
        [StringLength(250, ErrorMessage = "Maksimipituus 250 merkkiä")]
        public string Väri { get; set; }
        public string Sukupuoli { get; set; }
        [StringLength(300, ErrorMessage = "Maksimipituus 300 merkkiä")]
        public string Tietoja { get; set; }
        public int? OstajaId { get; set; }
        public string Email { get; set; }

        public virtual Kani Kani { get; set; }
        public virtual Ostaja OstajaNavigation { get; set; }
        public virtual Poikueet Poikue { get; set; }
        public virtual ICollection<Ostaja> Ostaja { get; set; }
    }
}
