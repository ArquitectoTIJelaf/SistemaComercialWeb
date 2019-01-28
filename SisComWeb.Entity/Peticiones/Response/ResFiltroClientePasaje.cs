namespace SisComWeb.Entity
{
    public class ResFiltroClientePasaje
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public ClientePasajeEntity Valor { set; get; }

        public ResFiltroClientePasaje() { }

        public ResFiltroClientePasaje(bool esCorrecto, ClientePasajeEntity valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new ClientePasajeEntity());
            Mensaje = mensaje;
        }

        public ResFiltroClientePasaje(bool esCorrecto, ClientePasajeEntity valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new ClientePasajeEntity());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
