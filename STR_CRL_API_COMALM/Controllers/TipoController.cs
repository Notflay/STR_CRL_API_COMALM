using STR_CRL_API_COMALM.BL;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{

    [RoutePrefix("api/tipoear")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TipoController : ApiController
    {
        [Route("documentos")]
        [HttpGet]
        public IHttpActionResult GetDocumentos()
        {
            // Obtiene Campo ID


            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.ObtenerTpoDocumentos();

            return Ok(response);
        }
    }
}