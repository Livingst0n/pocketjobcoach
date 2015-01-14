using System.Web;
using System.Web.Mvc;

namespace Pocket_Job_Coach
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}