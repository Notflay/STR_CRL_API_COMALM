﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL
{
    public class SolicitudRDSerializer
    {
        public int Series { get; set; }
        public string RequriedDate { get; set; }
        public string RequesterEmail { get; set; }
        public string U_STR_TIPOEAR { get; set; }
        public string U_CE_MNDA { get; set; }
        public int U_STR_WEB_COD { get; set; }
        public string Requester { get; set; }
        public string RequesterName { get; set; }
        public int RequesterBranch { get; set; }
        public int RequesterDepartment { get; set; }
        public int ReqType { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string Comments { get; set; }
        public string DocCurrency { get; set; }
        public double DocRate { get; set; }
        public string TaxDate { get; set; }
       // public string DocDate { get; set; }
        public string DocType { get; set; }
        public string Printed { get; set; }
        public string AuthorizationStatus { get; set; }
        public string TaxCode { get; set; }
        public string TaxLiable { get; set; }
        public string JournalMemo { get; set; }
        public string U_STR_WEB_AUTPRI { get; set; }
        public string U_STR_WEB_AUTSEG { get; set; }
        public string U_STR_WEB_ORDV { get; set; }
        public string U_STR_WEB_PRIID { get; set; }
        public string U_STR_WEB_SEGID { get; set; }
        public string U_STR_WEB_EMPASIG { get; set; }
        public double U_CE_TOTINCIMP { get; set; }
        public string U_ELE_Tipo_ER { get; set; }
        //public string U_CE_MNDA { get; set; }
        public string U_ST_NroRQWeb { get; set; }
        public List<DetalleSerializar> DocumentLines { get; set; }
    }
}
