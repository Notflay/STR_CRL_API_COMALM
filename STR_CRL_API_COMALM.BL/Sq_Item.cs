using STR_CRL_API_COMALM.EL.Response;
using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Item
    {
        HanaADOHelper hash = new HanaADOHelper();
        public ConsultationResponse<Articulo> ObtieneItems(string tipo,string area)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron items";

            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                List<Articulo> list = hash.GetResultAsType(SQ_QueryManager.Generar(tipo == "art" ? Sq_Query.get_items_art : Sq_Query.get_items_serv), dc =>
                {
                    return new Articulo
                    {
                        ItemCode = dc["ItemCode"],
                        ItemName = dc["ItemName"],
                        U_BPP_TIPUNMED = dc["U_BPP_TIPUNMED"],
                        WhsCode = dc["WhsCode"],
                        Stock = string.IsNullOrWhiteSpace(Convert.ToString(dc["OnHand"])) ? (double?)null : Convert.ToDouble(dc["OnHand"]),                   
                        Precio = string.IsNullOrWhiteSpace(Convert.ToString(dc["AvgPrice"])) ? (double?)null : Convert.ToDouble(dc["AvgPrice"]),
                    };
                }, area.ToString()).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Articulo>(ex);
            }
        }

        public ConsultationResponse<Item> ObtenerItem(string itemCode)
        {
            var respIncorrect = "No se encuentra Items";

            try
            {
                List<Item> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_item), dc =>
                {
                    return new Item()
                    {
                        ItemCode = dc["id"],
                        ItemName = dc["name"],
                        id = dc["id"],
                        name = dc["name"],
                        // POSFINANCIERA = dc["posFinanciera"],
                        posFinanciera = dc["posFinanciera"],
                        CTA = dc["CTA"]

                    };
                }, itemCode).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Item>(ex);
            }
        }
    }
}
