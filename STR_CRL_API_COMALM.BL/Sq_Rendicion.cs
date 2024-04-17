using STR_CRL_API_COMALM.EL.Request;
using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (doc.STR_TIPO_AGENTE == null) CamposVacios.Add("Retención o Detracción");
                if (doc.STR_MONEDA == null) CamposVacios.Add("Moneda");
                // if (doc.STR_COMENTARIOS == null) CamposVacios.Add("Moneda");
                if (doc.STR_TIPO_DOC == null) CamposVacios.Add("Tipo de Documento");
                if (string.IsNullOrEmpty(doc.STR_SERIE_DOC)) CamposVacios.Add("Serie de Documento");
                if (string.IsNullOrEmpty(doc.STR_CORR_DOC)) CamposVacios.Add("Correlativo de Documento");
                if (doc.STR_VALIDA_SUNAT == false) CamposVacios.Add("Validación SUNAT");
                if (string.IsNullOrEmpty(doc.STR_ANEXO_ADJUNTO)) CamposVacios.Add("Adjunto");

                doc.detalles.ForEach((e) =>
                {
                    if (e.STR_CODARTICULO == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.STR_INDIC_IMPUESTO == null) { CamposVacios.Add("Nivel detalle"); return; }
                    // if (e.STR_PROYECTO == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.STR_CENTCOSTO == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.STR_POS_FINANCIERA == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.STR_CUP == null) { CamposVacios.Add("Nivel detalle"); return; }
                });

                if (doc.detalles.Count == 0) CamposVacios.Add("Lineas de Detalle");

                if (CamposVacios.Count > 0)
                {
                    string CamposErroneos = string.Join(", ", CamposVacios);
                    respIncorrect += $" {doc.STR_SERIE_DOC + "-" + doc.STR_CORR_DOC} | Faltan completar campos de " + CamposErroneos;
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
