using STR_CRL_API_COMALM.BL;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/item")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ItemController : ApiController
    {
        [HttpGet]
        [Route("art")]
        public IHttpActionResult Articulos(string area)
        {
            Sq_Item sq = new Sq_Item();
            var response = sq.ObtieneItems("art", area);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("serv")]
        public IHttpActionResult Servicios(string area)
        {
            Sq_Item sq = new Sq_Item();
            var response = sq.ObtieneItems("serv", area);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}