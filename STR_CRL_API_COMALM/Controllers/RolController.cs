using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;
using STR_CRL_API_COMALM.BL;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/rol")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RolController : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            Sq_Rol sq = new Sq_Rol();
            var response = sq.ObtieneRoles();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

    }
}