using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;

namespace STR_CRL_API_COMALM.Controllers
{
    [RoutePrefix("api/lgin")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {

        [HttpPost]
        public IHttpActionResult Post() 
        {



            return Ok();
        }

    }
}