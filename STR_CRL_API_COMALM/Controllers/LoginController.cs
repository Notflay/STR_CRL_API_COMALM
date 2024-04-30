using STR_CRL_API_COMALM.BL;
using STR_CRL_API_COMALM.EL;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/sesion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public IHttpActionResult Post(string portalId, LoginRequest user)
        {
            Sq_Usuario sq = new Sq_Usuario();
            var response = sq.ObtieneSesion(user, portalId);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}