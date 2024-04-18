﻿using STR_CRL_API_COMALM.EL.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL.Request
{
    public class DocumentoDet
    {
        public int ID { get; set; }
        public Articulo STR_CODARTICULO { get; set; }
        public double STR_SUBTOTAL { get; set; }
        public Complemento STR_INDIC_IMPUESTO { get; set; }
        public Complemento STR_DIM1 { get; set; }
        public Complemento STR_DIM2 { get; set; }
        public Complemento STR_DIM3 { get; set; }
        public Complemento STR_DIM4 { get; set; }
        public Complemento STR_DIM5 { get; set; }
        /*
       
        public Complemento STR_PROYECTO { get; set; }
        public CentroCosto STR_CENTCOSTO { get; set; }
        public Complemento STR_POS_FINANCIERA { get; set; }
        public Cup STR_CUP { get; set; }
        */
        public string STR_ALMACEN { get; set; }
        public int STR_CANTIDAD { get; set; }
        public string STR_TPO_OPERACION { get; set; }
        public int STR_DOC_ID { get; set; }

        //Nuevo
        public Cup STR_CUP { get; set; }

        public Complemento STR_PROYECTO { get; set; }
        public CentroCosto STR_CENTCOSTO { get; set; }
        public Complemento STR_POS_FINANCIERA { get; set; }
    }
}
