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

        public ConsultationResponse<Complemento> ObtenerProyecto(string id)
        {
            var respIncorrect = "No se encuentró el proyecto Asignado";
            List<Complemento> list = new List<Complemento>();
            try
            {
                list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_proyecto), dc =>
                {
                    return new Complemento()
                    {
                        id = dc["PrjCode"],
                        name = dc["PrjName"]
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }


        public ConsultationResponse<Articulo> ObtenerItem(string itemCode,string centroCosto)
        {
            var respIncorrect = "No se encuentra Items";

            try
            {
                List<Articulo> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_item), dc =>
                {
                    return new Articulo()
                    {
                        ItemCode = dc["ItemCode"],
                        ItemName = dc["ItemName"],
                        
                        U_BPP_TIPUNMED = dc["U_BPP_TIPUNMED"],
                        WhsCode = dc["WhsCode"],
                        Stock = string.IsNullOrWhiteSpace(Convert.ToString(dc["OnHand"])) ? (double?)null : Convert.ToDouble(dc["OnHand"]),
                        Precio = string.IsNullOrWhiteSpace(Convert.ToString(dc["AvgPrice"])) ? (double?)null : Convert.ToDouble(dc["AvgPrice"])
                        
                        // POSFINANCIERA = dc["posFinanciera"],
                        //posFinanciera = dc["posFinanciera"],
                        //CTA = dc["CTA"]
                    };
                }, centroCosto,itemCode).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Articulo>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtenerIndicador(string id)
        {
            var respIncorrect = "No se encontró indicador";

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_indicador), dc =>
                {
                    return new Complemento()
                    {
                        id = dc["id"],
                        name = dc["name"]
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
    }

}
