namespace SisComWeb.Entity
{
    public class ResFiltroRuc
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public RucEntity Valor { get; set; }

        public ResFiltroRuc()
        {
        }

        public ResFiltroRuc(bool esCorrecto, RucEntity valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new RucEntity());
            Mensaje = mensaje;
        }

        public ResFiltroRuc(bool esCorrecto, RucEntity valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new RucEntity());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
