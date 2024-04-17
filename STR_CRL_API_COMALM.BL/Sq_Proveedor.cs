using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Proveedor
    {
        public ConsultationResponse<Proveedor> ObtieneProveedores()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron estados";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Proveedor> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_proveedores), dc =>
                {
                    return new Proveedor
                    {
                        CardCode = dc["CardCode"],
                        CardName = dc["CardName"],
                        LicTradNum = dc["LicTradNum"]
                    };
                }).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Proveedor>(ex);
            }
        }
    }
}
