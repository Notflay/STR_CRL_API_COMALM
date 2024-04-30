using STR_CRL_API_COMALM.BL;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/ubicacion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DireccionController : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            Sq_Direccion sq = new Sq_Direccion();
            var response = sq.ObtieneDirecciones();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}