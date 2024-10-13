using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL.Request
{
    public class SolicitudRd
    {
        public int? ID { get; set; }
        public Usuario STR_EMPLDREGI { get; set; }
        public int STR_EMPLDREGI_ID { get; set; }
        public string STR_EMPLDREGI_NOMBRE { get; set; }
        public Usuario STR_EMPLDASIG { get; set; }
        public int STR_EMPLDASIG_ID { get; set; }
        public string STR_EMPLDASIG_NOMBRE { get; set; }
        public int? STR_NRSOLICITUD { get; set; }
        public string STR_NRRENDICION { get; set; }
        public string STR_ESTADO_INFO { get; set; }
        public int? STR_ESTADO { get; set; }
        //public int? STR_EMPLDASIG { get; set; }
        public string STR_FECHAREGIS { get; set; }
        public Complemento STR_MONEDA { get; set; }
        public Complemento STR_TIPORENDICION { get; set; }
        public Complemento STR_MOTIVORENDICION { get; set; }
        public double STR_TOTALSOLICITADO { get; set; }
        public string STR_MOTIVOMIGR { get; set; }
        public string STR_AREA { get; set; }
        public int? STR_DOCENTRY { get; set; }
        public string CREATE { get; set; }
        public string STR_COMENTARIO { get; set; }
      
        /*
        public List<SolicitudRDdet> SOLICITUD_DET { get; set; }
        public Direccion STR_DIRECCION { get; set; }
        public Complemento STR_RUTA_INFO { get; set; }
        public Complemento STR_TIPORENDICION_INFO { get; set; }
        public Usuario STR_EMPLEADO_ASIGNADO { get; set; }
        */
    }
}
