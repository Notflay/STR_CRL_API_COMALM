using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;
using STR_CRL_API_COMALM.EL;
using STR_CRL_API_COMALM.BL;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/sesion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public IHttpActionResult Post(LoginRequest user) 
        {
            Sq_Usuario sq = new Sq_Usuario();
            var response = sq.ObtieneSesion(user);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}