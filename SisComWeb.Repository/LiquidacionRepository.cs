using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using System.Data;

namespace SisComWeb.Repository
{
    public class LiquidacionRepository
    {
        public static bool Borrar(LiquidacionRequest filtro)
        {
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "LiquidacionSP.[Usp_Tb_Temp_Liquidacion_GeneralWeb_Delete]";
                db.AddParameter("@CodInt", DbType.Int16, ParameterDirection.Input, filtro.CodInterno);
                db.Execute();
            }
            return true;
        }

        public static bool Poblar(LiquidacionRequest filtro)
        {
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "LiquidacionSP.[Usp_Tb_Temp_Liquidacion_GeneralWeb_All]";
                db.AddParameter("@Fecha", DbType.Date, ParameterDirection.Input, filtro.FechaLiquidacion);
                db.AddParameter("@Empresa", DbType.Int16, ParameterDirection.Input, filtro.CodEmpresa);
                db.AddParameter("@Suc", DbType.Int32, ParameterDirection.Input, filtro.CodSucursal);
                db.AddParameter("@Puntov", DbType.Int32, ParameterDirection.Input, filtro.CodPuntVenta);
                db.AddParameter("@Usuario", DbType.Int32, ParameterDirection.Input, filtro.CodUsuario);
                db.AddParameter("@codigointerno", DbType.Int16, ParameterDirection.Input, filtro.CodInterno);
                db.AddParameter("@TipoProc", DbType.Int16, ParameterDirection.Input, filtro.TipoProc);
                db.AddParameter("@tipoLiq", DbType.String, ParameterDirection.Input, filtro.tipoLiq);
                db.Execute();
            }
            return true;
        }

        public static LiquidacionEntity Data(LiquidacionRequest filtro)
        {
            var objeto = new LiquidacionEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "LiquidacionSP.[Usp_Tb_Temp_Liquidacion_GeneralWeb_Select_All]";
                db.AddParameter("@CodInt", DbType.Int16, ParameterDirection.Input, filtro.CodInterno);
                db.AddParameter("@tipoLiq", DbType.String, ParameterDirection.Input, filtro.tipoLiq);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        objeto = new LiquidacionEntity
                        {
                            Fecha = Reader.GetDateStringValue(drlector, "Fecha"),
                            Empresa = Reader.GetStringValue(drlector, "Empresa"),
                            CodiEmpresa = Reader.GetIntValue(drlector, "CodEmpresa"),
                            Sucursal = Reader.GetStringValue(drlector, "Sucursal"),
                            CodiSucursal = Reader.GetIntValue(drlector, "CodSucursal"),
                            PuntoVenta = Reader.GetStringValue(drlector, "Punto_venta"),
                            CodiPuntoVenta = Reader.GetIntValue(drlector, "CodPuntVenta"),
                            Usuario = Reader.GetStringValue(drlector, "Usuario"),
                            CodiUsuario = Reader.GetIntValue(drlector, "CodUsuario"),
                            PasIng = Reader.GetDecimalValue(drlector, "Pas_Ing"),
                            AfectoPasIng = 1,
                            VenRem = Reader.GetDecimalValue(drlector, "Ven_Rem"),
                            AfectoVenRem = 1,
                            Venrut = Reader.GetDecimalValue(drlector, "Ven_rut"),
                            AfectoVenrut = 1,
                            VenEnc = Reader.GetDecimalValue(drlector, "Ven_Enc"),
                            AfectoVenEnc = 1,
                            VenExe = Reader.GetDecimalValue(drlector, "Ven_Exe"),
                            AfectoVenExe = 1,
                            FacLib = Reader.GetDecimalValue(drlector, "Fac_Lib"),
                            AfectoFacLib = 1,
                            GirRec = Reader.GetDecimalValue(drlector, "Gir_Rec"),
                            AfectoGirRec = 1,
                            CobDes = Reader.GetDecimalValue(drlector, "Cob_Des"),
                            AfectoCobDes = 1,
                            CobDel = Reader.GetDecimalValue(drlector, "Cob_Del"),
                            AfectoCobDel = 1,
                            IngCaj = Reader.GetDecimalValue(drlector, "Ing_Caj"),
                            AfectoIngCaj = 1,
                            IngDet = Reader.GetDecimalValue(drlector, "Ing_Det"),
                            AfectoIngDet = 1,
                            TotalAfecto = Reader.GetDecimalValue(drlector, "TOTALI"),
                            AfectoTotalAfecto = 1,
                            RemEmi = Reader.GetDecimalValue(drlector, "Rem_Emi"),
                            AfectoRemEmi = 0,
                            BolCre = Reader.GetDecimalValue(drlector, "Bol_Cre"),
                            AfectoBolCre = 0,
                            WebEmi = Reader.GetDecimalValue(drlector, "Web_Emi"),
                            AfectoWebEmi = 0,
                            RedBus = Reader.GetDecimalValue(drlector, "Red_Bus"),
                            AfectoRedBus = 0,
                            TieVir = Reader.GetDecimalValue(drlector, "Tie_Vir"),
                            AfectoTieVir = 0,
                            DelEmi = Reader.GetDecimalValue(drlector, "Del_Emi"),
                            AfectoDelEmi = 0,
                            Ventar = Reader.GetDecimalValue(drlector, "Ven_tar"),
                            AfectoVentar = 0,
                            Enctar = Reader.GetDecimalValue(drlector, "Enc_tar"),
                            AfectoEnctar = 0,
                            EgrCaj = Reader.GetDecimalValue(drlector, "Egr_Caj"),
                            AfectoEgrCaj = 0,
                            GirEnt = Reader.GetDecimalValue(drlector, "Gir_Ent"),
                            AfectoGirEnt = 0,
                            BolAnF = Reader.GetDecimalValue(drlector, "Bol_AnF"),
                            AfectoBolAnF = 0,
                            BolAnR = Reader.GetDecimalValue(drlector, "Bol_AnR"),
                            AfectoBolAnR = 0,
                            ValAnR = Reader.GetDecimalValue(drlector, "Val_AnR"),
                            AfectoValAnR = 0,
                            EncPag = Reader.GetDecimalValue(drlector, "Enc_Pag"),
                            AfectoEncPag = 0,
                            Ctagui = Reader.GetDecimalValue(drlector, "Cta_gui"),
                            AfectoCtagui = 0,
                            CtaCan = Reader.GetDecimalValue(drlector, "Cta_Can"),
                            AfectoCtaCan = 0,
                            Notcre = Reader.GetDecimalValue(drlector, "Not_cre"),
                            AfectoNotcre = 0,
                            Totdet = Reader.GetDecimalValue(drlector, "Tot_det"),
                            AfectoTotdet = 0,
                            Gasrut = Reader.GetDecimalValue(drlector, "Gas_rut"),
                            AfectoGasrut = 0,
                            TotalInafecto = Reader.GetDecimalValue(drlector, "TOTALS"),
                            AfectoTotalInafecto = 0,
                            Total = Reader.GetDecimalValue(drlector, "TOTAL"),
                            AfectoTotal = 0
                        };
                        break;
                    }
                }
            }

            return objeto;
        }
    }
}
