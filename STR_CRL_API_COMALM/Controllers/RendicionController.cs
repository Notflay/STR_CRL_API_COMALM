using STR_CRL_API_COMALM.BL;
using STR_CRL_API_COMALM.EL.Request;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.IO;


using System.Web;

using System.Configuration;

using System.Net.Http.Headers;

using System.Net;

using System.Linq;

using System.Xml.Linq;
using STR_CRL_API_COMALM.SQ;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/rendicion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RendicionController : ApiController
    {
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult Get(string usrCreate, string usrAsign, int perfil, string fecini, string fecfin, string nrrendi, string estados, string area)
        {

            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ListarRendicones(usrCreate, usrAsign, perfil, fecini, fecfin, nrrendi, estados, area);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("documento/{id}")]
        public IHttpActionResult GetDocumento(string id)
        {

            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ObtenerDocumento(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("documento")]
        public IHttpActionResult Post(Documento documento)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.CrearDocumento(documento);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);

        }
        [HttpPatch]
        [Route("documento")]
        public IHttpActionResult Update(Documento documento)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ActualizarDocumento(documento);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpPatch]
        [Route("aprobacion/acepta")]
        public IHttpActionResult AceptaSolicitud(int solicitudId, string aprobadorId, string areaAprobador, int estado, int rendicionId, int area)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.AceptarAprobacion(solicitudId, aprobadorId, areaAprobador, estado, rendicionId, area);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("aprobacion/{id}")]
        public IHttpActionResult CreateAprobacion(int id, string idSolicitud, int usuarioId, int estado, string areaAprobador, decimal montoDiferencia)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.EnviarAprobacion(id.ToString(), idSolicitud, usuarioId, estado, areaAprobador);

            HanaADOHelper hash = new HanaADOHelper();
            hash.GetValueSql(SQ_QueryManager.Generar(Sq_Query.upd_montoDiferenciaRD), montoDiferencia.ToString(), id.ToString());

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }


        [HttpPatch]
        [Route("aprobacion/revertir/{rendicionId}")]
        public IHttpActionResult RevertirSolicitud(int rendicionId)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.RevertirAprobacion(rendicionId);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("autorizar/revertir/{rendicionId}")]
        public IHttpActionResult AutorizarRevertirSolicitud(int rendicionId)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.AutorizarRevertirAprobacion(rendicionId);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }



        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)

        {

            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ObtenerRendicion(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        
        [HttpPost]
        [Route("documento/plantilla/{id}")]
        public async Task<IHttpActionResult> PostUpload(int id)
        {
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            // Verificar si hay algún archivo en la solicitud
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("La solicitud no contiene un archivo.");
            }

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                if (provider.Contents.Count == 0)
                {
                    return BadRequest("No se proporcionó ningún archivo.");
                }

                var response = sQ_Complemento.UploadPlantillaAsync(provider.Contents[0], id);

                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest("No se proporcionó ningún archivo.");
            }
        }
        [HttpPost]
        [Route("documento/validacion/{id}")]
        public IHttpActionResult Validacion(int id)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ValidacionDocumento(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("upload-Pdf/{id}")]

        public async Task<IHttpActionResult> Cargarpdf(string id)
        {
            try
            {
                // Verificar si hay archivos en la solicitud
                if (HttpContext.Current.Request.Files.Count == 0)
                    return BadRequest("No se ha subido ningún archivo");

                // Obtener el archivo PDF
                var file = HttpContext.Current.Request.Files[0];

                // Verificar si el archivo es un PDF
                if (file.ContentType != "application/pdf")
                    return BadRequest("Tipo de archivo invalido. Sólo se permiten archivos PDF.");

                string urlPdfRendicion = ConfigurationManager.AppSettings["UrlPdfRendicion"];
                if (urlPdfRendicion == null)
                    return NotFound();

                if (!Directory.Exists(Path.GetDirectoryName(urlPdfRendicion)))
                    Directory.CreateDirectory(Path.GetDirectoryName(urlPdfRendicion));

                // Guardar temporalmente el archivo en el servidor
                var tempFilePath = Path.Combine(urlPdfRendicion, file.FileName);
                file.SaveAs(tempFilePath);


                // Guardar en sap el {tempFilePath} para la rendicion {id}
                Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
                var response = sq_Rendicion.cargarpdfRendicion(id, tempFilePath);

                if (response.CodRespuesta == "99")
                {
                    return BadRequest(response.DescRespuesta);
                }
                return Ok(response);

                return Ok($"Archivo PDF cargado correctamente en: {tempFilePath}");

            }
            catch (Exception)
            {
                return BadRequest("No se proporcionó ningún archivo.");
            }
        }

        [HttpGet]
        [Route("download-pdf/{id}")]
        public HttpResponseMessage GetPdfFile(string id)
        {

            try
            {
                Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
                var redencion = sq_Rendicion.ObtenerRendicion(id);
                var filePath = redencion.Result.FirstOrDefault().U_STR_FILER;
                if (!File.Exists(filePath))
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Archivo no encontrado");

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Path.GetFileName(filePath)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return response;
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error al descargar el archivo PDF: {ex.Message}");
            }


        }
        /*
        [HttpGet]
        [Route("documento/{id}")]
        public IHttpActionResult GetDocumento(string id)
        {

            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ObtenerDocumento(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("documento")]
        public IHttpActionResult Post(Documento documento)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.CrearDocumento(documento);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);

        }
        */
        /*
        [HttpPatch]
        [Route]
        public IHttpActionResult Update(Rendicion rendicion)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ActualizarRendicion(rendicion);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        */
        /*
        [HttpPost]
        [Route("aprobacion/{id}")]
        public IHttpActionResult CreateAprobacion(int id, string idSolicitud, int usuarioId, int estado, string areaAprobador)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.EnviarAprobacion(id.ToString(), idSolicitud, usuarioId, estado, areaAprobador);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }
        [HttpPatch]
        [Route("aprobacion/acepta")]
        public IHttpActionResult AceptaSolicitud(int solicitudId, string aprobadorId, string areaAprobador, int estado, int rendicionId, int area)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.AceptarAprobacion(solicitudId, aprobadorId, areaAprobador, estado, rendicionId, area);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }
        [HttpPatch]
        [Route("documento")]
        public IHttpActionResult Update(Documento documento)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ActualizarDocumento(documento);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpPatch]
        [Route("documento/snt/{id}")]
        public IHttpActionResult Updatesnt(int id, string estado)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ActualizarSntDocumento(id, estado);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("documento")]
        public IHttpActionResult Delete(int id, int rdId)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.BorrarDocumento(id, rdId);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        */
        /*
        [HttpDelete]
        [Route("documento/detalle")]
        public IHttpActionResult DeleteDet(int id, int docId)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.BorrarDetalleDcoumento(id, docId);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("documento/plantilla")]
        public HttpResponseMessage DescargarPlantilla()
        {
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.DescargarPlantillaDefecto();

            return response;
        }

        [HttpPost]
        [Route("documento/plantilla/{id}")]
        public async Task<IHttpActionResult> PostUpload(int id)
        {
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            // Verificar si hay algún archivo en la solicitud
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("La solicitud no contiene un archivo.");
            }

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                if (provider.Contents.Count == 0)
                {
                    return BadRequest("No se proporcionó ningún archivo.");
                }

                var response = sQ_Complemento.UploadPlantillaAsync(provider.Contents[0], id);

                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest("No se proporcionó ningún archivo.");
            }
        }

        [HttpPatch]
        [Route("aprobacion/reintentar/{id}")]
        public IHttpActionResult ReintentarSR(int id)
        {
            Sq_Rendicion sq_SolicitudRd = new Sq_Rendicion();
            var response = sq_SolicitudRd.ReintentarRendicion(id.ToString());

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("aprobadores")]
        public IHttpActionResult ObtieneAprobadores(int idRendicion)
        {
            Sq_Rendicion sq_SolicitudRd = new Sq_Rendicion();
            var response = sq_SolicitudRd.ObtieneAprobadores(idRendicion.ToString());

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("aprobacion/rechazar")]
        public IHttpActionResult RechazarSolicitud(int solicitudId, string aprobadorId, string areaAprobador, int estado, int rendicionId, int area)
        {
            Sq_Rendicion sq_SolicitudRd = new Sq_Rendicion();
            var response = sq_SolicitudRd.RechazarRendicion(solicitudId, aprobadorId, areaAprobador, estado, rendicionId, area);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("adjuntos/{id}")]
        public IHttpActionResult ObtieneAdjuntos(int id)
        {
            Sq_Rendicion sq_SolicitudRd = new Sq_Rendicion();
            var response = sq_SolicitudRd.ObtieneAdjuntos(id.ToString());

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
                Sq_Rendicion sq_Rendicion = new Sq_Rendicion();

                if (s.accion == "aceptar")
                {
                    var response = sq_Rendicion.AceptarAprobacion(Convert.ToInt32(s.idSolicitud), s.idAprobador, s.areaAprobador,
                        Convert.ToInt32(s.estado), Convert.ToInt32(s.rendicionId), Convert.ToInt32(s.area));

                    if (response != null && response.CodRespuesta == "99")
                    {
                        return BadRequest(response.DescRespuesta);
                    }

                    return Ok(s);
                }
                else
                {
                    var response = sq_Rendicion.RechazarRendicion(Convert.ToInt32(s.idSolicitud), s.idAprobador, s.areaAprobador,
                        Convert.ToInt32(s.estado), Convert.ToInt32(s.rendicionId), Convert.ToInt32(s.area));

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
        }*/
    }
}