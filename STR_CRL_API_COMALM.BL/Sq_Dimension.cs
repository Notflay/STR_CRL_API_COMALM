using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Dimension
    {
        public ConsultationResponse<Complemento> ObtieneDimensiones(string dimension)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron dimensiones";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_dimensiones), dc =>
                {
                    return new Complemento
                    {
                        id = dc["PrcCode"],
                        name = dc["PrcName"]
                    };
                }, dimension).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtieneDimension(string dim)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron dimensiones";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_dimension), dc =>
                {
                    return new Complemento
                    {
                        id = dc["PrcCode"],
                        name = dc["PrcName"]
                    };
                }, dim).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtieneProyectos()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron proyectos";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_proyectos), dc =>
                {
                    return new Complemento
                    {
                        id = dc["PrjCode"],
                        name = dc["PrjName"]
                    };
                }).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtieneProyecto(string id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontraron proyectos";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(Sq_Query.get_proyecto), dc =>
                {
                    return new Complemento
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

    }
}
