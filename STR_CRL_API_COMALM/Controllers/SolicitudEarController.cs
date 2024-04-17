using STR_CRL_API_COMALM.BL;
using STR_CRL_API_COMALM.EL.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/solicitudEar")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SolicitudEarController : ApiController
    {
        [HttpPost]
        [Route]
        public IHttpActionResult ValidaSolicitud(SolicitudRd slctd)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.CreaSolicitudRd(slctd);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult ObtieneSolicitud(int id, string create)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ObtenerSolicitud(id, create);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("lista")]
        public IHttpActionResult Get(string usrCreate, string usrAsign, int perfil, string fecini, string fecfin, string nrrendi, string estados, string area)
        {

            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ListarSolicutudes(usrCreate, usrAsign, perfil, fecini, fecfin, nrrendi, estados, area);

            return Ok(response);
        }
    }
}