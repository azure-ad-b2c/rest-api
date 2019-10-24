using System.Web;
using System.Web.Mvc;

namespace AADB2C.WebAPI.BasicAuth
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
