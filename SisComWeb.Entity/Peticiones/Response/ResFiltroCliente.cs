namespace SisComWeb.Entity
{
    public class ResFiltroCliente
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        public ClientePasajesEntity Valor { set; get; }

        public ResFiltroCliente()
        {
        }

        public ResFiltroCliente(bool esCorrecto, ClientePasajesEntity valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new ClientePasajesEntity());
            Mensaje = mensaje;
        }

        public ResFiltroCliente(bool esCorrecto, ClientePasajesEntity valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new ClientePasajesEntity());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
