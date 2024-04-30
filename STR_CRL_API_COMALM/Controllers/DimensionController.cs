using STR_CRL_API_COMALM.BL;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/dimension")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DimensionController : ApiController
    {
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            Sq_Dimension sq = new Sq_Dimension();
            var response = sq.ObtieneDimensiones(id.ToString());

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("project")]
        public IHttpActionResult GetProject()
        {
            Sq_Dimension sq = new Sq_Dimension();
            var response = sq.ObtieneProyectos();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}