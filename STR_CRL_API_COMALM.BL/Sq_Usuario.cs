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
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Usuario
    {
        HanaADOHelper hash = new HanaADOHelper();
        public ConsultationResponse<Usuario> ObtieneSesion(LoginRequest user, string portalId)
        {
            var respOk = "OK";
            var respIncorrect = "Usuario y/o contraseña incorrecta";

            List<Usuario> list = null;

            try
            {
                // Obtiene VALOR de la contraseña - Si no hay nada Es incorrecta
                string passActual = hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.get_tokenPass), user.username);

                if (string.IsNullOrWhiteSpace(passActual)) throw new Exception(respIncorrect);

                // Obtiencontraseña y hace la validación
                Encript validacion = new Encript();
                bool reslt = validacion.ValidarCredenciales(passActual, user.password);

                if (!reslt) throw new Exception(respIncorrect);

                // ObtieneId

                list = new List<Usuario>() {
                    getUsuario(portalId, user.username)
                };

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Usuario>(ex);
            }

        }
        public Usuario getUsuario(string portalId, string username) {

            List<Usuario> list = null; 
            try
            {
                list = new List<Usuario>();

                list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_infoUser), dc =>
                {
                    return new Usuario()
                    {
                        usuarioId = Convert.ToInt32(dc["STR_IDUSUARIO"]),
                        sapID = Convert.ToInt32(dc["empID"]),
                        nombres = dc["firstName"],
                        apellidos = dc["lastName"],
                        email = dc["email"],
                        username = dc["STR_USERNAME"],
                        password = dc["STR_CONTRASENIA"],
                        rol = Sq_Rol.ObtieneRolPorId(Convert.ToInt32(dc["STR_IDROL"]))[0],
                        filial = Sq_Filial.obtenerFilialPorId(Convert.ToInt32(dc["U_ST_CeCo2"]))[0],
                        area = new Complemento {  id = dc["Area"], name = dc["AreaDesc"] },
                    };
                }, portalId.ToString(), username).ToList();

                return list[0];
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Usuario getUsuarioId(string portalId, string empId)
        {

            List<Usuario> list = null;
            try
            {
                list = new List<Usuario>();

                list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_infoUserId), dc =>
                {
                    return new Usuario()
                    {
                        usuarioId = Convert.ToInt32(dc["STR_IDUSUARIO"]),
                        sapID = Convert.ToInt32(dc["empID"]),
                        nombres = dc["firstName"],
                        apellidos = dc["lastName"],
                        email = dc["email"],
                        username = dc["STR_USERNAME"],
                        password = dc["STR_CONTRASENIA"],
                        rol = Sq_Rol.ObtieneRolPorId(Convert.ToInt32(dc["STR_IDROL"]))[0],
                        filial = Sq_Filial.obtenerFilialPorId(Convert.ToInt32(dc["U_ST_CeCo2"]))[0]
                    };
                }, portalId.ToString(), empId).ToList();

                return list[0];
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
