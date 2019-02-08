using SisComWeb.Aplication.Helpers;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [SessionExpire]
    [RoutePrefix("venta-pasajes")]
    public class VentaPasajesController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}