using OfficeOpenXml;
using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class SQ_Complemento
    {
        public ConsultationResponse<Complemento> ObtenerTpoDocumento(string id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontró tipo de documento";
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                // if (id == "-99") throw new Exception("No se encuentra data con el Field ID " + id.ToString());

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_tpoDocumento), dc =>
                {
                    return new Complemento
                    {
                        id = dc["id"],
                        name = dc["name"],
                    };
                }, id).ToList();

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

        public ConsultationResponse<Complemento> ObtenerAfectacion(string id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontró la afectacion";
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                // if (id == "-99") throw new Exception("No se encuentra data con el Field ID " + id.ToString());
                //lista de afectaciones
                List<Complemento> list = new List<Complemento>
                    {
                        new Complemento { id = "1", name = "Retencion" },
                        new Complemento { id = "2", name = "Detraccion" },
                        new Complemento { id = "3", name = "-" }
            };

                Complemento complemento = list.FirstOrDefault(c => c.id == id);

                if (complemento == null) {
                    complemento = new Complemento { id = "3", name = "-" };
                        }

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = complemento != null ? "00" : "22",
                    DescRespuesta = complemento != null ? respOk : respIncorrect,
                    Result = complemento != null ? new List<Complemento> { complemento } : new List<Complemento>()
                };
                /*
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };*/
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
        public ConsultationResponse<Complemento> ObtenerTpoDocumentos()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontró tipo de documentos";
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                // if (id == "-99") throw new Exception("No se encuentra data con el Field ID " + id.ToString());

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_tpoDocumentos), dc =>
                {
                    return new Complemento
                    {
                        id = dc["id"],
                        name = dc["name"],
                    };
                }, string.Empty).ToList();

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
        
        public async Task<ConsultationResponse<Complemento>> UploadPlantillaAsync(HttpContent file, int id)
        {
            Plantilla sq_Plantilla = new Plantilla();
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var respOk = "OK";
            var respIncorrect = "No se encontró plantilla";
            List<Complemento> list = new List<Complemento>();


            HanaADOHelper hash = new HanaADOHelper();
            try
            {

                var stream = await file.ReadAsStreamAsync();
                var package = new ExcelPackage(stream);
                var docs = sq_Plantilla.ObtienePlantilla(package, id);
                docs.ForEach((e) =>
                {
                    sq_Rendicion.CrearDocumento(e);
                });

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
        
    }
}
