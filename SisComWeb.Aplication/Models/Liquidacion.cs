
namespace SisComWeb.Aplication.Models
{
    public class Liquidacion
    {
        public string Fecha { get; set; }
        public string Empresa { get; set; }
        public int CodiEmpresa { get; set; }
        public string Sucursal { get; set; }
        public int CodiSucursal { get; set; }
        public string PuntoVenta { get; set; }
        public int CodiPuntoVenta { get; set; }
        public string Usuario { get; set; }
        public int CodiUsuario { get; set; }
        public decimal PasIng { get; set; }
        public byte AfectoPasIng { get; set; }
        public decimal VenRem { get; set; }
        public byte AfectoVenRem { get; set; }
        public decimal Venrut { get; set; }
        public byte AfectoVenrut { get; set; }
        public decimal VenEnc { get; set; }
        public byte AfectoVenEnc { get; set; }
        public decimal VenExe { get; set; }
        public byte AfectoVenExe { get; set; }
        public decimal FacLib { get; set; }
        public byte AfectoFacLib { get; set; }
        public decimal GirRec { get; set; }
        public byte AfectoGirRec { get; set; }
        public decimal CobDes { get; set; }
        public byte AfectoCobDes { get; set; }
        public decimal CobDel { get; set; }
        public byte AfectoCobDel { get; set; }
        public decimal IngCaj { get; set; }
        public byte AfectoIngCaj { get; set; }
        public decimal IngDet { get; set; }
        public byte AfectoIngDet { get; set; }
        public decimal TotalAfecto { get; set; }
        public byte AfectoTotalAfecto { get; set; }
        public decimal RemEmi { get; set; }
        public byte AfectoRemEmi { get; set; }
        public decimal BolCre { get; set; }
        public byte AfectoBolCre { get; set; }
        public decimal WebEmi { get; set; }
        public byte AfectoWebEmi { get; set; }
        public decimal RedBus { get; set; }
        public byte AfectoRedBus { get; set; }
        public decimal TieVir { get; set; }
        public byte AfectoTieVir { get; set; }
        public decimal DelEmi { get; set; }
        public byte AfectoDelEmi { get; set; }
        public decimal Ventar { get; set; }
        public byte AfectoVentar { get; set; }
        public decimal Enctar { get; set; }
        public byte AfectoEnctar { get; set; }
        public decimal EgrCaj { get; set; }
        public byte AfectoEgrCaj { get; set; }
        public decimal GirEnt { get; set; }
        public byte AfectoGirEnt { get; set; }
        public decimal BolAnF { get; set; }
        public byte AfectoBolAnF { get; set; }
        public decimal BolAnR { get; set; }
        public byte AfectoBolAnR { get; set; }
        public decimal ValAnR { get; set; }
        public byte AfectoValAnR { get; set; }
        public decimal EncPag { get; set; }
        public byte AfectoEncPag { get; set; }
        public decimal Ctagui { get; set; }
        public byte AfectoCtagui { get; set; }
        public decimal CtaCan { get; set; }
        public byte AfectoCtaCan { get; set; }
        public decimal Notcre { get; set; }
        public byte AfectoNotcre { get; set; }
        public decimal Totdet { get; set; }
        public byte AfectoTotdet { get; set; }
        public decimal Gasrut { get; set; }
        public byte AfectoGasrut { get; set; }
        public decimal TotalInafecto { get; set; }
        public byte AfectoTotalInafecto { get; set; }
        public decimal Total { get; set; }
        public byte AfectoTotal { get; set; }
    }

    public class FiltroLiquidacion
    {
        public string FechaLiquidacion { get; set; }
        public int CodEmpresa { get; set; }
        public int CodSucursal { get; set; }
        public int CodPuntVenta { get; set; }
        public int CodUsuario { get; set; }
        public string tipoLiq { get; set; }
    }
}