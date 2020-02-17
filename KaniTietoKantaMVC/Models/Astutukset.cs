using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KaniTietoKantaMVC.Models
{
    public partial class Astutukset
    {
        public int AstutusId { get; set; }
        [Required(ErrorMessage = "Syötä astutuspäivä")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Astutuspäivä { get; set; }

        public int? IsäId { get; set; }

        [Required(ErrorMessage = "Syötä isän kutsumanimi")]
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string IsäKnimi { get; set; }

        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string IsäVnimi { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? IsäSyntymäpv { get; set; }

        [StringLength(150, ErrorMessage = "Maksimipituus 150 merkkiä")]
        public string IsäVäri { get; set; }

        [Required(ErrorMessage = "Syötä isän rotu")]
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string IsäRotu { get; set; }

        public int? EmäId { get; set; }

        [Required(ErrorMessage = "Syötä emän kutsumanimi")]
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string EmäKnimi { get; set; }

        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string EmäVnimi { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? EmäSyntymäpv { get; set; }

        [StringLength(150, ErrorMessage = "Maksimipituus 150 merkkiä")]
        public string EmäVäri { get; set; }

        [Required(ErrorMessage = "Syötä emän rotu")]
        [StringLength(100, ErrorMessage = "Maksimipituus 100 merkkiä")]
        public string EmäRotu { get; set; }

        [StringLength(500, ErrorMessage = "Maksimipituus 500 merkkiä")]
        public string Lisätietoja { get; set; }

        public string Email { get; set; }

        public virtual Kani Emä { get; set; }
        public virtual Kani Isä { get; set; }
    }
}
