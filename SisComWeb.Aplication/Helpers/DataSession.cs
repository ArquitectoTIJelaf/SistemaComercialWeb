using SisComWeb.Aplication.Models;
using System.Web;

namespace SisComWeb.Aplication.Helpers
{
    public class DataSession
    {
        private static Usuario _UsuarioLogueado;

        public static Usuario UsuarioLogueado
        {
            get
            {
                var session = (Usuario)HttpContext.Current.Session["SessionUsuario"] ?? new Usuario();
                _UsuarioLogueado = session;
                return _UsuarioLogueado;
            }
            set
            {
                HttpContext.Current.Session["SessionUsuario"] = value;
            }
        }
    }
}