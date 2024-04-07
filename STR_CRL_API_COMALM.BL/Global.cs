using STR_CRL_API_COMALM.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Global
    {
        public static ConsultationResponse<T> ReturnError<T>(Exception ex)
        {
            return new ConsultationResponse<T>
            {
                CodRespuesta = "99",
                DescRespuesta = ex.Message,
            };
        }

        public static ConsultationResponse<T> ReturnOk<T>(List<T> list, string respIncorrect)
        {
            string respOk = "OK";
            return new ConsultationResponse<T>
            {
                CodRespuesta = list.Count() > 0 ? "00" : "22",
                DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                Result = list
            };
        }
    }
}
