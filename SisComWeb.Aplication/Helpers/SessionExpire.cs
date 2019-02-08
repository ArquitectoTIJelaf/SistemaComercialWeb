using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace SisComWeb.Aplication.Helpers
{
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (String.IsNullOrWhiteSpace(DataSession.UsuarioLogueado.Nombre))
            {
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                       {
                                { "action", "Index" },
                                { "controller", "Autenticacion" },
                       });
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Autenticacion/AjaxSessionExpired");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}