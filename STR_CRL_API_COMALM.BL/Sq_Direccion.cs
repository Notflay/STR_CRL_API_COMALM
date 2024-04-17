using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Direccion
    {
        public ConsultationResponse<Direccion> ObtieneDirecciones()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron estados";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Direccion> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_direccion), dc =>
                {
                    return new Direccion
                    {
                        PrcCode = dc["PrcCode"],
                        PrcName = dc["PrcName"],
                        DimCode = Convert.ToInt32(dc["DimCode"]),
                        InfoDireccion = dc["InfoDireccion"]
                    };
                }).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Direccion>(ex);
            }
        }
    }
}
