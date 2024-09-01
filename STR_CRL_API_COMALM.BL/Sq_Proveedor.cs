using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Proveedor
    {
        HanaADOHelper hash = new HanaADOHelper();
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
        public ConsultationResponse<Proveedor> ObtenerProveedor(string id)
        {
            var respIncorrect = "No Hay Proveedores";

            try
            {
                List<Proveedor> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_proveedor), dc =>
                {
                    return new Proveedor()
                    {
                        CardCode = dc["CardCode"],
                        CardName = dc["CardName"],
                        LicTradNum = dc["LicTradNum"]
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Proveedor>(ex);
            }
        }
        public ConsultationResponse<Proveedor> ObtenerProveedorxRuc(string ruc)
        {
            var respIncorrect = "No hay Proveedores con este RUC";

            try
            {
                List<Proveedor> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_proveedorxruc), dc =>
                {
                    return new Proveedor()
                    {
                        CardCode = dc["CardCode"],
                        CardName = dc["CardName"],
                        LicTradNum = dc["LicTradNum"]
                    };
                }, ruc).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Proveedor>(ex);
            }
        }
    }
}
