namespace SisComWeb.Entity
{
    public class ResFiltroBus
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public BusEntity Valor { set; get; }

        public ResFiltroBus()
        {
        }

        public ResFiltroBus(bool esCorrecto, BusEntity valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new BusEntity());
            Mensaje = mensaje;
        }

        public ResFiltroBus(bool esCorrecto, BusEntity valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new BusEntity());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
