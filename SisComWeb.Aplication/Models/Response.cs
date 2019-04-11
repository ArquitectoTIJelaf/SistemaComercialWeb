namespace SisComWeb.Aplication.Models
{
    public class Response<T>
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public T Valor { set; get; }

        public bool EsCorrecto { get; set; }

        public Response()
        {
        }

        public Response(bool estado, string mensaje, T valor)
        {
            Estado = estado;
            Mensaje = mensaje;
            Valor = valor;
        }

        public Response (bool estado, string mensaje, T valor, bool esCorrecto)
        {
            Estado = estado;
            Mensaje = mensaje;
            Valor = valor;
            EsCorrecto = esCorrecto;
        }
    }
}
