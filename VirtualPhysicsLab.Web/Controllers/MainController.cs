using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VirtualPhysicsLab.Web.Controllers
{
    public class MainController : Controller
    {
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            if (IsInternetExplorer(Request))
            {
                return RedirectPreserveMethod(@"http://outdatedbrowser.com/hr");
            }
            else
            {
                return File("index.html", "text/html");
            }
        }

        private static bool IsInternetExplorer(HttpRequest req)
        {
            string userAgent = req.Headers["User-Agent"];

            return (userAgent.Contains("MSIE") || userAgent.Contains("Trident"));
        }
    }
}