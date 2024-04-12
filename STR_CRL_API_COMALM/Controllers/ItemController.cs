using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;
using STR_CRL_API_COMALM.BL;

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