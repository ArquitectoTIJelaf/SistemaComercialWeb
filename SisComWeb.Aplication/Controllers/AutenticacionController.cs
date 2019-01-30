using System;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [RoutePrefix("login")]
    public class AutenticacionController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("redirect")]
        public ActionResult Redirect()
        {

            return RedirectToAction("Index", "Default");
        }

        [HttpPost]
        [Route("post-usuario")]
        public void POST(string usuario)
        {
            string ipaddress = HttpContext.Request.UserHostAddress;
            string useragent = HttpContext.Request.UserAgent;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "ValidaUsuario");
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(usuario);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                // Log exception and throw as for GET example above
            }
        }
    }
}