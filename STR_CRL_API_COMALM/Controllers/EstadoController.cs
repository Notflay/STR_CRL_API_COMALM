using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;
using STR_CRL_API_COMALM.BL;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/estado")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EstadoController : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            Sq_Estado sq = new Sq_Estado();
            var response = sq.ObtieneEstados();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}