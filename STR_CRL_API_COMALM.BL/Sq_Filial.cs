using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.EL.Response;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Filial
    {
        public ConsultationResponse<Filial> ObtieneFiliales()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron estados";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Filial> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_query.get_filiales), dc =>
                {
                    return new Filial
                    {
                        Code = dc["Code"],
                        Name = dc["Name"],
                        U_ST_Filial = dc["U_ST_Filial"],
                        U_ST_NombreFilial = dc["U_ST_NombreFilial"],
                        U_ST_Ref = dc["U_ST_Ref"]
                    };
                }).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Filial>(ex);
            }
        }

        public ConsultationResponse<Filial> ObtieneFilial(int code)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron estados";

            //HanaADOHelper hash = new HanaADOHelper();

            try
            {
              
                return Global.ReturnOk(obtenerFilialPorId(code), respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Filial>(ex);
            }
        }

        public static List<Filial> obtenerFilialPorId(int code) {

            HanaADOHelper hash = new HanaADOHelper();

            List<Filial> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_query.get_filiales), dc =>
            {
                return new Filial
                {
                    Code = dc["Code"],
                    Name = dc["Name"],
                    U_ST_Filial = dc["U_ST_Filial"],
                    U_ST_NombreFilial = dc["U_ST_NombreFilial"],
                    U_ST_Ref = dc["U_ST_Ref"]
                };
            }, code.ToString()).ToList();

            return list;
        }
    }
}
