using System.Web;
using System.Web.Mvc;

namespace STR_CRL_API_COMALM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
