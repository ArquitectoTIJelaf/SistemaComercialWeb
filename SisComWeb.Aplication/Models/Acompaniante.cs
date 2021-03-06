﻿namespace SisComWeb.Aplication.Models
{
    public class Acompaniante
    {
        public string CodiTipoDoc { get; set; }

        public string Documento { get; set; }

        public string NombreCompleto { get; set; }

        public string FechaNac { get; set; }

        public string Edad { get; set; }

        public string Sexo { get; set; }

        public string Parentesco { get; set; }
    }

    public class AcompanianteRequest
    {
        public int IdVenta { get; set; }

        public Acompaniante Acompaniante { get; set; }

        public byte ActionType { get; set; }
    }
}