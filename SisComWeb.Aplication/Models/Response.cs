
namespace SisComWeb.Aplication.Models
{
    public class Response<T>
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public T Valor { set; get; }

        public Response (bool estado, string mensaje, T valor)
        {
            Estado = estado;
            Mensaje = mensaje;
            Valor = valor;
        }
    }
}
