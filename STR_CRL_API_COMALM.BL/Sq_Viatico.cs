using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Viatico
    {
        public string ear_id { get; set; }
        public Sq_Viatico() {
            ear_id = ConfigurationManager.AppSettings["campoId_ear"];
        } 

        public ConsultationResponse<Complemento> ObtieneViaticos()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron dimensiones";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_query.get_viaticos), dc =>
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
    }
}
