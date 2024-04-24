using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
