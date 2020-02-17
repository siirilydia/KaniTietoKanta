using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KaniTietoKantaMVC.Models
{
    public partial class Poikueet
    {
        public Poikueet()
        {
            Kani = new HashSet<Kani>();
        }

        public int PoikueId { get; set; }
        public int? EmäId { get; set; }
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string EmäVnimi { get; set; }
        [Required(ErrorMessage = "Syötä emän kutsumanimi")]
        [StringLength(50, ErrorMessage = "Maksimipituus 50 merkkiä")]
        public string EmäKnimi { get; set; }
        public string EmäVk { get; set; }
        public int? IsäId { get; set; }
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string IsäVnimi { get; set; }
        [Required(ErrorMessage = "Syötä isän kutsumanimi")]
        [StringLength(50, ErrorMessage = "Maksimipituus 50 merkkiä")]
        public string IsäKnimi { get; set; }
        public string IsäVk { get; set; }
        public int? Poikaslkm { get; set; }

        [Required(ErrorMessage = "Syötä poikueen syntymäpäivä")]
        [DataType(DataType.Date)]
        public DateTime? Syntymäpv { get; set; }

        [Required(ErrorMessage = "Syötä poikueen rotu")]
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string Rotu { get; set; }

        [StringLength(300, ErrorMessage = "Maksimipituus 300 merkkiä")]
        public string Tietoja { get; set; }

        public string Email { get; set; }

        public virtual Kani Emä { get; set; }
        public virtual Kani Isä { get; set; }
        public virtual ICollection<Kani> Kani { get; set; }
    }
}
