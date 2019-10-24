using System.Web;
using System.Web.Mvc;

namespace AADB2C.WebAPI.ClientCert
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
