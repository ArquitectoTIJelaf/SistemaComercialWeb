﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity.Objects.Entities
{
    public class FechaAbiertaEntity
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string FechaVenta { get; set; }
        public string PrecioVenta { get; set; }
        public string CodiSubruta { get; set; }
        public string CodiOrigen { get; set; }
        public string CodiEmpresa { get; set; }
        public string IdVenta { get; set; }
    }
}
