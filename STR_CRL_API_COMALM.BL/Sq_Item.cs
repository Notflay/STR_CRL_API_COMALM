using STR_CRL_API_COMALM.EL.Response;
using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Item
    {
        public ConsultationResponse<Complemento> ObtieneItems()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron items";

            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_query.get_items), dc =>
                {
                    return new Complemento
                    {
                        id = dc["ItemCode"],
                        name = dc["ItemName"]
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
