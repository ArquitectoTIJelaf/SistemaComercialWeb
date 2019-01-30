namespace SisComWeb.Entity
{
    public class ResFiltroTurnoViaje
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public TurnoViajeEntity Valor { set; get; }

        public ResFiltroTurnoViaje()
        {
        }

        public ResFiltroTurnoViaje(bool esCorrecto, TurnoViajeEntity valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new TurnoViajeEntity());
            Mensaje = mensaje;
        }

        public ResFiltroTurnoViaje(bool esCorrecto, TurnoViajeEntity valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new TurnoViajeEntity());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
