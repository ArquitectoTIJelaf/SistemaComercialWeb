using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class Sesion
    {
        private static Sesion _UsuarioLogueado;

        public static Sesion UsuarioLogueado
        {
            get
            {
                var sesion = (Sesion)HttpContext.Current.Session["SessionUsuario"] ?? new Sesion();
                _UsuarioLogueado = sesion;
                return _UsuarioLogueado;
            }
            set
            {
                HttpContext.Current.Session["SessionUsuario"] = value;
            }
        }
    }
}
