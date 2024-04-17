using STR_CRL_API_COMALM.EL.Response;
using STR_CRL_API_COMALM.EL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STR_CRL_API_COMALM.SQ;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Configuracion
    {
        HanaADOHelper hash = new HanaADOHelper();
        public ConsultationResponse<CFGeneral> getCFGeneral(string sociedad = "SBO_CRL_210922")
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Sociedad";
            

            //sociedad = ConfigurationManager.AppSettings["CompanyDB"].ToString();

            try
            {
                List<CFGeneral> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_cf), dc =>
                {
                    return new CFGeneral
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_IMAGEN = dc["STR_IMAGEN"],
                        STR_SOCIEDAD = dc["STR_SOCIEDAD"],
                        STR_MAXADJRD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXADJRD"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXADJRD"]),
                        STR_MAXADJSR = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXADJSR"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXADJSR"]),
                        STR_MAXAPRRD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXAPRRD"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXAPRRD"]),
                        STR_MAXAPRSR = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXAPRSR"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXAPRSR"]),
                        STR_MAXRENDI_CURSO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXRENDI_CURSO"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXRENDI_CURSO"]),
                        STR_OPERACION = dc["STR_OPERACION"],
                        STR_PARTIDAFLUJO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_PARTIDAFLUJO"])) ? (int?)null : Convert.ToInt32(dc["STR_PARTIDAFLUJO"]),
                        STR_PLANTILLARD = dc["STR_PLANTILLARD"]
                    };
                }, sociedad).ToList();

                return new ConsultationResponse<CFGeneral>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<CFGeneral>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
    }
}
