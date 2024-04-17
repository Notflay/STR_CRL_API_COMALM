using STR_CRL_API_COMALM.BL.Encriptacion;
using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Estado
    {
        public ConsultationResponse<Complemento> ObtieneEstados()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron estados";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_estados), dc =>
                {
                    return new Complemento
                    {
                        id = dc["ID"],
                        name = dc["DESCRIPCION"]
                    };
                }).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
    }
}
