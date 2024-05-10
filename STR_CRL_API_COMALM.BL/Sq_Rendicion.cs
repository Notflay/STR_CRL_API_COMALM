using STR_CRL_API_COMALM.EL.Request;
using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.EL.Commons;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using STR_CRL_API_COMALM.BL.Email;
using RestSharp;
using STR_CRL_API_COMALM.SL;
using System.Globalization;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Rendicion
    {
        HanaADOHelper hash = new HanaADOHelper();

        public ConsultationResponse<Rendicion> ListarRendicones(string usrCreate, string usrAsig, int perfil, string fecIni, string fecFin, string nrRendi, string estado, string area)
        {
            var respIncorrect = "No trajo la lista de solicitudes de rendición";
           // SQ_Complemento sQ = new SQ_Complemento();

            try
            {
                List<Rendicion> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_rendiciones), dc =>
                {
                    return new Rendicion()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_SOLICITUD = Convert.ToInt32(dc["STR_SOLICITUD"]),
                         STR_NRRENDICION = dc["STR_NRRENDICION"],
                         STR_NRAPERTURA = dc["STR_NRAPERTURA"],
                         STR_NRCARGA = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRCARGA"])) ? (int?)null : Convert.ToInt32(dc["STR_NRCARGA"]),
                         STR_ESTADO = Convert.ToInt32(dc["STR_ESTADO"]),
                         STR_ESTADO_INFO = new Complemento {id = dc["STR_ESTADO"], name = dc["STR_ESTADO_INFO"] },//dc["STR_ESTADO_INFO"] ,
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
        public ConsultationResponse<Complemento> ActualizarDocumento(Documento doc)
        {
            var respIncorrect = "No se pudo Actualizar Documento";

            List<Complemento> list = new List<Complemento>();

            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_idDOC),
                    doc.STR_RENDICION,
                    doc.STR_FECHA_CONTABILIZA,
                    doc.STR_FECHA_DOC,
                    doc.STR_FECHA_VENCIMIENTO,
                    doc.STR_PROVEEDOR?.CardCode,
                    doc.STR_PROVEEDOR?.LicTradNum,
                    doc.STR_TIPO_AGENTE?.id,
                    doc.STR_MONEDA?.name,
                    doc.STR_COMENTARIOS,
                    doc.STR_TIPO_DOC?.id,
                    doc.STR_SERIE_DOC,
                    doc.STR_CORR_DOC,
                    doc.STR_VALIDA_SUNAT == true ? 1 : 0,
                    doc.STR_ANEXO_ADJUNTO,
                    doc.STR_OPERACION,
                    doc.STR_PARTIDAFLUJO,
                    doc.STR_TOTALDOC,
                    doc.STR_PROVEEDOR?.CardName,
                    doc.STR_DIRECCION,
                    doc.STR_MOTIVORENDICION.id,
                    doc.STR_AFECTACION.id,
                    doc.ID
                    );

                //string idDoc = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_idDOC), doc.STR_RD_ID.ToString());
                // La actualización se hará en otro endpoint
                //var s = typeof doc.ID 
                doc.detalles.ForEach((e) =>
                {
                    // Si el detalle fue creado solo actualiza en la tabla 
                    if (e.ID != 0)
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_idDOCDet),
                        e.STR_CODARTICULO?.ItemCode,
                        e.STR_CODARTICULO?.ItemName,
                        e.STR_SUBTOTAL,
                        e.STR_INDIC_IMPUESTO?.id,
                        e.STR_PROYECTO?.id,
                        e.STR_ALMACEN,
                        e.STR_CANTIDAD,
                        e.STR_TPO_OPERACION,
                        e.STR_DIM1.id,
                        e.STR_DIM2.id,
                        e.STR_DIM4.id,
                        e.STR_DIM5.id,
                        e.STR_PRECIO,
                        e.STR_IMPUESTO,
                        e.ID);
                    }

                    else
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.post_insertDOCDt),
                             e.STR_CODARTICULO?.ItemCode,
                             e.STR_CODARTICULO.ItemName,
                             e.STR_SUBTOTAL,
                             e.STR_INDIC_IMPUESTO.id,
                             e.STR_DIM1.id,
                             e.STR_DIM2.id,
                             e.STR_DIM4.id,
                             e.STR_DIM5.id,
                             e.STR_ALMACEN,
                             e.STR_CANTIDAD,
                             e.STR_TPO_OPERACION,
                             e.STR_PROYECTO.id,
                             e.STR_PRECIO,
                             e.STR_IMPUESTO,
                             doc.ID);

                    }
                    if (Convert.ToInt32(e.FLG_ELIM) == 1)
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.del_idDOCDet), e.ID);
                    }
                    // Valida si ya tiene creado ID, si es así                     
                });

                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_RDTotal), doc.STR_RD_ID, doc.STR_RD_ID);

                Complemento complemento = new Complemento()
                {
                    id = doc.ID.ToString(),
                    name = "Se Actualizo Documento exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> CrearDocumento(Documento doc)
        {
            var respIncorrect = "No se pudo registrar Documento";
            Sq_Item sq = new Sq_Item();

            List<Complemento> list = new List<Complemento>();

            try
            {
                /*
                var test = ("INSERT INTO STR_WEB_DOC(STR_RENDICION,STR_FECHA_CONTABILIZA,STR_FECHA_DOC,STR_FECHA_VENCIMIENTO,STR_PROVEEDOR,STR_RUC,STR_MONEDA,STR_COMENTARIOS,STR_TIPO_DOC,STR_SERIE_DOC,STR_CORR_DOC,STR_VALIDA_SUNAT,STR_OPERACION,STR_PARTIDAFLUJO,STR_TOTALDOC,STR_RAZONSOCIAL,STR_RD_ID) VALUES",
                    doc.STR_RENDICION, doc.STR_FECHA_CONTABILIZA,
                    doc.STR_FECHA_DOC, doc.STR_FECHA_VENCIMIENTO, doc.STR_PROVEEDOR.CardCode, doc.STR_PROVEEDOR.LicTradNum, doc.STR_MONEDA.name, doc.STR_COMENTARIOS, doc.STR_TIPO_DOC.id, doc.STR_SERIE_DOC,
                    doc.STR_CORR_DOC, doc.STR_VALIDA_SUNAT == true ? 1 : 0, doc.STR_OPERACION,
                    doc.STR_PARTIDAFLUJO, doc.STR_TOTALDOC, doc.STR_PROVEEDOR.CardName, doc.STR_RD_ID);
                */
                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.post_insertDOC),
                    doc.STR_RENDICION,
                    doc.STR_FECHA_CONTABILIZA,
                    doc.STR_FECHA_DOC,
                    doc.STR_FECHA_VENCIMIENTO,
                    doc.STR_PROVEEDOR.CardCode,
                    doc.STR_PROVEEDOR.LicTradNum,
                    doc.STR_MONEDA.name,
                    doc.STR_COMENTARIOS,
                    doc.STR_TIPO_DOC.id,
                    doc.STR_SERIE_DOC,
                    doc.STR_CORR_DOC,
                    doc.STR_VALIDA_SUNAT == true ? 1 : 0,
                    doc.STR_OPERACION,
                    doc.STR_PARTIDAFLUJO,
                    doc.STR_TOTALDOC,
                    doc.STR_PROVEEDOR.CardName,
                    doc.STR_MOTIVORENDICION.id,
                    doc.STR_DIRECCION,
                    doc.STR_AFECTACION.id,
                    doc.STR_RD_ID);

                string idDoc = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_idDOC), doc.STR_RD_ID.ToString());
                

                doc.detalles.ForEach((e) =>
                {

                    hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.post_insertDOCDt), 
                        e.STR_CODARTICULO?.ItemCode,
                         e.STR_CODARTICULO.ItemName, 
                         e.STR_SUBTOTAL, 
                         e.STR_INDIC_IMPUESTO.id,
                         e.STR_DIM1.id,
                         e.STR_DIM2.id, 
                         e.STR_DIM4.id, 
                         e.STR_DIM5.id, 
                         //e.STR_PROYECTO.name,
                         e.STR_ALMACEN,
                         e.STR_CANTIDAD, 
                         e.STR_TPO_OPERACION,
                         e.STR_PROYECTO.id,
                         e.STR_PRECIO,
                         e.STR_IMPUESTO,
                         //e.STR_MOTIVORENDICION.name,
                         idDoc);
                });

                //hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_stateRDLoadDocs_RD), "9", doc.STR_RD_ID); 

                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_RDTotal), doc.STR_RD_ID, doc.STR_RD_ID);

                Complemento complemento = new Complemento()
                {
                    id = idDoc,
                    name = "Se creó documento exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> RevertirAprobacion(int rendicionId)
        {
            var respIncorrect = "No se revertio la aprobación";
            // Identificar si la aprobación es de Contabilidad o de otros aprobadores
            List<Complemento> list = new List<Complemento>();

            try
            {
                string valor = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.rev_estadoRD), rendicionId.ToString("F2"));
                Complemento complemento = new Complemento()
                {
                    id = valor,
                    name = "Se revertio la aprobación"
                };

                list.Add(complemento);
                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> AutorizarRevertirAprobacion(int rendicionId)
        {
            var respIncorrect = "No se autorizo la reversion de estado";
            // Identificar si la aprobación es de Contabilidad o de otros aprobadores
            List<Complemento> list = new List<Complemento>();

            try
            {
                string valor = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.upd_stateEdit_RD), "1", rendicionId.ToString("F2"));
                Complemento complemento = new Complemento()
                {
                    id = valor,
                    name = "Se autorizo la reversion de estado exitosamente"
                };

                list.Add(complemento);
                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<CreateResponse> AceptarAprobacion(int solicitudId, string aprobadorId, string areaAprobador, int estado, int rendicionId, int area)
        {
            var respIncorrect = "No se realizo la aprobación";
            // Identificar si la aprobación es de Contabilidad o de otros aprobadores
            Rendicion rendicion = new Rendicion();
            SolicitudRd solicitudRD = new SolicitudRd();
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();

            List<CreateResponse> list = new List<CreateResponse>();
            List<Aprobador> listaAprobados = new List<Aprobador>();
            try
            {

                solicitudRD = sq_SolicitudRd.ObtenerSolicitud(solicitudId, "PWB2").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_aprobadores), solicitudRD.STR_MOTIVORENDICION.id, solicitudRD.STR_AREA, solicitudRD.STR_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");

                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(3).Where(aprobador => aprobador != "-1").ToList();
                bool existeAprobador = aprobadores.Any(dat => dat.Equals(areaAprobador));

                // Valide que se encuentre en la lista de aprobadores pendientes
                listaAprobados = new List<Aprobador>();
                listaAprobados = ObtieneListaAprobadores(estado == 10 ? "2" : "3", solicitudRD.STR_NRSOLICITUD.ToString(), "0");
                existeAprobador = listaAprobados.Any(dat => dat.aprobadorId == Convert.ToInt32(aprobadorId));

                if (existeAprobador)
                {
                    if (estado == 10)
                    {

                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoRD), "11", "", rendicionId);
                        // Cambiar upd_aprobadoresRD que permita autorizar por área
                        /*
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("dd/MM/yy"), 1, areaAprobador, solicitudId, 0);
                        */
                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_aprobadoresRD), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, rendicionId, 0);
                        CreateResponse complemento = new CreateResponse()
                        {
                            DocEntry = 0,
                            DocNum = 0,
                            AprobacionFinalizada = 0,
                        };
                        list.Add(complemento);
                        //  ActualizarAprobacion(string idSolicitud, int usuarioId, int estado) 
                        EnviarAprobacion(rendicionId.ToString(), solicitudId.ToString(), Convert.ToInt32(solicitudRD.STR_EMPLDASIG.sapID), 11, area.ToString());
                    }
                    else if (estado == 11 & aprobadores.Count == 2 | estado == 13) // Valida estado final
                    {
                        // Envio de correo
                        EnviarEmail envio = new EnviarEmail();

                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_aprobadoresRD), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, rendicionId, 0);

                        var s = ObtenerRendicion(rendicionId.ToString());
                        rendicion = s.Result[0];
                        var response = GenerarRegistroDeRendicion(rendicion);
                        listaAprobados = ObtieneAprobadores(rendicion.ID.ToString()).Result;

                        if (response.IsSuccessful)
                        {

                            CreateResponse createResponse = JsonConvert.DeserializeObject<CreateResponse>(response.Content);
                            createResponse.AprobacionFinalizada = 1;

                            // Inserts despues de crear EAR en SAP
                            hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoRD), "16", "", rendicionId);
                            hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarMigradaRD), createResponse.DocEntry, createResponse.DocNum, rendicion.ID);
                            hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_migradaRdenSAP), listaAprobados[1].aprobadorNombre, listaAprobados.Count > 2 ? listaAprobados[2].aprobadorNombre : null, listaAprobados[0].aprobadorNombre, createResponse.DocEntry);

                            list.Add(createResponse);

                            /*
                            envio.EnviarInformativo(rendicion.STR_EMPLEADO_ASIGNADO.email, rendicion.STR_EMPLEADO_ASIGNADO.nombres, false,
                                rendicion.ID.ToString(), rendicion.STR_NRRENDICION, DateTime.Now.ToString("dd/MM/yyyy"), true, "");
                            */
                            return Global.ReturnOk(list, respIncorrect);
                        }
                        else
                        {
                            string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                            hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoRD), "17", mensaje.Replace("'", ""), rendicionId);
                            envio.EnviarError(false, rendicion.STR_NRRENDICION, rendicion.ID.ToString(), rendicion.STR_FECHAREGIS, mensaje.Replace("'", ""));
                            throw new Exception(mensaje);
                        }
                    }
                    else if (estado == 11 & aprobadores.Count == 3)
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoRD), "13", "", rendicionId);
                        // Cambiar upd_aprobadoresRD que permita autorizar por área
                        /*
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("dd/MM/yy"), 1, areaAprobador, solicitudId, 0);
                        */
                        hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.upd_aprobadoresRD), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, rendicionId, 0);
                        CreateResponse complemento = new CreateResponse()
                        {
                            DocEntry = 0,
                            DocNum = 0,
                            AprobacionFinalizada = 0,
                        };
                        list.Add(complemento);
                        //  ActualizarAprobacion(string idSolicitud, int usuarioId, int estado) 
                        EnviarAprobacion(rendicionId.ToString(), solicitudId.ToString(), Convert.ToInt32(solicitudRD.STR_EMPLDASIG), 13, area.ToString());
                    }
                    return Global.ReturnOk(list, respIncorrect);
                }
                else
                {
                    throw new Exception("No se tiene rendición a aprobar");
                }

            }
            catch (Exception ex)
            {
                return Global.ReturnError<CreateResponse>(ex);
            }
        }
        public IRestResponse GenerarRegistroDeRendicion(Rendicion rendicion)
        {
            List<RendicionDetSerializer> detalles = new List<RendicionDetSerializer>();

            // Obtiene Aprobadores
            List<Aprobador> aprobadores = new List<Aprobador>();
            aprobadores = ObtieneAprobadores(rendicion.ID.ToString()).Result;

            rendicion.documentos.ForEach((e) =>
            {
                Documento doc = new Documento();
                doc = ObtenerDocumento(e.ID.ToString()).Result[0];
                doc.detalles?.ForEach((d) =>
                {
                    detalles.Add(
                        new RendicionDetSerializer()
                        {
                            // doc = Cabecera
                            // d = detalle 
                            CardCode = doc.STR_PROVEEDOR.CardCode,
                            CardName = doc.STR_PROVEEDOR.CardName,
                            RUC = doc.STR_PROVEEDOR.LicTradNum,
                            //Comentarios = doc.STR_COMENTARIOS,
                            TipoDocumento = doc.STR_TIPO_DOC.id,
                            SerieDocumento = doc.STR_SERIE_DOC,
                            CorrelativoDocumento = doc.STR_CORR_DOC.PadLeft(8, '0'),
                            FechaEmision = DateTime.ParseExact(doc.STR_FECHA_DOC, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(doc.STR_FECHA_DOC).ToString("yyyy-MM-dd"),
                            FechaVencimiento = DateTime.ParseExact(doc.STR_FECHA_VENCIMIENTO, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(doc.STR_FECHA_VENCIMIENTO).ToString("yyyy-MM-dd"),
                            Moneda = doc.STR_MONEDA.name,
                            ItemCode = d.STR_CODARTICULO.ItemCode,
                            ItemDescription = d.STR_CODARTICULO.ItemName.Length > 50 ? d.STR_CODARTICULO.ItemName.Substring(0, 50) : d.STR_CODARTICULO.ItemName,
                            Impuesto = d.STR_INDIC_IMPUESTO.id,
                            PrecioUnidad = d.STR_SUBTOTAL / d.STR_CANTIDAD,
                            TotalLinea = d.STR_SUBTOTAL,
                            //CentroDCosto = d.STR_CENTCOSTO.CostCenter,
                            //PosFinanciera = d.STR_POS_FINANCIERA.name,
                            //CUP = d.STR_CUP.U_CUP,
                            Proyecto = d.STR_PROYECTO?.id,
                            TipoOperacion = doc.STR_OPERACION == "1" ? "01" : doc.STR_OPERACION,
                            Almacen = d.STR_ALMACEN,
                            Cantidad = d.STR_CANTIDAD,
                            //Retencion = doc.STR_TIPO_AGENTE.id == "0" ? "N" : doc.STR_TIPO_AGENTE.id == "1" ? "Y" : "N",
                            // Valores de la migración 
                            RutaAdjunto = doc.STR_ANEXO_ADJUNTO,
                            UnidadNegocio = d.STR_DIM1.id,
                            Filial = d.STR_DIM2.id,
                            Area = d.STR_DIM4.id,
                            CentroCosto = d.STR_DIM5.id,
                            // Valores por Defecto
                            U_CE_ESTD = "CRE",
                            U_CE_SLCC = "Y",
                            U_ObjType = "18",
                            U_ODUM_DocType = "I",
                        }
                        );
                });
            });

            RendicionSerializer body = new RendicionSerializer();


            body.NumRendicion = rendicion.STR_NRRENDICION;
            body.Moneda = rendicion.documentos[0].STR_MONEDA.id;
            body.FechaCargaDocs = DateTime.Now.ToString("yyyy-MM-dd");   // Por Defecto se asigna del día de hoy
           body.DocsTotal = rendicion.STR_TOTALRENDIDO;

            body.SaldoApertura = rendicion.STR_TOTALAPERTURA;
            //UsuarioEARCod = rendicion.STR_EMPLEADO_ASIGNADO.numeroEAR,

            // Valores de la migración
            //PrimerAutorizador = aprobadores[1].aprobadorNombre,
            //SegundoAutorizador = aprobadores.Count > 2 ? aprobadores[2].aprobadorNombre : null,
            //ContableAutorizador = aprobadores[0].aprobadorNombre,
            // valores en defecto
            body.Estado = "G";
            body.TipoActividad = "G";                                  // por defecto se asigna G
            body.SaldoFinal = 0.0;
            body.TipoRendicion = "EAR";

            body.U_STR_WEB_EMPASIG = rendicion.STR_EMPLDREGI.ToString();
            body.U_STR_WEB_PRIID = aprobadores[1].aprobadorId.ToString();
            body.U_STR_WEB_SEGID = aprobadores.Count > 2 ? aprobadores[2].aprobadorId.ToString() : null;
            body.U_STR_WEB_CONID = aprobadores[0].aprobadorId.ToString();
            //U_STR_WEB_EMPASIG = rendicion.STR_EMPLDREGI.ToString(),

            body.STR_EARCRGDETCollection = detalles;
            
            B1SLEndpoint sl = new B1SLEndpoint();
            string json = JsonConvert.SerializeObject(body);
            IRestResponse response = sl.CreateCargaDocumentos(json);

            return response;
        }
        public ConsultationResponse<Documento> ObtenerDocumento(string id)
        {
            var respIncorrect = "No se obtuvo Documento";
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            Sq_Item sq_item = new Sq_Item();
            Sq_Viatico sq_Viatico = new Sq_Viatico();
            Sq_Dimension sq_dimension = new Sq_Dimension();
            try
            {
                List<DocumentoDet> listDet = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_listaDocumentoDet), dc =>
                {
                    return new DocumentoDet()
                    {
                        ID = Convert.ToInt32(dc["ID"]),

                        STR_CODARTICULO = sq_item.ObtenerItem(dc["STR_CODARTICULO"], dc["STR_DIM2"]).Result.FirstOrDefault() ?? null,
                        STR_DOC_ID = Convert.ToInt32(dc["STR_DOC_ID"]),
                        STR_INDIC_IMPUESTO = sq_item.ObtenerIndicador(dc["STR_INDIC_IMPUESTO"]).Result.FirstOrDefault() ?? null,
                        STR_PROYECTO = dc["STR_PROYECTO"] == "" ? null : sq_item.ObtenerProyecto(dc["STR_PROYECTO"]).Result[0],
                        STR_SUBTOTAL = Convert.ToDouble(dc["STR_SUBTOTAL"]),
                        STR_ALMACEN = dc["STR_ALMACEN"],
                        STR_CONCEPTO = dc["STR_CONCEPTO"],
                        STR_CANTIDAD = Convert.ToInt32(dc["STR_CANTIDAD"]),
                        STR_PRECIO = Convert.ToDecimal(dc["STR_PRECIO"]),
                        STR_IMPUESTO = Convert.ToDecimal(dc["STR_IMPUESTO"]),
                        STR_DIM1 = sq_dimension.ObtieneDimension(dc["STR_DIM1"]).Result.FirstOrDefault() ?? null,
                        STR_DIM2 = sq_dimension.ObtieneDimension(dc["STR_DIM2"]).Result.FirstOrDefault() ?? null,
                        STR_DIM4 = sq_dimension.ObtieneDimension(dc["STR_DIM4"]).Result.FirstOrDefault() ?? null,
                        STR_DIM5 = sq_dimension.ObtieneDimension(dc["STR_DIM5"]).Result.FirstOrDefault() ?? null,
                        STR_TPO_OPERACION = dc["STR_TPO_OPERACION"]
                    };
                }, id).ToList();

                List<Documento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_obtenerDocumento), dc =>
                {
                    return new Documento()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        //STR_RENDICION = Convert.ToInt32(dc["STR_RENDICION"]),
                        STR_COMENTARIOS = dc["STR_COMENTARIOS"],
                        STR_ANEXO_ADJUNTO = dc["STR_ANEXO_ADJUNTO"],
                        STR_CORR_DOC = dc["STR_CORR_DOC"],
                        STR_FECHA_CONTABILIZA = string.IsNullOrWhiteSpace(dc["STR_FECHA_CONTABILIZA"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_CONTABILIZA"]).ToString("dd/MM/yyyy"),
                        STR_FECHA_DOC = string.IsNullOrWhiteSpace(dc["STR_FECHA_DOC"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_DOC"]).ToString("dd/MM/yyyy"),
                        STR_FECHA_VENCIMIENTO = string.IsNullOrWhiteSpace(dc["STR_FECHA_VENCIMIENTO"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_VENCIMIENTO"]).ToString("dd/MM/yyyy"),
                        STR_MONEDA = new Complemento { id = dc["STR_MONEDA"], name = dc["STR_MONEDA"] },
                        STR_OPERACION = dc["STR_OPERACION"],
                        STR_PARTIDAFLUJO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_PARTIDAFLUJO"])) ? (int?)null : Convert.ToInt32(dc["STR_PARTIDAFLUJO"]),
                        STR_PROVEEDOR = dc["STR_PROVEEDOR"] == "" ? null : ObtieneProveedorPrev(dc["STR_PROVEEDOR"], dc["STR_RUC"], dc["STR_RAZONSOCIAL"]),
                        STR_SERIE_DOC = dc["STR_SERIE_DOC"],
                        STR_VALIDA_SUNAT = Convert.ToUInt16(dc["STR_VALIDA_SUNAT"]) == 1,
                        //SR_TIPO_AGENTE = dc["STR_TIPO_AGENTE"] == "" ? null : ObtenTipoAgente(dc["STR_TIPO_AGENTE"]),
                        STR_TIPO_DOC = dc["STR_TIPO_DOC"] == "" ? null : sQ_Complemento.ObtenerTpoDocumento(dc["STR_TIPO_DOC"]).Result[0],
                        STR_RD_ID = Convert.ToInt32(dc["STR_RD_ID"]),
                        STR_DIRECCION = dc["STR_DIRECCION"],
                        STR_RUC = dc["STR_RUC"],
                        STR_RAZONSOCIAL = dc["STR_RAZONSOCIAL"],
                        STR_MOTIVORENDICION = string.IsNullOrEmpty(dc["STR_MOTIVORENDICION"]) ? null : sq_Viatico.ObtieneViatico(dc["STR_MOTIVORENDICION"]),
                        //STR_MOTIVORENDICION = string.IsNullOrEmpty(dc["STR_TIPORENDICION"]) ? null : sq_Viatico.ObtieneViatico(dc["STR_TIPORENDICION"]),
                        STR_TOTALDOC = Convert.ToDouble(dc["STR_TOTALDOC"]),
                        STR_AFECTACION = sQ_Complemento.ObtenerAfectacion(dc["STR_AFECTACION"]).Result[0],
                        detalles = listDet
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Documento>(ex);
            }

        }

        public Proveedor ObtieneProveedorPrev(string proveedor, string ruc, string razonSocial)
        {
            Proveedor _proveedor = new Proveedor();
            Sq_Proveedor sQ_Proveedor = new Sq_Proveedor();
            if (proveedor == "P99999999999")
            {
                _proveedor.CardCode = proveedor;
                _proveedor.CardName = razonSocial;
                _proveedor.LicTradNum = ruc;

                return _proveedor;
            }
            else
            {
                return sQ_Proveedor.ObtenerProveedor(proveedor).Result[0];
            }
        }
        public List<Aprobador> obtieneAprobadoresDet(string idArea, string aprobadorId, string fecha)
        {
            List<Aprobador> aprobadors = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_listaAprobadoresDetRd), dc =>
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
        public ConsultationResponse<Aprobador> ObtieneAprobadores(string idRendicion)
        {
            try
            {
                List<Aprobador> aprobadors = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_listaAprobadoresCabRd), dc =>
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
                }, idRendicion).ToList();


                return Global.ReturnOk(aprobadors, "");

            }
            catch (Exception ex)
            {
                return Global.ReturnError<Aprobador>(ex);
            }
        }
        public ConsultationResponse<Rendicion> ObtenerRendicion(string id)
        
        {
            var respIncorrect = "Obtener Rendicion";
            SQ_Complemento sQ = new SQ_Complemento();
            Sq_Usuario sQ_Usuario = new Sq_Usuario();
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();

            try
            {
                List<Rendicion> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_rendicion), dc =>
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
                        STR_TOTALAPERTURA = dc["STR_TOTALAPERTURA"] != null ? Convert.ToDouble(dc["STR_TOTALAPERTURA"]) : 0.0,
                        STR_FECHAREGIS = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"],
                        STR_EMPLEADO_ASIGNADO = sQ_Usuario.getUsuarioId("1",dc["STR_EMPLDASIG"]),
                        SOLICITUDRD = sq_SolicitudRd.ObtenerSolicitud(Convert.ToInt32(dc["STR_SOLICITUD"]), "PWB", false).Result[0],
                        STR_EDIT = (dc["STR_EDIT"]),
                        documentos = ObtenerDocumentos(dc["ID"]).Result
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Rendicion>(ex);
            }
        }


        public ConsultationResponse<Documento> ObtenerDocumentos(string id)
        {
            Sq_Proveedor sQ_Proveedor = new Sq_Proveedor();
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var respIncorrect = "No se obtuvo Documentos";

            try
            {
                List<Documento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_obtenerDocumentos), dc =>
                {
                    return new Documento()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_COMENTARIOS = dc["STR_COMENTARIOS"],
                        STR_ANEXO_ADJUNTO = dc["STR_ANEXO_ADJUNTO"],
                        STR_CORR_DOC = dc["STR_CORR_DOC"],
                        STR_FECHA_CONTABILIZA = string.IsNullOrWhiteSpace(dc["STR_FECHA_CONTABILIZA"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_CONTABILIZA"]).ToString("dd/MM/yyyy"),
                        STR_FECHA_DOC = string.IsNullOrWhiteSpace(dc["STR_FECHA_DOC"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_DOC"]).ToString("dd/MM/yyyy"),
                        STR_FECHA_VENCIMIENTO = string.IsNullOrWhiteSpace(dc["STR_FECHA_VENCIMIENTO"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_VENCIMIENTO"]).ToString("dd/MM/yyyy"),
                        STR_MONEDA = new Complemento { id = dc["STR_MONEDA"], name = dc["STR_MONEDA"] },
                        STR_OPERACION = dc["STR_OPERACION"],
                        STR_PARTIDAFLUJO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_PARTIDAFLUJO"])) ? (int?)null : Convert.ToInt32(dc["STR_PARTIDAFLUJO"]),
                        STR_PROVEEDOR = dc["STR_PROVEEDOR"] == "" ? null : ObtieneProveedorPrev(dc["STR_PROVEEDOR"], dc["STR_RUC"], dc["STR_RAZONSOCIAL"]),
                        STR_SERIE_DOC = dc["STR_SERIE_DOC"],
                        STR_VALIDA_SUNAT = Convert.ToUInt16(dc["STR_VALIDA_SUNAT"]) == 1,
                        STR_TIPO_AGENTE = new Complemento { id = dc["STR_TIPO_AGENTE"], name = dc["STR_TIPO_AGENTE"] },
                        STR_TIPO_DOC = dc["STR_TIPO_DOC"] == "" ? null : sQ_Complemento.ObtenerTpoDocumento(dc["STR_TIPO_DOC"]).Result[0],
                        // STR_TIPO_DOC = sQ_Complemento.ObtenerTpoDocumento(dc["STR_TIPO_DOC"]).Result[0],
                        STR_RD_ID = Convert.ToInt32(dc["STR_RD_ID"]),
                        STR_TOTALDOC = Convert.ToDouble(dc["STR_TOTALDOC"])
                        //detalles = listDet
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Documento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtenerEstado(int id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Estado";
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_estado), dc =>
                {
                    return new Complemento
                    {
                        id = dc["ID"],
                        name = dc["DESCRIPCION"]
                    };
                }, id.ToString()).ToList();

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public ConsultationResponse<AprobadorResponse> EnviarAprobacion(string idRendicion, string idSolicitud, int usuarioId, int estado, string areAprobador) // Id de la solicitud / Usuario y Estado 
        {

            // Obtiene Aprobadores
            List<AprobadorResponse> response = new List<AprobadorResponse>();
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            SolicitudRd solicitudRD = new SolicitudRd();
            List<Aprobador> aprobadors;
            try
            {
                // Solo actualizar a Cargado cuando se envie por primera vez la solicitud
                if (estado == 9 | estado == 12) hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.upd_cambiarEstadoRD), "10", "", idRendicion);

                solicitudRD = sq_SolicitudRd.ObtenerSolicitud(Convert.ToInt32(idSolicitud), "PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_aprobadores), solicitudRD.STR_MOTIVORENDICION.id, solicitudRD.STR_AREA, solicitudRD.STR_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");

                List<string> aprobadores = valor.Split(',').Take(3).Where(aprobador => aprobador != "-1").ToList();

                // Se encarga de insertar los aprobadores en la TABLA STR_WEB_APR_RD 
                hash.insertValueSql(SQ_QueryManager.Generar(Sq_Query.post_insertAprobadoresRD), idRendicion, usuarioId.ToString(), null, null, 0, estado == 9 ? aprobadores[aprobadores.Count == 2 ? 1 : 2] : estado == 11 ? aprobadores[0] : aprobadores[1]);

                aprobadors = new List<Aprobador>();
                aprobadors = ObtieneListaAprobadores(estado == 9 ? "2" : "3", solicitudRD.ID.ToString(), "0"); // Autorizadores, solicitud, estado

                if (aprobadors.Count < 1)
                    throw new Exception("No se encontró Aprobadores");

                aprobadors.ForEach(a =>
                {
                    EnviarEmail envio = new EnviarEmail();

                    if (!string.IsNullOrWhiteSpace(a.emailAprobador))
                    {
                        envio.EnviarConfirmacion(a.emailAprobador,
                        a.aprobadorNombre, a.nombreEmpleado, false, a.idSolicitud.ToString(), "",
                        a.fechaRegistro, estado == 9 ? "10" : estado.ToString(), a.area.ToString(), a.aprobadorId.ToString(), solicitudRD.ID.ToString(), solicitudRD.STR_AREA);
                    }
                });

                hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.upd_AnularModeEditRD), idRendicion);

                response = new List<AprobadorResponse> { new AprobadorResponse() {
                     aprobadores = aprobadores.Count()
                }};

                return Global.ReturnOk(response, "Ok");

            }
            catch (Exception ex)
            {
                return Global.ReturnError<AprobadorResponse>(ex);
            }

        }

        public List<Aprobador> ObtieneListaAprobadores(string tipoUsuario, string idSolicitud, string estado)
        {
            List<Aprobador> listaAprobadores = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_infoAprobadoresRD), dc =>
            {
                return new Aprobador
                {
                    idSolicitud = Convert.ToInt32(dc["STR_ID_RD"]),
                    aprobadorId = Convert.ToInt32(dc["Aprobador Id"]),
                    aprobadorNombre = dc["Nombre Autorizador"],
                    emailAprobador = dc["Email Aprobador"],
                    finalizado = Convert.ToInt32(dc["Finalizado"]),
                    //empleadoId = Convert.ToInt32(dc["Empleado Id"]),
                    nombreEmpleado = dc["Nombre Empleado"],
                    area = string.IsNullOrWhiteSpace(Convert.ToString(dc["Area"])) ? (int?)null : Convert.ToInt32(dc["Area"]),
                    fechaRegistro = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy")
                };
            }, tipoUsuario, idSolicitud, "0").ToList();

            return listaAprobadores;
        }

        /*
        public ConsultationResponse<Rendicion> ObtenerRendicion(string id)
        {
            var respIncorrect = "Obtener Rendicion";
            SQ_Complemento sQ = new SQ_Complemento();
            SQ_Usuario sQ_Usuario = new SQ_Usuario();
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();

            try
            {
                List<Rendicion> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_rendicion), dc =>
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
                        STR_TOTALAPERTURA = dc["STR_TOTALAPERTURA"] != null ? Convert.ToDouble(dc["STR_TOTALAPERTURA"]) : 0.0,
                        STR_FECHAREGIS = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"],
                        STR_EMPLEADO_ASIGNADO = sQ_Usuario.getUsuario(Convert.ToInt32(dc["STR_EMPLDASIG"])).Result[0],
                        SOLICITUDRD = sq_SolicitudRd.ObtenerSolicitud(Convert.ToInt32(dc["STR_SOLICITUD"]), "PWB", false).Result[0],
                        documentos = ObtenerDocumentos(dc["ID"]).Result
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Rendicion>(ex);
            }
        }

        public ConsultationResponse<Documento> ObtenerDocumentos(string id)
        {
            SQ_Proveedor sQ_Proveedor = new SQ_Proveedor();
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var respIncorrect = "No se obtuvo Documentos";

            try
            {
                List<Documento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtenerDocumentos), dc =>
                {
                    return new Documento()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_COMENTARIOS = dc["STR_COMENTARIOS"],
                        STR_ANEXO_ADJUNTO = dc["STR_ANEXO_ADJUNTO"],
                        STR_CORR_DOC = dc["STR_CORR_DOC"],
                        STR_FECHA_CONTABILIZA = string.IsNullOrWhiteSpace(dc["STR_FECHA_CONTABILIZA"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_CONTABILIZA"]).ToString("dd/MM/yyyy"),
                        STR_FECHA_DOC = string.IsNullOrWhiteSpace(dc["STR_FECHA_DOC"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_DOC"]).ToString("dd/MM/yyyy"),
                        STR_FECHA_VENCIMIENTO = string.IsNullOrWhiteSpace(dc["STR_FECHA_VENCIMIENTO"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_VENCIMIENTO"]).ToString("dd/MM/yyyy"),
                        STR_MONEDA = new Complemento { id = dc["STR_MONEDA"], name = dc["STR_MONEDA"] },
                        STR_OPERACION = dc["STR_OPERACION"],
                        STR_PARTIDAFLUJO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_PARTIDAFLUJO"])) ? (int?)null : Convert.ToInt32(dc["STR_PARTIDAFLUJO"]),
                        STR_PROVEEDOR = dc["STR_PROVEEDOR"] == "" ? null : ObtieneProveedorPrev(dc["STR_PROVEEDOR"], dc["STR_RUC"], dc["STR_RAZONSOCIAL"]),
                        STR_SERIE_DOC = dc["STR_SERIE_DOC"],
                        STR_VALIDA_SUNAT = Convert.ToUInt16(dc["STR_VALIDA_SUNAT"]) == 1,
                        STR_TIPO_AGENTE = new Complemento { id = dc["STR_TIPO_AGENTE"], name = dc["STR_TIPO_AGENTE"] },
                        STR_TIPO_DOC = dc["STR_TIPO_DOC"] == "" ? null : sQ_Complemento.ObtenerTpoDocumento(dc["STR_TIPO_DOC"]).Result[0],
                        // STR_TIPO_DOC = sQ_Complemento.ObtenerTpoDocumento(dc["STR_TIPO_DOC"]).Result[0],
                        STR_RD_ID = Convert.ToInt32(dc["STR_RD_ID"]),
                        STR_TOTALDOC = Convert.ToDouble(dc["STR_TOTALDOC"])
                        //detalles = listDet
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Documento>(ex);
            }
        }

        public ConsultationResponse<Documento> ObtenerDocumento(string id)
        {
            var respIncorrect = "No se obtuvo Documento";
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            Sq_Item sq_item = new Sq_Item();

            try
            {
                List<DocumentoDet> listDet = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaDocumentoDet), dc =>
                {
                    return new DocumentoDet()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_CODARTICULO = sq_item.ObtenerItem(dc["STR_CODARTICULO"]).Result.FirstOrDefault() ?? null,
                        STR_POS_FINANCIERA = new Complemento { id = dc["STR_POS_FINANCIERA"], name = dc["STR_POS_FINANCIERA"] },
                        STR_CENTCOSTO = new CentroCosto { CostCenter = dc["STR_CENTCOSTO"], name = dc["STR_CENTCOSTO"] },
                        STR_CUP = !string.IsNullOrEmpty(dc["STR_CENTCOSTO"]) & !string.IsNullOrEmpty(dc["STR_POS_FINANCIERA"]) ? sq_item.ObtenerCUP(Convert.ToInt32(dc["STR_CENTCOSTO"]), Convert.ToInt32(dc["STR_POS_FINANCIERA"]), 2022).Result.FirstOrDefault() ?? null : null,
                        STR_DOC_ID = Convert.ToInt32(dc["STR_DOC_ID"]),
                        STR_INDIC_IMPUESTO = sq_item.ObtenerIndicador(dc["STR_INDIC_IMPUESTO"]).Result.FirstOrDefault() ?? null,
                        STR_PROYECTO = dc["STR_PROYECTO"] == "" ? null : sq_item.ObtenerProyecto(dc["STR_PROYECTO"]).Result[0],
                        STR_SUBTOTAL = Convert.ToDouble(dc["STR_SUBTOTAL"]),
                        STR_ALMACEN = dc["STR_ALMACEN"],
                        STR_CANTIDAD = Convert.ToInt32(dc["STR_CANTIDAD"]),
                        STR_TPO_OPERACION = dc["STR_TPO_OPERACION"]
                    };
                }, id).ToList();

                List<Documento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtenerDocumento), dc =>
                {
                    return new Documento()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_COMENTARIOS = dc["STR_COMENTARIOS"],
                        STR_ANEXO_ADJUNTO = dc["STR_ANEXO_ADJUNTO"],
                        STR_CORR_DOC = dc["STR_CORR_DOC"],
                        STR_FECHA_CONTABILIZA = string.IsNullOrWhiteSpace(dc["STR_FECHA_CONTABILIZA"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_CONTABILIZA"]).ToString("dd/MM/yyyy"),
                        STR_FECHA_DOC = string.IsNullOrWhiteSpace(dc["STR_FECHA_DOC"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_DOC"]).ToString("dd/MM/yyyy"),
                        STR_FECHA_VENCIMIENTO = string.IsNullOrWhiteSpace(dc["STR_FECHA_VENCIMIENTO"]) ? "" : Convert.ToDateTime(dc["STR_FECHA_VENCIMIENTO"]).ToString("dd/MM/yyyy"),
                        STR_MONEDA = new Complemento { id = dc["STR_MONEDA"], name = dc["STR_MONEDA"] },
                        STR_OPERACION = dc["STR_OPERACION"],
                        STR_PARTIDAFLUJO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_PARTIDAFLUJO"])) ? (int?)null : Convert.ToInt32(dc["STR_PARTIDAFLUJO"]),
                        STR_PROVEEDOR = dc["STR_PROVEEDOR"] == "" ? null : ObtieneProveedorPrev(dc["STR_PROVEEDOR"], dc["STR_RUC"], dc["STR_RAZONSOCIAL"]),
                        STR_SERIE_DOC = dc["STR_SERIE_DOC"],
                        STR_VALIDA_SUNAT = Convert.ToUInt16(dc["STR_VALIDA_SUNAT"]) == 1,
                        STR_TIPO_AGENTE = dc["STR_TIPO_AGENTE"] == "" ? null : ObtenTipoAgente(dc["STR_TIPO_AGENTE"]),
                        STR_TIPO_DOC = dc["STR_TIPO_DOC"] == "" ? null : sQ_Complemento.ObtenerTpoDocumento(dc["STR_TIPO_DOC"]).Result[0],
                        STR_RD_ID = Convert.ToInt32(dc["STR_RD_ID"]),
                        STR_TOTALDOC = Convert.ToDouble(dc["STR_TOTALDOC"]),

                        detalles = listDet
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Documento>(ex);
            }

        }

        public Complemento ObtenTipoAgente(string agente)
        {

            string val = agente == "1" ? "Retención" : agente == "0" ? "Ninguno" : "Detracción";

            Complemento comp = new Complemento { id = agente, name = val };
            return comp;
        }

        public Proveedor ObtieneProveedorPrev(string proveedor, string ruc, string razonSocial)
        {
            Proveedor _proveedor = new Proveedor();
            SQ_Proveedor sQ_Proveedor = new SQ_Proveedor();
            if (proveedor == "P99999999999")
            {
                _proveedor.CardCode = proveedor;
                _proveedor.CardName = razonSocial;
                _proveedor.LicTradNum = ruc;

                return _proveedor;
            }
            else
            {
                return sQ_Proveedor.ObtenerProveedor(proveedor).Result[0];
            }
        }

        public ConsultationResponse<Complemento> CrearDocumento(Documento doc)
        {
            var respIncorrect = "No se pudo registrar Documento";
            Sq_Item sq = new Sq_Item();

            List<Complemento> list = new List<Complemento>();

            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertDOC), doc.STR_RENDICION, doc.STR_FECHA_CONTABILIZA,
                    doc.STR_FECHA_DOC, doc.STR_FECHA_VENCIMIENTO, doc.STR_PROVEEDOR.CardCode, doc.STR_PROVEEDOR.LicTradNum,
                    doc.STR_TIPO_AGENTE.id, doc.STR_MONEDA.name, doc.STR_COMENTARIOS, doc.STR_TIPO_DOC.id, doc.STR_SERIE_DOC,
                    doc.STR_CORR_DOC, doc.STR_VALIDA_SUNAT == true ? 1 : 0, doc.STR_ANEXO_ADJUNTO, doc.STR_OPERACION, doc.STR_PARTIDAFLUJO, doc.STR_TOTALDOC, doc.STR_PROVEEDOR.CardName, doc.STR_RD_ID);

                string idDoc = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_idDOC), doc.STR_RD_ID.ToString());

                doc.detalles.ForEach((e) =>
                {
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertDOCDt), e.STR_CODARTICULO?.id,
                         e.STR_CODARTICULO != null ? sq.ObtenerItem(e.STR_CODARTICULO.id).Result[0].ItemName : null, e.STR_SUBTOTAL, e.STR_INDIC_IMPUESTO.id, e.STR_PROYECTO.name, e.STR_CENTCOSTO.CostCenter, e.STR_CODARTICULO.posFinanciera,
                        e.STR_CUP.U_CUP, e.STR_ALMACEN, e.STR_CANTIDAD, e.STR_TPO_OPERACION, idDoc);
                });

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_RDTotal), doc.STR_RD_ID, doc.STR_RD_ID);

                Complemento complemento = new Complemento()
                {
                    id = idDoc,
                    name = "Se creó documento exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        public ConsultationResponse<Complemento> ActualizarRendicion(Rendicion rendicion)
        {
            var respIncorrect = "No se pudo Actualizar Rendición";

            List<Complemento> list = new List<Complemento>();

            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_Rendicon), rendicion.STR_NRAPERTURA, rendicion.STR_NRCARGA, rendicion.STR_ESTADO, rendicion.STR_TOTALRENDIDO, rendicion.STR_DOCENTRY, rendicion.STR_MOTIVOMIGR, rendicion.ID);

                Complemento complemento = new Complemento()
                {
                    id = rendicion.ID.ToString(),
                    name = "Se Actualizo Rendición exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {

                return Global.ReturnError<Complemento>(ex);
            }

        }
        public ConsultationResponse<Complemento> ActualizarDocumento(Documento doc)
        {
            var respIncorrect = "No se pudo Actualizar Documento";

            List<Complemento> list = new List<Complemento>();

            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_idDOC), doc.STR_RENDICION, doc.STR_FECHA_CONTABILIZA,
                    doc.STR_FECHA_DOC, doc.STR_FECHA_VENCIMIENTO, doc.STR_PROVEEDOR?.CardCode, doc.STR_PROVEEDOR?.LicTradNum,
                    doc.STR_TIPO_AGENTE?.id, doc.STR_MONEDA?.name, doc.STR_COMENTARIOS, doc.STR_TIPO_DOC?.id, doc.STR_SERIE_DOC,
                    doc.STR_CORR_DOC, doc.STR_VALIDA_SUNAT == true ? 1 : 0, doc.STR_ANEXO_ADJUNTO, doc.STR_OPERACION, doc.STR_PARTIDAFLUJO, doc.STR_TOTALDOC, doc.STR_PROVEEDOR?.CardName, doc.ID);

                //string idDoc = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_idDOC), doc.STR_RD_ID.ToString());
                // La actualización se hará en otro endpoint
                //var s = typeof doc.ID
                doc.detalles.ForEach((e) =>
                {
                    // Si el detalle fue creado solo actualiza en la tabla 
                    if (e.ID != 0)
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_idDOCDet), e.STR_CODARTICULO?.id,
                            e.STR_CODARTICULO?.name, e.STR_SUBTOTAL, e.STR_INDIC_IMPUESTO?.id, e.STR_PROYECTO?.name, e.STR_CENTCOSTO?.CostCenter, e.STR_CODARTICULO?.posFinanciera,
                            e.STR_CUP?.U_CUP, e.STR_ALMACEN, e.STR_CANTIDAD, e.STR_TPO_OPERACION, e.ID);
                    }
                    else
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertDOCDt), e.STR_CODARTICULO?.id,
                            e.STR_CODARTICULO?.name, e.STR_SUBTOTAL, e.STR_INDIC_IMPUESTO?.id, e.STR_PROYECTO?.name, e.STR_CENTCOSTO?.CostCenter, e.STR_CODARTICULO?.posFinanciera,
                            e.STR_CUP?.U_CUP, e.STR_ALMACEN, e.STR_CANTIDAD, e.STR_TPO_OPERACION, doc.ID);
                    }
                    // Valida si ya tiene creado ID, si es así                     
                });

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_RDTotal), doc.STR_RD_ID, doc.STR_RD_ID);

                Complemento complemento = new Complemento()
                {
                    id = doc.ID.ToString(),
                    name = "Se Actualizo Documento exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        //public ConsultationResponse<Complemento> ActualizarDocumentoDet()
        public ConsultationResponse<Complemento> BorrarDocumento(int id, int rdId)
        {
            var respIncorrect = "No se pudo eliminar DOcumento";

            List<Complemento> list = new List<Complemento>();
            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_documentoDet), id);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_documento), id);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_RDTotal), rdId, rdId);

                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    name = "Se elimino documento exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        */
        public ConsultationResponse<Complemento> ValidacionDocumento(int id)
        {

            var respIncorrect = "Validación Erronea";
            List<Complemento> list = new List<Complemento>();
            Documento doc = new Documento();
            List<string> CamposVacios = new List<string>();

            try
            {
                doc = ObtenerDocumento(id.ToString()).Result[0];

                if (doc.STR_PROVEEDOR == null) CamposVacios.Add("Proveedor");
                //if (doc.STR_TIPO_AGENTE == null) CamposVacios.Add("Retención o Detracción");
                if (doc.STR_MONEDA == null) CamposVacios.Add("Moneda");
                // if (doc.STR_COMENTARIOS == null) CamposVacios.Add("Moneda");
                if (doc.STR_TIPO_DOC == null) CamposVacios.Add("Tipo de Documento");
                if (string.IsNullOrEmpty(doc.STR_SERIE_DOC)) CamposVacios.Add("Serie de Documento");
                if (string.IsNullOrEmpty(doc.STR_CORR_DOC)) CamposVacios.Add("Correlativo de Documento");
                //if (doc.STR_VALIDA_SUNAT == false) CamposVacios.Add("Validación SUNAT");
                //if (string.IsNullOrEmpty(doc.STR_ANEXO_ADJUNTO)) CamposVacios.Add("Adjunto");

                int contDuplicateDocs = Convert.ToInt32(hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_duplicated_docs), doc.STR_PROVEEDOR.LicTradNum, doc.STR_SERIE_DOC, doc.STR_CORR_DOC));
                if (contDuplicateDocs > 1) CamposVacios.Add("Ya existe un Documento con RUC, SERIE y CORRELATIVO iguales");

                doc.detalles.ForEach((e) =>
                {
                    if (e.STR_CODARTICULO == null) { CamposVacios.Add("Nivel detalle 1"); return; }
                    if (e.STR_INDIC_IMPUESTO == null) { CamposVacios.Add("Nivel detalle 2"); return; }
                    // if (e.STR_PROYECTO == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.STR_DIM1 == null) { CamposVacios.Add("Nivel detalle 3"); return; }
                    if (e.STR_DIM2 == null) { CamposVacios.Add("Nivel detalle 4"); return; }
                    //if (e.STR_DIM3 == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.STR_DIM4 == null) { CamposVacios.Add("Nivel detalle 5"); return; }
                    if (e.STR_DIM5 == null) { CamposVacios.Add("Nivel detalle 6"); return; }
                    if (e.STR_SUBTOTAL == 0) { CamposVacios.Add("Nivel detalle 7"); return; }
                    if (e.STR_ALMACEN == null) { CamposVacios.Add("Nivel detalle 8"); return; }
                    if (e.STR_CANTIDAD == 0) { CamposVacios.Add("Nivel detalle 9"); return; }
                    //if (e.STR_TPO_OPERACION == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.STR_DOC_ID == 0) { CamposVacios.Add("Nivel detalle 10"); return; }
                    //if (e.STR_CONCEPTO == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.STR_PROYECTO == null) { CamposVacios.Add("Nivel detalle 11"); return; }
                    if (e.STR_PRECIO == 0) { CamposVacios.Add("Nivel detalle 12"); return; }
                    if (e.STR_IMPUESTO == 0) { CamposVacios.Add("Nivel detalle 13"); return; }

                });

                if (doc.detalles.Count == 0) CamposVacios.Add("Lineas de Detalle");

                if (CamposVacios.Count > 0)
                {
                    string CamposErroneos = string.Join(", ", CamposVacios);
                   
                    if (contDuplicateDocs > 1)
                    {
                        respIncorrect += $" {doc.STR_SERIE_DOC + "-" + doc.STR_CORR_DOC} | " + CamposErroneos + $" al documento con ID : {doc.ID}";
                    }
                    else
                    {
                        respIncorrect += $" {doc.STR_SERIE_DOC + "-" + doc.STR_CORR_DOC} | Faltan completar campos de " + CamposErroneos;
                    }

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
        }
        /*
        public ConsultationResponse<Complemento> BorrarDetalleDcoumento(int id, int docId)
        {
            var respIncorrect = "No se pudo eliminar Detalle de Documento";

            List<Complemento> list = new List<Complemento>();
            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_documentoDetId), id, docId);

                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    name = "Se elimino documento exitosamente"
                };
                list.Add(complemento);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_DOCTotal), docId, docId);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        */
    }
}
