//using SisComWeb.Aplication..Servicio;
using SisComWeb.Aplication.Models;
using System.Collections.Generic;
using System.Web;

namespace SisComWeb.Aplication.Helpers
{
    public class DataSession
    {
        private static Usuario _UsuarioLogueado;
        //private static List<OpcionPerfil> _PermisosUsuario;
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