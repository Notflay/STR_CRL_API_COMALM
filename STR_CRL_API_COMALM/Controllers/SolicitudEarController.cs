using STR_CRL_API_COMALM.BL;
using STR_CRL_API_COMALM.EL;
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
        public IHttpActionResult Post(SolicitudRd slctd)
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
            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route]
        public IHttpActionResult Update(SolicitudRd solicitudRD)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ActualizarSolicitudSr(solicitudRD);
            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("aprobacion/{id}")]
        public IHttpActionResult SolicitaAprobacion(int id, Rq_Aprobacion request)
        {
            //string rutaAPI = Request.Headers.Referrer.AbsoluteUri + Request.RequestUri.Segments[1] + Request.RequestUri.Segments[2] + Request.RequestUri.Segments[3];

            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.EnviarSolicitudAprobacion(id.ToString(), request.usuarioId, request.tipord, request.area, request.monto, request.estado, request.p_Borradores);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("aprobacion/acepta")]
        public IHttpActionResult AceptaSolicitud(int id, string aprobadorId, string areaAprobador, int estado)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.AceptarSolicitud(id, aprobadorId, areaAprobador, estado);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("aprobacion/rechazar")]
        public IHttpActionResult RechazarSolicitud(int id, string aprobadorId, Complemento comentarios, string areaAprobador)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.RechazarSolicitud(id.ToString(), aprobadorId, comentarios?.name, areaAprobador);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("aprobacion/reintentar/{id}")]
        public IHttpActionResult ReintentarSR(int id)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ReintentarSolicitud(id);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("aprobadores")]
        public IHttpActionResult ObtieneAprobadores(string idSolicitud)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ObtieneAprobadores(idSolicitud);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }
        [HttpPost]
        [Route("validatoken")]
        public IHttpActionResult ValidaToken(string token)
        {
            BL.Token token1 = new BL.Token();
            try
            {
                var s = token1.LeerToken(token);
                Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();

                if (s.accion == "aceptar")
                {
                    var response = sq_SolicitudRd.AceptarSolicitud(Convert.ToInt32(s.idSolicitud), s.idAprobador, s.areaAprobador, Convert.ToInt32(s.estado));

                    if (response != null && response.CodRespuesta == "99")
                    {
                        return BadRequest(response.DescRespuesta);
                    }

                    return Ok(s);
                }
                else
                {
                    var response = sq_SolicitudRd.RechazarSolicitud(s.idSolicitud, s.idAprobador, "", s.areaAprobador);

                    if (response != null && response.CodRespuesta == "99")
                    {
                        return BadRequest(response.DescRespuesta);
                    }

                    return Ok(s);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());

            }
        }
    }
}