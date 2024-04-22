using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL.Response
{
    public class CreateResponse
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        //[JsonIgnore]
        public int AprobacionFinalizada { get; set; }
    }
}
