using STR_CRL_API_COMALM.EL.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL.Request
{
    public class DocumentoDet
    {
		public Articulo STR_CODARTICULO { get; set; }

		public string STR_CONCEPTO { get; set; } 

		public string STR_ALMACEN { get; set; }

		public Complemento STR_PROYECTO { get; set; }

		public Complemento STR_DIM1 { get; set; } /*-----unidad de negocio*/

		public Complemento STR_DIM2 { get; set; }/* -----filial*/

		public Complemento STR_DIM4 { get; set; } /*-----area*/

		public Complemento STR_DIM5 { get; set; } /*-----centro de costo*/

		public Complemento STR_INDIC_IMPUESTO { get; set; }

		public decimal STR_PRECIO { get; set; } /*---crealo*/

		public int STR_CANTIDAD { get; set; }

		public int STR_IMPUESTO { get; set; } /*---crealo*/
	}
}
