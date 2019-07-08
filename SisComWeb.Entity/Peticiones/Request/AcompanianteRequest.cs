
namespace SisComWeb.Entity.Peticiones.Request
{
    public class AcompanianteRequest
    {
        public int IdVenta { get; set; }
       
        public AcompanianteEntity Acompaniante { get; set; }

        public byte ActionType { get; set; } // 1: Insert, 2: Update
    }
}
