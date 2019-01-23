namespace SisComWeb.Entity
{
    public class ResFiltroUsuario
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public UsuarioEntity Valor { set; get; }

        public ResFiltroUsuario()
        {
        }

        public ResFiltroUsuario(bool esCorrecto, UsuarioEntity valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new UsuarioEntity());
            Mensaje = mensaje;
        }

        public ResFiltroUsuario(bool esCorrecto, UsuarioEntity valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new UsuarioEntity());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
