using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/solicitudTrsl")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SolicitudTrslController : ApiController
    {
    }
}