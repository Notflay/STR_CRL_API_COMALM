using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/solicitudTrsl")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SolicitudTrslController : ApiController
    {
    }
}