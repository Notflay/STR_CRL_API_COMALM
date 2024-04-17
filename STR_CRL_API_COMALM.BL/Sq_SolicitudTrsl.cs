using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.EL.Request;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_SolicitudTrsl
    {
        HanaADOHelper hash = new HanaADOHelper();

        // POST
        public ConsultationResponse<SolicitudTrsl> CrearSolicitud(SolicitudTrsl slctd)
        {
            // Creación de Solicitud de Traslado
            var respIncorrect = "No se pudo registrar Solicitud";
            //Sq_Item sq = new Sq_Item();
            List<SolicitudTrsl> list = new List<SolicitudTrsl>();

            try
            {
                // Inserta los valores en Solicitud Traslado, los valores puedes ser null ('?')
                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.post_solicitudTrsl), slctd.STR_USUARIO.id,
                    slctd.STR_FEC_REGISTRO,slctd.STR_FEC_REQUER,slctd.STR_FILIAL?.U_ST_Filial,slctd.STR_MONEDA?.id,
                    slctd.STR_ESTADO,slctd.STR_DOCNUM,slctd.STR_MENSAJE_MIG);

                // Obtiene ID de Solicitud
                string id = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_idSolicitudTrsl), slctd.STR_USUARIO.id);
                slctd.STR_ID = Convert.ToInt32(id);

                slctd.detalles.ForEach((s) =>
                {
                    hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.post_solicitudTrslDt), id, s.STR_ITEM?.ItemCode, s.STR_CANTIDAD, s.STR_SUBTOTAL
                        , s.STR_FECHAREQ, s.STR_DIM1?.id, s.STR_DIM2?.id, s.STR_DIM4?.id,s.STR_DIM5, s.STR_COMENTARIO);
                });

                // Actualiza el Total
                //hash.insertValueSql(SQ_QueryManager.Generar(Sq_query.upd_RDTotal), doc.STR_RD_ID, doc.STR_RD_ID);

                list.Add(slctd);
                    
                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<SolicitudTrsl>(ex);
            }
        }

        // GET
        /*
        public ConsultationResponse<SolicitudTrsl> ListarSolicitudesTrsl(string usrCreate, string usrAsig, int perfil, string fecIni, string fecFin, string nrRendi, string estado, string area)
        {
            var respIncorrect = "No trajo la lista de solicitudes de rendición";
            SolicitudTrsl sQ = new SolicitudTrsl();

            try
            {
                List<SolicitudTrsl> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaRendiciones), dc =>
                {
                    return new Rendicion()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_SOLICITUD = Convert.ToInt32(dc["STR_SOLICITUD"]),
                        STR_NRRENDICION = dc["STR_NRRENDICION"],
                        STR_NRAPERTURA = dc["STR_NRAPERTURA"],
                        STR_NRCARGA = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRCARGA"])) ? (int?)null : Convert.ToInt32(dc["STR_NRCARGA"]),
                        STR_ESTADO = Convert.ToInt32(dc["STR_ESTADO"]),
                        STR_ESTADO_INFO = sQ.ObtenerEstado(Convert.ToInt32(dc["STR_ESTADO"])).Result[0],//dc["STR_ESTADO_INFO"] ,
                        STR_EMPLDASIG = Convert.ToInt32(dc["STR_EMPLDASIG"]),
                        STR_EMPLDREGI = Convert.ToInt32(dc["STR_EMPLDREGI"]),
                        STR_TOTALRENDIDO = Convert.ToDouble(dc["STR_TOTALRENDIDO"]),
                        //STR_TOTALAPERTURA = Convert.ToDouble(dc["STR_TOTALAPERTURA"]),
                        STR_FECHAREGIS = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"]
                    };
                }, usrCreate, usrAsig, perfil.ToString(), fecIni, fecFin, nrRendi, estado, area).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Rendicion>(ex);
            }
        }
        */
        // PUT
    }
}
