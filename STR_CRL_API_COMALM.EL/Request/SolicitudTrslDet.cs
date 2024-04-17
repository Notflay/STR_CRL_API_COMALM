using STR_CRL_API_COMALM.EL.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL.Request
{
    public class SolicitudTrslDet
    {
        public Articulo STR_ITEM { get; set; }
        public int? STR_CANTIDAD { get; set; }
        public double? STR_SUBTOTAL { get; set; }
        public string STR_FECHAREQ { get; set; }
        public Complemento STR_DIM1 { get; set; }
        public Complemento STR_DIM2 { get; set; }
        public Complemento STR_DIM3 { get; set; }
        public Complemento STR_DIM4 { get; set; }
        public Complemento STR_DIM5 { get; set; }
        public string STR_COMENTARIO { get; set; }
    }
}
