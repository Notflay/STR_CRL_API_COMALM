using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Viatico
    {
        HanaADOHelper hash = new HanaADOHelper();
        public string ear_id { get; set; }
        public Sq_Viatico() {
            ear_id = ConfigurationManager.AppSettings["campoId_ear"];
        } 

        public ConsultationResponse<Complemento> ObtieneViaticos()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron dimensiones";
      
            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_viaticos), dc =>
                {
                    return new Complemento
                    {
                        id = dc["FldValue"],
                        name = dc["Descr"]
                    };
                }, ear_id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public Complemento ObtieneViatico(string id) 
        {
            List<Complemento> viatico = new List<Complemento>();
            try
            {
                 viatico = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_viatico), dc =>
                {
                    return new Complemento
                    {
                        id = dc["FldValue"],
                        name = dc["Descr"]
                    };
                }, ear_id, id).ToList();

                return viatico[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ConsultationResponse<Complemento> ObtieneTpViaticos()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron dimensiones";

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_tpViaticos), dc =>
                {
                    return new Complemento
                    {
                        id = dc["Code"],
                        name = dc["Name"]
                    };
                }).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public Complemento ObtieneTpViatico(string id)
        {
            List<Complemento> viatico = new List<Complemento>();
            try
            {
                viatico = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_tpViatico), dc =>
                {
                    return new Complemento
                    {
                        id = dc["Code"],
                        name = dc["Name"]
                    };
                }, id).ToList();

                return viatico[0];
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
