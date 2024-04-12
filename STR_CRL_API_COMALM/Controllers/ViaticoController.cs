using STR_CRL_API_COMALM.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/viatico")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ViaticoController : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            Sq_Viatico sq = new Sq_Viatico();
            var response = sq.ObtieneViaticos();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}