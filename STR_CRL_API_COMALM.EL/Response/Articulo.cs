using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL.Response
{
    public class Articulo
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string U_BPP_TIPUNMED { get; set; }
        public string WhsCode { get; set; }
        public double? Stock { get; set; }
        public double? Precio { get; set; }
    }
}
