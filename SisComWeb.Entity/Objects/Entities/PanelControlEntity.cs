namespace SisComWeb.Entity
{
    public class PanelControlEntity
    {
        public string CodiPanel { get; set; }

        public string Valor { get; set; }

        public string Descripcion { get; set; }

        public string TipoControl { get; set; }
    }

    public class PanelControlNivelEntity
    {
        public int Codigo { get; set; }

        public string Descripcion { get; set; }

        public byte Nivel { get; set; }
    }
}
