using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STR_CRL_API_COMALM.SQ;
using System.Collections;
using STR_CRL_API_COMALM.BL.Encriptacion;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Usuario
    {

        public ConsultationResponse<Usuario> ObtieneSesion(LoginRequest user)
        {
            var respOk = "OK";
            var respIncorrect = "Usuario y/o contraseña incorrecta";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                // Obtiene VALOR de la contraseña - Si no hay nada Es incorrecta
                string passActual = hash.GetValueSql(SQ_QueryManager.Generar(Sq_query.get_tokenPass), user.username);

                if (string.IsNullOrWhiteSpace(passActual)) throw new Exception(respIncorrect);

                // Obtiencontraseña y hace la validación
                Encript validacion = new Encript();
                bool reslt = validacion.ValidarCredenciales(passActual,user.password);

                if (!reslt) throw new Exception(respIncorrect);

                List<Usuario> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_query.get_infoUser), dc =>
                {
                    return new Usuario()
                    {
                        usuarioId = Convert.ToInt32(dc["STR_IDUSUARIO"]),
                        nombres  = dc["STR_NOMBRE"],
                        apellidos  = dc["STR_APELLIDO"],
                        email = dc["STR_CORREO"],
                        username  = dc["STR_USERNAME"],
                        password = dc["STR_CONTRASENIA"],
                        rol  = Sq_Rol.ObtieneRolPorId(Convert.ToInt32(dc["STR_IDROL"]))[0],
                        filial = Sq_Filial.obtenerFilialPorId(Convert.ToInt32(dc["U_ST_CeCo2"]))[0]
                    };
                }, user.username).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Usuario>(ex);
            }

        }
    }
}
