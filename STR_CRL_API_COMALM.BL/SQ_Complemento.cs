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
