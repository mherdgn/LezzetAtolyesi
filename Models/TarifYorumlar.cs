using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LezzetAtolyesi.Models
{
    public class TarifYorumlar
    {
        public YemekTarifleri tarif {  get; set; }

        public List<Yorumlar> yorumlar { get; set; }
    }
}
