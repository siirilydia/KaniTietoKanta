using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaniTietoKantaMVC.Models;

namespace KaniTietoKantaMVC.ViewModels
{
    public class PoikueTietoNäkymä
    {
        public Poikueet poikue { get; set; }
        public List<Poikastiedot> pt { get; set; }
        public List<Ostaja> ostajat { get; set; }

    }
}
