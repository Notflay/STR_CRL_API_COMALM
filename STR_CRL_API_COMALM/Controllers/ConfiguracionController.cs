using STR_CRL_API_COMALM.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/configuracion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConfiguracionController : ApiController
    {
        [HttpGet]
        [Route("cfgeneral")]
        public IHttpActionResult CreaCFGeneral()
        {
            // Valida si hay alguna configuración con el mismo codigo 
            Sq_Configuracion consulta = new Sq_Configuracion();
            var response = consulta.getCFGeneral();
            //response
            // var response = "";
            return Ok(response);
        }
    }
}