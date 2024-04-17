using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STR_CRL_API_COMALM.EL.Response;
using STR_CRL_API_COMALM.SL;
using STR_CRL_API_COMALM.SQ;

namespace STR_CRL_API_COMALM.EL.Request
{
    public class SolicitudTrsl
    {
        public int STR_ID { get; set; } 
        public string STR_FEC_REGISTRO { get; set; }
        public string STR_FEC_REQUER { get; set; }
        public Complemento STR_MONEDA { get; set; }
        public Complemento STR_USUARIO { get; set; }
        public Filial STR_FILIAL { get; set; }
        public int STR_ESTADO { get; set; }
        public int? STR_DOCNUM { get; set; }
        public string STR_MENSAJE_MIG { get; set; }
        public List<SolicitudTrslDet> detalles { get; set; }
    }
}
