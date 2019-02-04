using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class Sesion
    {
        private static Sesion _UsuarioLogueado;
        //private static List<OpcionPerfil> _PermisosUsuario;
        public static Sesion UsuarioLogueado
        {
            get
            {
                var session = (Sesion)HttpContext.Current.Session["SessionUsuario"] ?? new Sesion();
                _UsuarioLogueado = session;
                return _UsuarioLogueado;
            }
            set
            {
                HttpContext.Current.Session["SessionUsuario"] = value;
            }
        }

        //public static List<OpcionPerfil> PermisosUsuario
        //{
        //    get
        //    {
        //        var session = (List<OpcionPerfil>)HttpContext.Current.Session["DatosSesion"] ?? new List<OpcionPerfil>();
        //        _PermisosUsuario = session;
        //        return _PermisosUsuario;
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["DatosSesion"] = value;
        //    }
        //}
    }
}
