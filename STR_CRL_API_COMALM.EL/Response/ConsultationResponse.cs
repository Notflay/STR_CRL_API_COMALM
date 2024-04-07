using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL
{
    public class ConsultationResponse<T>
    {
        public string CodRespuesta { get; set; }
        public string DescRespuesta { get; set; }
        public List<T> Result { get; set; }
    }
}
