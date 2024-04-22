using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using STR_CRL_API_COMALM.BL;
using STR_CRL_API_COMALM.EL.Request;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/rendicion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RendicionController : ApiController
    {
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult Get(string usrCreate, string usrAsign, int perfil, string fecini, string fecfin, string nrrendi, string estados, string area)
        {

            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ListarRendicones(usrCreate, usrAsign, perfil, fecini, fecfin, nrrendi, estados, area);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("documento")]
        public IHttpActionResult Post(Documento documento)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.CrearDocumento(documento);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);

        }
    }


}