using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.EL.Request;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_SolicitudRd
    {
        string portal_ear = null;

        public Sq_SolicitudRd() {
            portal_ear = ConfigurationManager.AppSettings["portal_ear"];
        }
        HanaADOHelper hash = new HanaADOHelper();
        public ConsultationResponse<SolicitudRd> CreaSolicitudRd(SolicitudRd solicitudRD)
        {
            var respIncorrect = "Solicitud de Detalle";
            string id = string.Empty;
            List<SolicitudRd> list = new List<SolicitudRd>();
            
            try
            {
                // Inserta en la tabla

                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.post_solicitudEar), solicitudRD.STR_EMPLDREGI.sapID, 
                    solicitudRD.STR_NRSOLICITUD, solicitudRD.STR_NRRENDICION, solicitudRD.STR_ESTADO,solicitudRD.STR_EMPLDASIG.sapID,
                    solicitudRD.STR_FECHAREGIS, solicitudRD.STR_MONEDA?.id, solicitudRD.STR_MOTIVORENDICION?.id,solicitudRD.STR_TIPORENDICION?.id, solicitudRD.STR_COMENTARIO,
                    solicitudRD.STR_TOTALSOLICITADO, solicitudRD.STR_MOTIVOMIGR, solicitudRD.STR_DOCENTRY, solicitudRD.STR_AREA);

                id = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_solicitudEarId), solicitudRD.STR_EMPLDASIG.sapID.ToString());


                solicitudRD.ID = Convert.ToInt32(id);

                list.Add(solicitudRD);

                return Global.ReturnOk(list, respIncorrect);
            }

            catch (Exception ex)
            {
                return Global.ReturnError<SolicitudRd>(ex);
            }
        }
        
        public ConsultationResponse<SolicitudRd> ActualizarSolicitudSr(SolicitudRd solicitudRD)
        {
            var respIncorrect = "No se completo la actualización de Sr";
            string id = string.Empty;
            List<SolicitudRd> list = new List<SolicitudRd>();

            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_solicitudEar), solicitudRD.STR_EMPLDREGI,
                    solicitudRD.STR_NRSOLICITUD, solicitudRD.STR_NRRENDICION, solicitudRD.STR_ESTADO, solicitudRD.STR_EMPLDASIG,
                    solicitudRD.STR_FECHAREGIS, solicitudRD.STR_MONEDA?.id, solicitudRD.STR_TIPORENDICION?.id, solicitudRD.STR_MOTIVORENDICION?.id, solicitudRD.STR_COMENTARIO,
                    solicitudRD.STR_TOTALSOLICITADO,solicitudRD.STR_MOTIVOMIGR, solicitudRD.STR_DOCENTRY, solicitudRD.STR_AREA, solicitudRD.ID);

                list.Add(solicitudRD);
 
        
                return Global.ReturnOk(list, respIncorrect);

            }
            catch (Exception ex)
            {
                return Global.ReturnError<SolicitudRd>(ex);
            }
        }
        
        public ConsultationResponse<SolicitudRd> ObtenerSolicitud(int id, string create, bool masDetalle = true)
        {
            var respIncorrect = "No trajo la la solicitud de rendición";
            Sq_Viatico sq_Viatico = new Sq_Viatico();
            Sq_Usuario sq_Usuario = new Sq_Usuario();
            
            try
            {
                List<SolicitudRd> list = hash.GetResultAsType(SQ_QueryManager.Generar(create == "PWB" ? Sq_Query.get_solicitudEar : Sq_Query.get_solicitudEar), dc =>
                {
                    return new SolicitudRd()
                    {
                        ID = Convert.ToInt32(dc["IdSolicitud"]),
                        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["STR_NRSOLICITUD"]),
                        STR_NRRENDICION = dc["STR_NRRENDICION"],
                        STR_ESTADO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_ESTADO"])) ? (int?)null : Convert.ToInt32(dc["STR_ESTADO"]),
                        STR_ESTADO_INFO = dc["STR_ESTADO_INFO"],
                       STR_MONEDA = string.IsNullOrEmpty(dc["STR_MONEDA"]) ? null : new Complemento { id = dc["STR_MONEDA"], name = dc["STR_MONEDA"] },
                        STR_TIPORENDICION = string.IsNullOrEmpty(dc["STR_TIPORENDICION"]) ? null : sq_Viatico.ObtieneViatico(dc["STR_TIPORENDICION"]),
                        STR_MOTIVORENDICION = string.IsNullOrEmpty(dc["STR_MOTIVORENDICIOON"]) ? null : sq_Viatico.ObtieneTpViatico(dc["STR_MOTIVORENDICIOON"]),
                        STR_TOTALSOLICITADO = Convert.ToDouble(dc["STR_TOTALSOLICITADO"]),
                       STR_COMENTARIO = dc["STR_COMENTARIO"],            
                       STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"],
                       STR_EMPLDASIG = sq_Usuario.getUsuarioId(portal_ear, dc["STR_EMPLDASIG"]),
                        STR_EMPLDREGI = sq_Usuario.getUsuarioId(portal_ear, dc["STR_EMPLDREGI"]),
                         STR_FECHAREGIS = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy"),           
                         STR_AREA = dc["STR_AREA"],
                    };
                }, id.ToString()).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<SolicitudRd>(ex);
            }
        }

        public ConsultationResponse<SolicitudRd> ListarSolicutudes(string usrCreate, string usrAsign, int perfil, string fecini, string fecfin, string nrrendi, string estados, string area)
        {
            var respIncorrect = "No trajo la lista solicitud de rendición";
            Sq_Usuario sq_Usuario = new Sq_Usuario();
            Sq_Viatico sq_Viatico = new Sq_Viatico();
            try
            {

                List<SolicitudRd> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_lstSolicitudEar), dc =>
                {
                    return new SolicitudRd()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                           STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["STR_NRSOLICITUD"]),
                        STR_NRRENDICION = dc["STR_NRRENDICION"],
                        STR_ESTADO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_ESTADO"])) ? (int?)null : Convert.ToInt32(dc["STR_ESTADO"]),
                          STR_ESTADO_INFO = dc["STR_ESTADO_INFO"],
                          STR_MONEDA = string.IsNullOrEmpty(dc["STR_MONEDA"]) ? null : new Complemento { id = dc["STR_MONEDA"], name = dc["STR_MONEDA"] },
                         STR_TIPORENDICION = string.IsNullOrEmpty(dc["STR_TIPORENDICION"]) ? null : sq_Viatico.ObtieneViatico(dc["STR_TIPORENDICION"]),
                        STR_MOTIVORENDICION = string.IsNullOrEmpty(dc["STR_MOTIVORENDICIOON"]) ? null : sq_Viatico.ObtieneTpViatico(dc["STR_MOTIVORENDICIOON"]),
                           STR_TOTALSOLICITADO = Convert.ToDouble(dc["STR_TOTALSOLICITADO"]),
                       STR_COMENTARIO = dc["STR_COMENTARIO"],
                         STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"],
                         STR_EMPLDASIG_ID = sq_Usuario.getUsuarioId(portal_ear, dc["STR_EMPLDASIG"]).sapID,
                         STR_EMPLDREGI_ID = sq_Usuario.getUsuarioId(portal_ear, dc["STR_EMPLDREGI"]).sapID,
                       STR_FECHAREGIS = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                         STR_AREA = dc["STR_AREA"],
                         CREATE = dc["CREATE"]
                    };
                },usrCreate,  usrAsign, perfil.ToString(), fecini, fecfin, nrrendi, estados, area).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<SolicitudRd>(ex);
            }
        }

        /*
        public ConsultationResponse<SolicitudRd> ObtenerSolicitud(int id, string create, bool masDetalle = true)
        {
            var respIncorrect = "No trajo la la solicitud de rendición";

            try
            {

                SQ_Ubicacion sQ_Ubicacion = new SQ_Ubicacion();
                Sq_Item sq_item = new Sq_Item();
                List<SolicitudRDdet> listDet = null;
                if (masDetalle)
                {


                    listDet = hash.GetResultAsType(SQ_QueryManager.Generar(create == "PWB" ? SQ_Query.get_solicitudRendicionDet : SQ_Query.get_solicitudRendicionDetSAP), dc =>
                    {
                        /*
                        List<Complemento> listDetCentC = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_centrodeCostoPorItem), sc =>
                        {
                            return new Complemento()
                            {
                                id = sc["ID"],
                                name = sc["STR_CENTCOSTO"],
                                Descripcion = sc["STR_DET_ID"]
                            };
                        }, dc["ID"]).ToList();
                        */
        /*
        List<Cup> listaCUP = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtieneCup), dc =>
        {
            return new Cup()
            {
                CRP = Convert.ToInt32(dc["CRP"]),
                Partida = Convert.ToInt32(dc["Partida"]),
                U_CUP = dc["U_CUP"],
                U_DESCRIPTION = dc["U_DESCRIPTION"]
            };
        }, dc["STR_CUP"]).ToList();
        */
        /*
        return new SolicitudRDdet()
        {
            id = Convert.ToInt32(dc["ID"]),
            ID = Convert.ToInt32(dc["ID"]),
            STR_CANTIDAD = Convert.ToInt32(dc["STR_CANTIDAD"]),
            cantidad = Convert.ToInt32(dc["STR_CANTIDAD"]),
            STR_CONCEPTO = dc["STR_CONCEPTO"],
            STR_CODARTICULO = dc["STR_CODARTICULO"],
            STR_CUP = dc["STR_CUP"],
            STR_POSFINAN = dc["STR_POSFINAN"],
            STR_TOTAL = Convert.ToDouble(dc["STR_TOTAL"]),
            SR_ID = Convert.ToInt32(dc["SR_ID"]),
            articulo = sq_item.ObtenerItem(dc["STR_CODARTICULO"]).Result[0],
            ceco = dc["STR_CECO"],
            cup = string.IsNullOrEmpty(dc["STR_CUP"]) ? null : obtenerCup(dc["STR_CUP"]),
            posFinanciera = new Complemento { id = dc["STR_POSFINAN"], name = dc["STR_POSFINAN"] },
            precioUnitario = Convert.ToDouble(dc["STR_TOTAL"]) / Convert.ToDouble(dc["STR_CANTIDAD"]),
            precioTotal = Convert.ToDouble(dc["STR_TOTAL"]),
            ctc = dc["STR_CTC"]

        };
    }, id.ToString()).ToList(); // DocEntry
}



// Obtiene Ruta
SQ_Complemento sQ_Complemento = new SQ_Complemento();
int campo = 0;
campo = ObtieneCampoTipoRuta();

// Obitiene EAR
int campEar = 0;
campEar = ObtieneCampoTipoEar();

// Obtiene usuario
SQ_Usuario sQ_Usuario = new SQ_Usuario();

List<SolicitudRD> list = hash.GetResultAsType(SQ_QueryManager.Generar(create == "PWB" ? SQ_Query.get_solicitudRendicion : SQ_Query.get_solicitudRendicionSAP), dc =>
{
    return new SolicitudRD()
    {
        ID = Convert.ToInt32(dc["ID"]),
        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
        STR_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["STR_NRSOLICITUD"]),
        STR_NRRENDICION = dc["STR_NRRENDICION"],
        STR_ESTADO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_ESTADO"])) ? (int?)null : Convert.ToInt32(dc["STR_ESTADO"]),
        STR_MOTIVO = dc["STR_MOTIVO"],
        STR_UBIGEO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_UBIGEO"])) ? (int?)null : Convert.ToInt32(dc["STR_UBIGEO"]),
        STR_RUTA = dc["STR_RUTA"],
        STR_RUTA_INFO = masDetalle ? (string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_RUTA"])) ? null : sQ_Complemento.ObtenerDesplegablePorId(campo.ToString(), dc["STR_RUTA"]).Result[0]) : null,
        STR_DIRECCION = masDetalle ? (string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_UBIGEO"])) ? null : sQ_Ubicacion.ObtenerDireccion(dc["STR_UBIGEO"]).Result[0]) : null,
        STR_RUTAANEXO = dc["STR_RUTAANEXO"],
        STR_MONEDA = dc["STR_MONEDA"],
        STR_TIPORENDICION = dc["STR_TIPORENDICION"],
        STR_TIPORENDICION_INFO = masDetalle ? (string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_TIPORENDICION"])) ? null : sQ_Complemento.ObtenerDesplegablePorId(campEar.ToString(), dc["STR_TIPORENDICION"]).Result[0]) : null,
        STR_TOTALSOLICITADO = Convert.ToDouble(dc["STR_TOTALSOLICITADO"]),
        STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"],
        STR_EMPLDASIG = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_EMPLDASIG"])) ? (int?)null : Convert.ToInt32(dc["STR_EMPLDASIG"]),
        STR_EMPLDREGI = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_EMPLDREGI"])) ? (int?)null : Convert.ToInt32(dc["STR_EMPLDREGI"]),
        STR_FECHAREGIS = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy"),
        STR_FECHAINI = string.IsNullOrWhiteSpace(dc["STR_FECHAINI"]) ? "" : Convert.ToDateTime(dc["STR_FECHAINI"]).ToString("dd/MM/yyyy"),
        STR_FECHAFIN = string.IsNullOrWhiteSpace(dc["STR_FECHAFIN"]) ? "" : Convert.ToDateTime(dc["STR_FECHAFIN"]).ToString("dd/MM/yyyy"),
        STR_FECHAVENC = string.IsNullOrWhiteSpace(dc["STR_FECHAVENC"]) ? "" : Convert.ToDateTime(dc["STR_FECHAVENC"]).ToString("dd/MM/yyyy"),
        STR_EMPLEADO_ASIGNADO = masDetalle ? sQ_Usuario.getUsuario(Convert.ToInt32(dc["STR_EMPLDASIG"])).Result[0] : null,
        STR_ORDENVIAJE = dc["STR_ORDENVIAJE"],
        STR_AREA = dc["STR_AREA"],
        SOLICITUD_DET = masDetalle ? listDet : null,
    };
}, id.ToString()).ToList();

return Global.ReturnOk(list, respIncorrect);
}
catch (Exception ex)
{
return Global.ReturnError<SolicitudRd>(ex);
}*/
    }

}
