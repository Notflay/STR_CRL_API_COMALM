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
    public class Sq_Rol
    {
        public ConsultationResponse<Complemento> ObtieneRoles()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron estados";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_roles), dc =>
                {
                    return new Complemento
                    {
                        id = dc["STR_IDROL"],
                        name = dc["STR_NOMBRE_ROL"]
                    };
                }).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtieneRol(int id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron roles";
          
            try
            {
                return Global.ReturnOk(ObtieneRolPorId(id), respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public static List<Complemento> ObtieneRolPorId(int id) 
        {
            HanaADOHelper hash = new HanaADOHelper();

            List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_rol), dc =>
            {
                return new Complemento
                {
                    id = dc["STR_IDROL"],
                    name = dc["STR_NOMBRE_ROL"]
                };
            }, id.ToString()).ToList();

            return list;
        }
    }
}
