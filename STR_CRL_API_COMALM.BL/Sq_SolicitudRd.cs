using Newtonsoft.Json;
using RestSharp;
using STR_CRL_API_COMALM.BL.Email;
using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.EL.Request;
using STR_CRL_API_COMALM.SL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_solicitudEar), solicitudRD.STR_EMPLDREGI.sapID,
                    solicitudRD.STR_NRSOLICITUD, solicitudRD.STR_NRRENDICION, solicitudRD.STR_ESTADO, solicitudRD.STR_EMPLDASIG.sapID
                   , solicitudRD.STR_MONEDA?.id, solicitudRD.STR_MOTIVORENDICION?.id, solicitudRD.STR_TIPORENDICION?.id, solicitudRD.STR_COMENTARIO,
                    solicitudRD.STR_TOTALSOLICITADO,solicitudRD.STR_MOTIVOMIGR, solicitudRD.STR_DOCENTRY, solicitudRD.ID);

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
                var test = ("SELECT T2.\"DESCRIPCION\" AS \"STR_ESTADO_INFO\",T1.\"ID\" AS \"IdSolicitud\", * FROM \"STR_WEB_SR\" T1 INNER JOIN \"STR_WEB_ESTADOS\" T2 ON T2.\"ID\" = T1.\"STR_ESTADO\" WHERE T1.ID =",
                    2);
                List <SolicitudRd> list = hash.GetResultAsType(SQ_QueryManager.Generar(create == "PWB" ? Sq_Query.get_solicitudEar : Sq_Query.get_solicitudEar), dc =>
                {
                    return new SolicitudRd()
                    {
                        ID = Convert.ToInt32(dc["IdSolicitud"]),
                        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["STR_NRSOLICITUD"]),
                        STR_NRRENDICION = dc["STR_NRRENDICION"],
                        STR_ESTADO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_ESTADO"])) ? (int?)null : Convert.ToInt32(dc["STR_ESTADO"]),
                        STR_ESTADO_INFO = dc["STR_ESTADO_INFO"],
                        STR_EMPLDASIG_ID = Convert.ToInt32(dc["STR_EMPLDASIG"]),
                        STR_MONEDA = string.IsNullOrEmpty(dc["STR_MONEDA"]) ? null : new Complemento { id = dc["STR_MONEDA"], name = dc["STR_MONEDA"] },
                        STR_TIPORENDICION = string.IsNullOrEmpty(dc["STR_MOTIVORENDICIOON"]) ? null : sq_Viatico.ObtieneTpViatico(dc["STR_MOTIVORENDICIOON"]),
                        STR_MOTIVORENDICION = string.IsNullOrEmpty(dc["STR_TIPORENDICION"]) ? null : sq_Viatico.ObtieneViatico(dc["STR_TIPORENDICION"]),
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
        public List<Aprobador> ObtieneListaAprobadores(string tipoUsuario, string idSolicitud, string estado)
        {
            List<Aprobador> listaAprobadores = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_infoAprobadores), dc =>
            {
                return new Aprobador
                {
                    idSolicitud = Convert.ToInt32(dc["ID_SR"]),
                    aprobadorId = Convert.ToInt32(dc["Aprobador Id"]),
                    aprobadorNombre = dc["Nombre Autorizador"],
                    emailAprobador = dc["Email Aprobador"],
                    finalizado = Convert.ToInt32(dc["Finalizado"]),
                    empleadoId = Convert.ToInt32(dc["Empleado Id"]),
                    nombreEmpleado = dc["Nombre Empleado"],
                    area = string.IsNullOrWhiteSpace(Convert.ToString(dc["Area"])) ? (int?)null : Convert.ToInt32(dc["Area"]),
                    fechaRegistro = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? null : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy")
                };
            }, tipoUsuario, idSolicitud, estado).ToList();

            return listaAprobadores;
        }

        public ConsultationResponse<AprobadorResponse> EnviarSolicitudAprobacion(string idSolicitud, int usuarioId, string tipord, string area, double monto, int estado, List<P_borrador> borradores)
        {
            // Obtiene Aprobadores
            List<AprobadorResponse> response = new List<AprobadorResponse>();
            string valor = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_aprobadores), tipord, area, monto.ToString("F2"));
            List<Aprobador> aprobadors;
            try
            {
                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");
                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(3).Where(aprobador => aprobador != "-1").ToList();

                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.post_insertAprobadores), idSolicitud, usuarioId.ToString(), null, null, 0, estado == 1 ? aprobadores[0] : aprobadores[1]);

                if (estado == 1) hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoSR), "2", "", idSolicitud);                                        // Actualiza el estado

                aprobadors = new List<Aprobador>();
                aprobadors = ObtieneListaAprobadores(estado == 3 ? "3" : "2", idSolicitud, "0"); // Autorizadores, solicitud, estado

                if (aprobadors.Count < 1)
                    throw new Exception("No se encontró Aprobadores");

                aprobadors.ForEach(a =>
                {
                    EnviarEmail envio = new EnviarEmail();

                    if (!string.IsNullOrWhiteSpace(a.emailAprobador))
                    {
                        envio.EnviarConfirmacion(a.emailAprobador,
                       a.aprobadorNombre, a.nombreEmpleado, true, a.idSolicitud.ToString(), "", a.fechaRegistro, a.estado.ToString(), a.area.ToString(), a.aprobadorId.ToString());
                    }
                });

                response = new List<AprobadorResponse> { new AprobadorResponse() {
              aprobadores = aprobadores.Count()
          } };

                return Global.ReturnOk(response, "Ok");
            }
            catch (Exception ex)
            {

                return Global.ReturnError<AprobadorResponse>(ex);
            }
        }

        public ConsultationResponse<CreateResponse> AceptarSolicitud(int solicitudId, string aprobadorId, string areaAprobador, int estado)
        {
            List<CreateResponse> lista = new List<CreateResponse>();
            List<Aprobador> listaAprobados = null;
            SolicitudRd solicitudRD = new SolicitudRd();
            // Dependiendo de si ya es la segunda Aceptación de la solicitud o si solo tiene una migraría a
            // 3: "En Autorizacion SR"  o directamente 4: Autorizado SR
            try
            {
                solicitudRD = ObtenerSolicitud(solicitudId, "PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_aprobadores), solicitudRD.STR_MOTIVORENDICION.id, solicitudRD.STR_AREA, solicitudRD.STR_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");
                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(3).Where(aprobador => aprobador != "-1").ToList();
                bool existeAprobador = aprobadores.Any(dat => dat.Equals(areaAprobador));

                // Valide que se encuentre en la lista de aprobadores pendientes        
                listaAprobados = new List<Aprobador>();
                listaAprobados = ObtieneListaAprobadores(estado == 3 ? "3" : "2", solicitudId.ToString(), "0");
                existeAprobador = listaAprobados.Any(dat => dat.aprobadorId == Convert.ToInt32(aprobadorId));

                if (existeAprobador)
                {
                    if (aprobadores.Count == 1 | solicitudRD.STR_ESTADO == 3)
                    {
                        EnviarEmail envio = new EnviarEmail();

                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, solicitudId, 0);

                        var response = GeneraSolicitudRDenSAP(solicitudRD);

                        if (response.IsSuccessful)
                        {
                            CreateResponse createResponse = JsonConvert.DeserializeObject<CreateResponse>(response.Content);
                            createResponse.AprobacionFinalizada = 1;

                            // Inserts despues de crear la SR en SAP 
                            hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoSR), "6", "", solicitudId);                                       // Actualiza Estado
                            //string codigoRendicion = hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.get_numeroRendicion),solicitudRD.STR_EMPLDASIG.codEar);   // Obtiene el número de Rendición con el DocEntry
                            hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarMigradaSR), createResponse.DocEntry, createResponse.DocNum, "", solicitudId);   // Actualiza en la tabla, DocEnty DocNum y Numero de Rendicón                                                                                                                                                                                // Quita de activos en la tabla de pendientes de Borrador

                            lista.Add(createResponse);

                            // Envio de Correo


                            envio.EnviarInformativo(solicitudRD.STR_EMPLDASIG.email, FormatearNombreCompleto(solicitudRD.STR_EMPLDASIG.nombres), true, solicitudRD.ID.ToString(), "", solicitudRD.STR_FECHAREGIS, true, "");
                            return Global.ReturnOk(lista, "");
                        }
                        else
                        {
                            string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                            hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoSR), "7", mensaje.Replace("'", ""), solicitudId);
                            envio.EnviarError(true, null, solicitudRD.ID.ToString(), solicitudRD.STR_FECHAREGIS, mensaje.Replace("'", ""));
                            throw new Exception(mensaje);
                        }
                    }
                    else
                    {
                        CreateResponse createresponse = new CreateResponse()
                        {
                            AprobacionFinalizada = 0
                        };
                        //createresponse.aprobacionfinalizada = 0;
                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, solicitudId, 0);
                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoSR), 3, null, solicitudId);

                        lista.Add(createresponse);

                        // Envia la solicitud al siguiente aprobador
                        EnviarSolicitudAprobacion(solicitudId.ToString(), solicitudRD.STR_EMPLDASIG_ID, solicitudRD.STR_MOTIVORENDICION.id, solicitudRD.STR_AREA, solicitudRD.STR_TOTALSOLICITADO, 3, null);

                        return Global.ReturnOk(lista, "");
                    }
                }
                else
                {
                    throw new Exception("No se encontraron solicitudes pendientes");
                }
            }
            catch (Exception ex)
            {

                return Global.ReturnError<CreateResponse>(ex);
            }
        }

        public ConsultationResponse<string> RechazarSolicitud(string solicitudId, string aprobadorId, string comentarios, string areaAprobador)
        {
            string nuevoEstado = "5";
            // Si una solicitud es Rechazada volverá a ser editable
            // 5: "Rechazado SR"
            List<Aprobador> listaAprobados = null;
            SolicitudRd solicitudRD = new SolicitudRd();
            Sq_Usuario sQ_Usuario = new Sq_Usuario();
            Usuario usuario = null;

            try
            {
                solicitudRD = ObtenerSolicitud(Convert.ToInt32(solicitudId), "PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_aprobadores), solicitudRD.STR_TIPORENDICION.id, solicitudRD.STR_AREA, solicitudRD.STR_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");
                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(2).Where(aprobador => aprobador != "-1").ToList();
                bool existeAprobador = aprobadores.Any(dat => dat.Equals(areaAprobador));

                // Valide que se encuentre en la lista de aprobadores pendientes
                listaAprobados = new List<Aprobador>();
                listaAprobados = ObtieneListaAprobadores("2", solicitudId.ToString(), "0");
                existeAprobador = listaAprobados.Any(dat => dat.aprobadorId == Convert.ToInt32(aprobadorId));

                if (existeAprobador)
                {
                    List<string> lista = new List<string>() {
                    "Rechazado con exito"
                    };

                    EnviarEmail envio = new EnviarEmail();

                    usuario = new Usuario();
                    usuario = sQ_Usuario.getUsuario("1",listaAprobados[0].empleadoId.ToString());

                    if (usuario.email == null | usuario.email == "")
                    {
                        throw new Exception("No se encontró correo del empleado");
                    }
                    envio.EnviarInformativo(usuario.email, usuario.nombres, true, listaAprobados[0].idSolicitud.ToString(),
                        "", listaAprobados[0].fechaRegistro, false, comentarios);

                    hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.dlt_aprobadoresSr), solicitudId);
                    hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoSR), nuevoEstado, "", solicitudId);

                    return Global.ReturnOk(lista, "No se rechazo correctamente");
                }
                else
                {
                    throw new Exception("No se encontró solicitud a rechazar");
                }
            }
            catch (Exception ex)
            {
                return Global.ReturnError<string>(ex);
            }
        }
        public ConsultationResponse<Complemento> ValidacionSolicitud(int id)
        {
            return null;
            /*
            List<Complemento> list = new List<Complemento>();
            SolicitudRd solicitud = new SolicitudRd();
            List<string> CamposVacios = new List<string>();
           // PresupuestoRq preRq = new PresupuestoRq();
           // S_qCom sQ_Complemento = new SQ_Complemento();
            var respIncorrect = string.Empty;

            try
            {
                solicitud = ObtenerSolicitud(id, "PWB", true).Result[0];

                if (solicitud.STR_UBIGEO == null) CamposVacios.Add("Dirección");
                if (string.IsNullOrEmpty(solicitud.STR_RUTA)) CamposVacios.Add("Ruta");
                if (string.IsNullOrEmpty(solicitud.STR_RUTAANEXO)) CamposVacios.Add("Adjuntos");
                if (string.IsNullOrEmpty(solicitud.STR_FECHAFIN)) CamposVacios.Add("Fecha Fin");
                if (string.IsNullOrEmpty(solicitud.STR_FECHAINI)) CamposVacios.Add("Fecha de Inicio");
                if (solicitud.STR_TIPORENDICION != "ORV") if (string.IsNullOrEmpty(solicitud.STR_FECHAVENC)) CamposVacios.Add("Fecha de Vencimiento");
                if (string.IsNullOrEmpty(solicitud.STR_MONEDA)) CamposVacios.Add("Moneda");
                if (string.IsNullOrEmpty(solicitud.STR_TIPORENDICION)) CamposVacios.Add("Tipo de Rendición");

                // Valida Cent Costo



                if (solicitud.STR_TIPORENDICION != "ORV")
                {
                    if (solicitud.SOLICITUD_DET.Count == 0)
                    {
                        CamposVacios.Add("Lineas de Detalle");
                    }
                    else
                    {
                        solicitud.SOLICITUD_DET.ForEach((e) =>
                        {
                            if (e.articulo == null) { CamposVacios.Add("Nivel detalle"); return; }
                            if (e.STR_CANTIDAD == 0) { CamposVacios.Add("Nivel detalle"); return; }
                            // if (e.centCostos.Count == 0) { CamposVacios.Add("Nivel detalle"); return; }
                            if (e.ceco == null) { CamposVacios.Add("Nivel detlale"); return; }
                            if (e.cup == null) { CamposVacios.Add("Nivel detalle"); return; }
                            if (e.posFinanciera == null) { CamposVacios.Add("Nivel detalle"); return; }

                            bool exisCeco = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_validaCECO), e.ceco) != "0";

                            if (!exisCeco) { CamposVacios.Add("Centro de Costo no existe"); return; }

                        });

                        if (!CamposVacios.Any((text) => text == "Nivel detalle"))
                        {
                            SolicitudRDdet det = solicitud.SOLICITUD_DET[0];

                            // Valida presupuesto solo si tiene contenido a nivel detalle - si no termine 
                            preRq = new PresupuestoRq()
                            {
                                //centCostos = det.centCostos[0]?.name,
                                centCostos = det.ceco,
                                posFinanciera = det.posFinanciera.name,
                                anio = DateTime.Now.Year.ToString(),
                                precio = -(decimal)solicitud.STR_TOTALSOLICITADO,
                            };
                            var lestPrep = sQ_Complemento.ObtienePresupuesto(preRq).Result;

                            if (lestPrep.Count == 0)
                            {
                                throw new Exception("No se tiene presupuesto");
                            }
                            else
                            {
                                var s = lestPrep[0];

                                if (s.name == "-1")
                                {
                                    throw new Exception("No hay presupuesto suficiente");
                                }
                            }
                        }
                    }
                };
                if (CamposVacios.Count > 0)
                {
                    string CamposErroneos = string.Join(", ", CamposVacios);
                    respIncorrect += $" Faltan completar campos de " + CamposErroneos;
                    throw new Exception(respIncorrect);
                };

                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    name = "Se valido exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
            */
        }

        public ConsultationResponse<CreateResponse> ReintentarSolicitud(int solicitudId)
        {
            List<CreateResponse> lista = new List<CreateResponse>();
            SolicitudRd solicitudRD = new SolicitudRd();
            List<Aprobador> listaAprobados = null;
            string nuevoEstado = "0"; // Depende si va una solicitud o va ultima
                                      // Dependiendo de si ya es la segunda Aceptación de la solicitud o si solo tiene una migraría a
                                      // 3: "En Autorizacion SR"  o directamente 4: Autorizado SR

            try
            {
                // Envio de Correo
                EnviarEmail envio = new EnviarEmail();

                solicitudRD = ObtenerSolicitud(solicitudId, "PWB").Result[0];

                var response = GeneraSolicitudRDenSAP(solicitudRD);

                if (response.IsSuccessful)
                {

                    CreateResponse createResponse = JsonConvert.DeserializeObject<CreateResponse>(response.Content);
                    createResponse.AprobacionFinalizada = 1;
                    nuevoEstado = "6";

                    // Inserts despues de crear la SR en SAP 
                    hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoSR), nuevoEstado, "", solicitudId);                                       // Actualiza Estado
                                                                                                                                                                    //hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_intermedia), createResponse.DocEntry);                                             // Inserta en la tabla intemedia de EAR para generar codigo
                    string codigoRendicion = hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.get_numeroRendicion), solicitudRD.STR_EMPLDASIG.codEar);   // Obtiene el número de Rendición con el DocEntry
                    hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarMigradaSR), createResponse.DocEntry, createResponse.DocNum, codigoRendicion, solicitudId);   // Actualiza en la tabla, DocEnty DocNum y Numero de Rendicón

                    lista.Add(createResponse);



                    envio.EnviarInformativo(solicitudRD.STR_EMPLDASIG.email, FormatearNombreCompleto(solicitudRD.STR_EMPLDASIG.nombres), true, solicitudRD.ID.ToString(), "Número de Rendición: " + codigoRendicion, solicitudRD.STR_FECHAREGIS, true, "");
                    return Global.ReturnOk(lista, "");
                }
                else
                {
                    nuevoEstado = "7";
                    string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                    hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoSR), nuevoEstado, mensaje.Replace("'", ""), solicitudId);
                    //envio.EnviarError(true, null, solicitudRD.ID.ToString(), solicitudRD.STR_FECHAREGIS);
                    throw new Exception(mensaje);
                }
            }
            catch (Exception ex)
            {
                return Global.ReturnError<CreateResponse>(ex);

            }
        }
        static string FormatearNombreCompleto(string nombreCompleto)
        {
            string[] partes = nombreCompleto.Split(' ');

            if (partes.Length >= 2)
            {
                for (int i = 0; i < partes.Length; i++)
                {
                    partes[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(partes[i].ToLower());
                }

                return string.Join(" ", partes);
            }

            return nombreCompleto;
        }
        public List<Aprobador> obtieneAprobadoresDet(string idArea, string aprobadorId, string fecha)
        {
            List<Aprobador> aprobadors = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_listaAprobadoresDet), dc =>
            {
                return new Aprobador()
                {
                    aprobadorId = Convert.ToInt32(dc["empID"]),
                    aprobadorNombre = dc["lastName"],
                    finalizado = dc["empID"] == aprobadorId ? 1 : 0,
                    fechaRegistro = dc["empID"] == aprobadorId ? fecha : null
                };
            }, idArea).ToList();
            return aprobadors;
        }
        public ConsultationResponse<Aprobador> ObtieneAprobadores(string idSolicitud)
        {
            try
            {
                List<Aprobador> aprobadors = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_listaAprobadoresCab), dc =>
                {
                    return new Aprobador()
                    {
                        aprobadorNombre = dc["Nombres"],
                        aprobadorId = string.IsNullOrEmpty(dc["STR_USUARIOAPROBADORID"]) ? 0 : Convert.ToInt32(dc["STR_USUARIOAPROBADORID"]),
                        finalizado = Convert.ToInt32(dc["APROBACIONFINALIZADA"]),
                        area = Convert.ToInt32(dc["STR_AREA"]),
                        fechaRegistro = string.IsNullOrWhiteSpace(dc["FECHAAPROBACION"]) ? "" : DateTime.Parse(dc["FECHAAPROBACION"]).ToString("dd/MM/yyyy"),
                        aprobadores = obtieneAprobadoresDet(dc["STR_AREA"], dc["STR_USUARIOAPROBADORID"], string.IsNullOrWhiteSpace(dc["FECHAAPROBACION"]) ? "" : DateTime.Parse(dc["FECHAAPROBACION"]).ToString("dd/MM/yyyy"))
                    };
                }, idSolicitud).ToList();


                return Global.ReturnOk(aprobadors, "");

            }
            catch (Exception ex)
            {
                return Global.ReturnError<Aprobador>(ex);
            }
        }

        public int ObtenerSerieOPRQ()
        {
            try
            {
                string serie = ConfigurationManager.AppSettings["SerieCodeEAR"];
                string anio = DateTime.Now.Year.ToString();
                int codeEAR = 0;

                codeEAR = Convert.ToInt32(hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_SerieOPRQ), anio, serie));

                return codeEAR;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IRestResponse GeneraSolicitudRDenSAP(SolicitudRd sr)
        {
            Sq_Usuario sQ_Usuario = new Sq_Usuario();
            List<DetalleSerializar> detalleSerializars = new List<DetalleSerializar>();

            //int series = Convert.ToInt32(ConfigurationManager.AppSettings["SerieCodeEAR"]);


            Usuario usuario = sQ_Usuario.getUsuarioId("1",sr.STR_EMPLDASIG_ID.ToString());
            //AttatchmentSerializer adj = ObtenerAdjuntos(sr.STR_RUTAANEXO);
            List<Aprobador> aprobadores = new List<Aprobador>();
            aprobadores = ObtieneAprobadores(sr.ID.ToString()).Result;

            string cuenta = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_cuentaCo));


            /*
            if (sr.STR_TIPORENDICION != "ORV")
            {
                sr.SOLICITUD_DET.ForEach((e) =>
                {
                    DetalleSerializar detalleSerializar = new DetalleSerializar
                    {
                        ItemCode = e.STR_CODARTICULO,
                        Quantity = e.STR_CANTIDAD,
                        Price = e.precioTotal / e.STR_CANTIDAD,
                        Currency = sr.STR_MONEDA,
                        //CostingCode = e.centCostos[0].name,
                        CostingCode = e.ceco,
                        CostingCode2 = e.posFinanciera.id,
                        U_CNCUP = e.cup.U_CUP,
                        TaxCode = "EXO"
                    };
                    detalleSerializars.Add(detalleSerializar);

                });
            }
            else
            {
                string conceptOrd = ConfigurationManager.AppSettings["concepto_orden_viaje"].ToString();

                DetalleSerializar detalleSerializar = new DetalleSerializar
                {
                    ItemCode = conceptOrd,
                    Quantity = 1,
                };
                detalleSerializars.Add(detalleSerializar);
            }*/

            DetalleSerializar detalleSerializar = new DetalleSerializar
            {
               AccountCode = cuenta,
               Currency = sr.STR_MONEDA.id,
               LineVendor = sr.STR_EMPLDASIG.provAsoc,
               RequiredDate = DateTime.ParseExact(sr.STR_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                U_CE_IMSL = sr.STR_TOTALSOLICITADO,
            };
            detalleSerializars.Add(detalleSerializar);

            SolicitudRDSerializer body = new SolicitudRDSerializer()
            {
                Series = 458,// ObtenerSerieOPRQ(),
                ReqType = 171,
                // Fechas
                RequriedDate = DateTime.ParseExact(sr.STR_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                DocDate = DateTime.ParseExact(sr.STR_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                TaxDate = DateTime.ParseExact(sr.STR_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                DocDueDate = DateTime.ParseExact(sr.STR_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                //RequriedDate = DateTime.ParseExact(sr.STR_FECHAINI, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"), //DateTime.Parse(sr.STR_FECHAINI).ToString("yyyy-MM-dd"),
                RequesterEmail = usuario.email,
                Comments = sr.STR_COMENTARIO,
                JournalMemo = sr.STR_COMENTARIO,
                // AttachmentEntry = adj.AbsoluteEntry,
                U_STR_TIPOEAR = sr.STR_MOTIVORENDICION.id,

                U_CE_TOTINCIMP = sr.STR_TOTALSOLICITADO,
                U_ELE_Tipo_ER = sr.STR_MOTIVORENDICION.id,
                U_ST_NroRQWeb = sr.ID.ToString(),
                U_CE_MNDA = sr.STR_MONEDA.id,
                /*
                U_DEPARTAMENTO = sr.STR_DIRECCION.Departamento,
                U_PROVINCIA = sr.STR_DIRECCION.Provincia,
                U_DISTRITO = sr.STR_DIRECCION.Distrito,
                U_STR_TIPORUTA = sr.STR_RUTA_INFO.Nombre,
                U_FECINI = DateTime.ParseExact(sr.STR_FECHAINI, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.STR_FECHAINI).ToString("yyyy-MM-dd"),
                U_FECFIN = DateTime.ParseExact(sr.STR_FECHAFIN, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.STR_FECHAFIN).ToString("yyyy-MM-dd"),              
                 */

                Requester = sr.STR_EMPLDASIG.sapID.ToString(),
                //RequesterName = sr.STR_EMPLEADO_ASIGNADO.Nombres.ToString(),
                //RequesterBranch = sr.STR_EMPLEADO_ASIGNADO.SubGerencia,
                //RequesterDepartment = sr.STR_EMPLEADO_ASIGNADO.dept,
          
                //DocDate = DateTime.ParseExact(sr.STR_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.STR_FECHAREGIS).ToString("yyyy-MM-dd"),
               // DocDueDate = string.IsNullOrEmpty(sr.STR_FECHAVENC) ? null : DateTime.ParseExact(sr.STR_FECHAVENC, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.STR_FECHAVENC).ToString("yyyy-MM-dd"),
               
                DocCurrency = sr.STR_MONEDA.id,
                DocumentLines = detalleSerializars,
                DocType = "dDocument_Service",
                DocRate = 1.0,
             
                U_STR_WEB_COD = (int)sr.ID,
                U_STR_WEB_AUTPRI = aprobadores[0].aprobadorNombre,
                U_STR_WEB_AUTSEG = aprobadores.Count > 1 ? aprobadores[1].aprobadorNombre : null,
                // U_ELE_Tipo_ER = solicitudRD.STR_TIPORENDICION_INFO.Nombre,
                Printed = "psYes",
                //sAuthorizationStatus = "dasGenerated",
                //U_ELE_SEDE = sr.STR_EMPLEADO_ASIGNADO.fax,
                //U_ELE_SUBGER = sr.STR_EMPLEADO_ASIGNADO.SubGerencia.ToString(),
                TaxCode = "EXO",
                TaxLiable = "tYES",
                // NUEVOS CAMPOS
                //U_STR_WEB_ORDV = sr.STR_ORDENVIAJE,
                U_STR_WEB_EMPASIG = sr.STR_EMPLDREGI.ToString(),
                U_STR_WEB_PRIID = aprobadores[0].aprobadorId.ToString(),
                U_STR_WEB_SEGID = aprobadores.Count > 1 ? aprobadores[1].aprobadorId.ToString() : null,
            };

            B1SLEndpoint sl = new B1SLEndpoint();
            string json = JsonConvert.SerializeObject(body);
            IRestResponse response = sl.CreateOrdenSL(json);

            return response;
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
