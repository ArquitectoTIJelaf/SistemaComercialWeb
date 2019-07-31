﻿namespace SisComWeb.Entity.Peticiones.Request
{
    public class ReintegroRequest
    {
        public string Tipo { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public int CodiEmpresa { get; set; }        
    }

    public class ReintegroVentaRequest
    {
        public string Serie { get; set; }
        public string nume_boleto { get; set; }
        public string Codi_Empresa { get; set; }
        public string CODI_SUCURSAL { get; set; }
        public string CODI_PROGRAMACION { get; set; }
        public string CODI_SUBRUTA { get; set; }
        public string CODI_Cliente { get; set; }
        public string NIT_CLIENTE { get; set; }
        public double PRECIO_VENTA { get; set; }
        public string NUMERO_ASIENTO { get; set; }
        public string FLAG_VENTA { get; set; }
        public string FECH_VENTA { get; set; }
        public string Recoger { get; set; }
        public string Clav_Usuario { get; set; }
        public string Dni { get; set; }
        public string EDAD { get; set; }
        public string TELEF { get; set; }
        public string NOMB { get; set; }
        public double porcentaje { get; set; }
        public string Codi_Esca { get; set; }
        public double tota_ruta1 { get; set; }
        public double tota_ruta2 { get; set; }
        public string Estado { get; set; }
        public string Punto_Venta { get; set; }
        public string tipo_doc { get; set; }
        public string codi_ori_psj { get; set; }
        public string Tipo { get; set; }
        public string per_autoriza { get; set; }
        public string Cod_Cliente { get; set; }
        public string estado_asiento { get; set; }
        public string SEXO { get; set; }
        public string Tipo_Pago { get; set; }
        public string Vale_Remoto { get; set; }
        public string Tipo_Venta { get; set; }
        public string Fecha_viaje { get; set; }
        public string HORA_V { get; set; }
        public string nacionalidad { get; set; }
        public string servicio { get; set; }
        public string Sube_en { get; set; }
        public string Baja_en { get; set; }
        public string Hora_Emb { get; set; }
        public string nivel { get; set; }
        public string Codi_Empresa__ { get; set; }
        public string CODI_SUCURSAL__ { get; set; }
        public string CODI_TERMINAL__ { get; set; }
        public string Codi_Documento__ { get; set; }
        public string NUME_CORRELATIVO__ { get; set; }
        public string fecha_venta__ { get; set; }
        public string Pventa__ { get; set; }
        public string SERIE_BOLETO__ { get; set; }
        public string Sw_IngManual { get; set; }
    }
}