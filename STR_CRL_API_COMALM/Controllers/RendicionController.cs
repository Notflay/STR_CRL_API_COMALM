using STR_CRL_API_COMALM.BL;
using STR_CRL_API_COMALM.EL.Request;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;


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

        [HttpDelete]
        [Route("documento/{id}")]
        public IHttpActionResult Delete(int id)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.EliminarDocumento(id);

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
                if (HttpContext.Current.Request.Files.Count == 0)
                    return BadRequest("No se ha subido ningún archivo");

                string urlPdfRendicion = ConfigurationManager.AppSettings["UrlPdfRendicion"];
                if (urlPdfRendicion == null)
                    return NotFound();

                string rendicionFolder = Path.Combine(urlPdfRendicion, id);
                if (!Directory.Exists(rendicionFolder))
                    Directory.CreateDirectory(rendicionFolder);

                List<string> filePaths = new List<string>();

                foreach (string fileKey in HttpContext.Current.Request.Files.AllKeys)
                {
                    var file = HttpContext.Current.Request.Files[fileKey];

                    if (file != null && file.ContentLength > 0)
                    {
                        var filePath = Path.Combine(rendicionFolder, file.FileName);
                        file.SaveAs(filePath);

                        filePaths.Add(filePath);
                    }
                }

                Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
                var response = sq_Rendicion.cargarpdfRendicion(id, filePaths);

                if (response.CodRespuesta == "99")
                {
                    return BadRequest(response.DescRespuesta);
                }

                return Ok("Todos los archivos se cargaron y procesaron correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al cargar los archivos: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("archivos/{id}")]
        public IHttpActionResult GetArchivosPorRendicion(string id)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ObtenerArchivosPorRendicionId(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            //var nombresArchivos = response.Result.Select(archivo => archivo.name).ToList();

            return Ok(response);
        }

        //[HttpGet]
        //[Route("download-Pdf/{id}/{fileName}")]
        //public IHttpActionResult DownloadPdf(string id, string fileName)
        //{
        //    try
        //    {
        //        // Obtener la ruta base desde la configuración
        //        string urlPdfRendicion = ConfigurationManager.AppSettings["UrlPdfRendicion"];
        //        if (urlPdfRendicion == null)
        //            return NotFound();

        //        // Construir la ruta completa del archivo
        //        string filePath = Path.Combine(urlPdfRendicion, id, fileName);

        //        if (!File.Exists(filePath))
        //            return NotFound();

        //        // Leer el archivo y devolverlo como respuesta
        //        var result = new HttpResponseMessage(HttpStatusCode.OK)
        //        {
        //            Content = new ByteArrayContent(File.ReadAllBytes(filePath))
        //        };

        //        // Determinar el tipo MIME y establecer en la respuesta
        //        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //        {
        //            FileName = fileName
        //        };

        //        // Devolver la respuesta
        //        return ResponseMessage(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Error al descargar el archivo: " + ex.Message);
        //    }
        //}

        [HttpGet]
        [Route("download-pdf/{IdDoc}")]
        public HttpResponseMessage GetPdfFile(int IdDoc)
        {
            try
            {
                Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
                var respuesta = sq_Rendicion.ConsultarRutaArchivo(IdDoc);
                var ruta = respuesta.Result[0].ruta;
                //string filePath = sq_Rendicion.ConsultarRutaArchivo(IdDoc).ruta;
                //filePath = "C:\\Rendiciones\\EAR-2024-480504358\\CapturaFormulario.pdf";
                //Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
                //var redencion = sq_Rendicion.ObtenerRendicion(id);
                //var filePath = redencion.Result.FirstOrDefault().U_STR_FILER;
                if (!File.Exists(ruta))
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Archivo no encontrado");

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(ruta, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Path.GetFileName(ruta)
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