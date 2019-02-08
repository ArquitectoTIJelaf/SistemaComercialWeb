using SisComWeb.Aplication.Helpers;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [SessionExpire]
    [RoutePrefix("principal")]
    public class PrincipalController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}