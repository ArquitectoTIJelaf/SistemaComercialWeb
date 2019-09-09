using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace SisComWeb.CuadreImpresora
{
    public class Cuadre
    {
        private static readonly string TipoImprimir = ConfigurationManager.AppSettings["tipoImprimir"];
        private static readonly string TipoReimprimir = ConfigurationManager.AppSettings["tipoReimprimir"];

        #region "VENTA"

        public static string WriteText(VentaRealizadaEntity venta, string TipoImpresion)
        {
            StringBuilder texto = new StringBuilder();

            // Impresora Térmica
            if (venta.TipoImpresora == 1)
            {
                texto.AppendLine(SplitStringPreserving(venta.EmpRazSocial, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving("R.U.C. " + venta.EmpRuc, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving(venta.EmpDireccion, 45, "^"));
                texto.AppendLine(SplitStringPreserving("Agencia: " + venta.EmpDirAgencia, 45, "^"));
                texto.AppendLine("|||^" + PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA DE VENTA") + " ELECTRÓNICA", 30));
                texto.AppendLine("|^" + PadBoth(string.Format(venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum), 18));
                texto.AppendLine("");
                texto.AppendLine("^FECHA DE EMISIÓN : " + venta.EmisionFecha + " - " + venta.EmisionHora);
                texto.AppendLine("^NRO. DOCUMENTO   : " + venta.DocNumero);
                texto.AppendLine(SplitStringPreserving("PASAJERO         : " + venta.PasNombreCom, 45, "^", false));
                if (venta.BoletoTipo == "F")
                {
                    texto.AppendLine("^NRO. RUC         : " + venta.PasRuc);
                    texto.AppendLine(SplitStringPreserving("CLIENTE          : " + venta.PasRazSocial, 45, "^", false));
                    texto.AppendLine(SplitStringPreserving("DIRECCION        : " + venta.PasDireccion, 45, "^", false));
                }
                texto.AppendLine("^COD. CAJERO      : " + venta.CajeroCod);
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("^DETALLE DEL SERVICIO");
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("||||^SERVICIO DE TRANSPORTE");
                texto.AppendLine("^ORIGEN           : " + venta.NomOriPas);
                texto.AppendLine("^DESTINO          : " + venta.NomDesPas);
                texto.AppendLine("||||^SERVICIO       : " + venta.NomServicio);
                texto.AppendLine("|||^FECHA VIAJE    : " + venta.FechaViaje);
                texto.AppendLine("|||^HORA DE SALIDA : " + venta.EmbarqueHora);
                texto.AppendLine("|||^ASIENTO        : " + venta.NumeAsiento);
                texto.AppendLine(SplitStringPreserving("DIR.EMBARQUE: " + venta.EmpDirAgencia, 30, "||||^", false));
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("|||^TOTAL" + ("S/ " + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan)).PadLeft(20));
                texto.AppendLine("^SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("^CÓDIGO: " + venta.CodigoX_FE);
                texto.AppendLine(SplitStringPreserving("Autorizado mediante", 43, "^", true));
                texto.AppendLine(SplitStringPreserving("N° " + venta.ResAut_FE + "/SUNAT", 43, "^", true));
                texto.AppendLine(SplitStringPreserving("Representación impresa del comprobante de venta, puede ser consultado en", 43, "^", true));
                texto.AppendLine(SplitStringPreserving(venta.LinkPag_FE, 45, "^", true));
                texto.AppendLine(SplitStringPreserving("Términos y condiciones, visite", 43, "^", true));
                texto.AppendLine(SplitStringPreserving("https://floreshnos.pe/terminos/", 45, "^", true));
                texto.AppendLine(SplitStringPreserving("Debe presentarse 30 minutos antes de la hora de salida.", 45, "^", true));
                texto.AppendLine(SplitStringPreserving("Al abordar el bus debe presentar la representación impresa o digital del comprobante.", 45, "^", true));
                texto.AppendLine(SplitStringPreserving("No se aceptan cambios ni devoluciones, conservar su comprobante ante cualquier eventualidad.", 45, "^", true));
                texto.AppendLine("");
                if (!string.IsNullOrEmpty(venta.PolizaNum))
                {
                    texto.AppendLine(SplitStringPreserving("CUBIERTO " + venta.PolizaNum + " " + venta.PolizaFechaReg + " - " + venta.PolizaFechaVen, 45, "^"));
                    texto.AppendLine("");
                }
                texto.AppendLine(SplitStringPreserving("TIPO DE VENTA: " + NameOfTipoVenta(venta.TipoPago, venta.FlagVenta), 43, "^", true));
                texto.AppendLine("");
                texto.AppendLine(string.Format("@@@{0}|{1}|{2}{3}|{4}|0|{5}|{6}|{7}|{8}",
                                                venta.EmpRuc, venta.DocTipo, venta.BoletoTipo,
                                                venta.BoletoSerie, venta.BoletoNum, venta.PrecioCan,
                                                venta.EmisionFecha, venta.DocTipo, venta.DocNumero));
                if (TipoImpresion == TipoReimprimir)
                {
                    texto.AppendLine("");
                    texto.AppendLine("^FEC. REIMP.: " + DataUtility.ObtenerFechaDelSistema() + " - " + DataUtility.Obtener12HorasDelSistema() + " - " + venta.UsuarioCodigo);
                }
            }
            // Impresora Matricial
            else if (venta.TipoImpresora == 3)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 10CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 80));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA DE VENTA") + " ELECTRONICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 80));
                texto.AppendLine(PadBoth(venta.EmbarqueDir + " - " + venta.EmpDirAgencia, 80));
                texto.AppendLine("FECHA Y HORA DE EMISION".PadRight(25) + (": " + venta.EmisionFecha + " - " + venta.EmisionHora + " - " + "CAJ.: " + venta.CajeroNom).PadRight(65));
                if (venta.BoletoTipo == "F")
                {
                    texto.AppendLine("NRO. RUC".PadRight(25) + (": " + venta.PasRuc).PadRight(65));
                    texto.AppendLine("CLIENTE".PadRight(25) + (": " + venta.PasRazSocial).PadRight(65));
                    texto.AppendLine("DIRECCION".PadRight(25) + (": " + venta.PasDireccion).PadRight(65));
                }
                texto.AppendLine("DOC. CLIENTE".PadRight(25) + (": " + venta.DocNumero).PadRight(65));
                texto.AppendLine("PASAJERO".PadRight(25) + (": " + venta.PasNombreCom).PadRight(65));
                texto.AppendLine("CONDICION DE VENTA".PadRight(25) + (": " + venta.NomTipVenta).PadRight(65));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("    DESCRIPCION DEL SERVICIO".PadRight(30) + "TOTAL".PadLeft(50));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine(("|||SER. DE TRAN. DE LA RUTA " + venta.NomOriPas + " - " + venta.NomDesPas + " / " + "SERVICIO:").PadRight(70) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(10));
                texto.AppendLine(venta.NomServicio + " / " + "NRO. ASIENTO: " + venta.NumeAsiento + " / " + "PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine(" / " + "DNI: " + venta.DocNumero + " / " + "FECHA VIAJE: " + venta.FechaViaje + " / " + "HORA VIAJE: " + venta.EmbarqueHora);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(39) + PadBoth("OP.INAFECTA", 25) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(10));
                texto.AppendLine("Autorizado mediante: ".PadRight(40) + PadBoth("OP. GRAVADA", 25) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(10));
                texto.AppendLine("N° " + venta.ResAut_FE + "/Sunat ".PadRight(40) + PadBoth("OP. EXONERADA", 25) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(10));
                texto.AppendLine("I.G.V. 18%".PadLeft(65) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(10));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(65) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(10));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("Representación impresa de la Factura Electrónica");
                texto.AppendLine("  Consulte el documento en: " + venta.LinkPag_FE);
                texto.AppendLine(SplitStringPreserving("Al recibir el presente documento, acepto todos los términos y/o condiciones del contratado del servicio de transporte publicados en los letreros o en los banners y detallados en la página.", 80, "", false));
                texto.AppendLine("");
                if (!string.IsNullOrEmpty(venta.PolizaNum))
                {
                    texto.AppendLine("CUBIERTO " + venta.PolizaNum + " " + venta.PolizaFechaReg + " - " + venta.PolizaFechaVen);
                    texto.AppendLine("");
                }
                texto.AppendLine(new string('-', 25));
                texto.AppendLine("  USUARIO CONFORME");
                texto.AppendLine(".");
                if (TipoImpresion == TipoReimprimir)
                {
                    texto.AppendLine("");
                    texto.AppendLine("FECHA DE REIMPRESION: " + DataUtility.ObtenerFechaDelSistema() + " - " + DataUtility.Obtener12HorasDelSistema() + " - " + venta.UsuarioCodigo);
                }
            }
            // Impresora Matricial (Formato reducido)
            else if (venta.TipoImpresora == 4)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 17CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 65));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA DE VENTA") + " ELECTRONICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 65));
                texto.AppendLine(PadBoth(venta.EmbarqueDir + " - " + venta.EmpDirAgencia, 65));
                texto.AppendLine("EMITIDO".PadRight(25) + (": " + venta.EmisionFecha + " - " + venta.EmisionHora + " - " + "CAJ.: " + venta.CajeroNom).PadRight(40));
                if (venta.BoletoTipo == "F")
                {
                    texto.AppendLine("NRO. RUC".PadRight(25) + (": " + venta.PasRuc + " - " + venta.PasRazSocial).PadRight(40));
                    texto.AppendLine("DIRECCION".PadRight(25) + (": " + venta.PasDireccion).PadRight(40));
                }
                texto.AppendLine("CLIENTE".PadRight(25) + (": " + venta.DocNumero + " - " + venta.PasNombreCom).PadRight(40));
                texto.AppendLine("COD. VENTA".PadRight(25) + (": " + venta.NomTipVenta).PadRight(40));
                texto.AppendLine(new string('-', 70));
                texto.AppendLine("    DESCRIPCION DEL SERVICIO".PadRight(25));
                texto.AppendLine(new string('-', 70));
                texto.AppendLine(("SERVICIO DE TRANSPORTE DE LA RUTA " + venta.NomOriPas + " - " + venta.NomDesPas + " / " + "SERVICIO:").PadRight(56));
                texto.AppendLine(venta.NomServicio + " / " + "NRO. ASIENTO: " + venta.NumeAsiento + " / " + "PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine(" / " + "DNI: " + venta.DocNumero + " / " + "FECHA VIAJE: " + venta.FechaViaje + " / " + "HORA VIAJE: " + venta.EmbarqueHora);
                texto.AppendLine(new string('-', 70));
                texto.AppendLine("SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 70));
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(32) + PadBoth("OP.INAFECTA", 20) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(8));
                texto.AppendLine("Autorizado mediante: ".PadRight(32) + PadBoth("OP. GRAVADA", 20) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(8));
                texto.AppendLine("N° " + venta.ResAut_FE + "/Sunat ".PadRight(32) + PadBoth("I.G.V. 18%", 20) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(8));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(52) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(8));
                texto.AppendLine(new string('-', 70));
                texto.AppendLine(SplitStringPreserving("  Consulte el documento en: " + venta.LinkPag_FE, 65, "", false));
                texto.AppendLine("Representación impresa de la Factura Electrónica");
                texto.AppendLine("");
                if (!string.IsNullOrEmpty(venta.PolizaNum))
                {
                    texto.AppendLine("CUBIERTO " + venta.PolizaNum + " " + venta.PolizaFechaReg + " - " + venta.PolizaFechaVen);
                    texto.AppendLine("");
                }
                texto.AppendLine(".");
                if (TipoImpresion == TipoReimprimir)
                {
                    texto.AppendLine("");
                    texto.AppendLine("FECHA DE REIMPRESION: " + DataUtility.ObtenerFechaDelSistema() + " - " + DataUtility.Obtener12HorasDelSistema() + " - " + venta.UsuarioCodigo);
                }
            }

            texto.AppendLine(" ");

            byte[] encodedText = Encoding.Default.GetBytes(texto.ToString());
            var boletoBase64 = Convert.ToBase64String(encodedText);

            return boletoBase64;
        }

        public static string WriteTextCopy(VentaRealizadaEntity venta, string TipoImpresion)
        {
            StringBuilder texto = new StringBuilder();

            // Impresora Térmica
            if (venta.TipoImpresora == 1)
            {
                texto.AppendLine(SplitStringPreserving(venta.EmpRazSocial, 30, "||||^"));
                texto.AppendLine(SplitStringPreserving("R.U.C. " + venta.EmpRuc, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving(venta.EmpDireccion, 45, "^"));
                texto.AppendLine(SplitStringPreserving("Agencia: " + venta.EmpDirAgencia, 45, "^"));
                texto.AppendLine("||^" + PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA DE VENTA") + " ELECTRÓNICA", 30));
                texto.AppendLine("|^" + PadBoth(string.Format(venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum), 18));
                texto.AppendLine("");
                texto.AppendLine("^FECHA DE EMISIÓN : " + venta.EmisionFecha + " - " + venta.EmisionHora);
                if (venta.BoletoTipo == "F")
                {
                    texto.AppendLine("^NRO. RUC         : " + venta.PasRuc);
                    texto.AppendLine(SplitStringPreserving("CLIENTE          : " + venta.PasRazSocial, 45, "^", false));
                    texto.AppendLine(SplitStringPreserving("DIRECCION        : " + venta.PasDireccion, 45, "^", false));
                }
                texto.AppendLine("^COD. CAJERO      : " + venta.CajeroCod);
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("");
                texto.AppendLine("^TIPO VENTA: " + venta.NomTipVenta);
                texto.AppendLine(new string('-', 42));
                texto.AppendLine(" # DESCRIPCIÓN                      TOTAL");
                texto.AppendLine(new string('-', 42));
                texto.AppendLine(" 1 SERVICIO DE TRANSPORTE EN LA     " + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan));
                texto.AppendLine(SplitStringPreserving("RUTA: " + venta.NomOriPas + " - " + venta.NomDesPas, 40, "   ", false));
                texto.AppendLine("   SERVICIO: " + venta.NomServicio);
                texto.AppendLine("   NRO ASIENTO: " + venta.NumeAsiento);
                texto.AppendLine(SplitStringPreserving("PASAJERO: " + venta.PasNombreCom, 40, "   ", false));
                texto.AppendLine("   DNI: " + venta.DocNumero);
                texto.AppendLine("   FECHA VIAJE: " + venta.FechaViaje);
                texto.AppendLine("   HORA EMBARQUE: " + venta.EmbarqueHora);
                texto.AppendLine(SplitStringPreserving("LUGAR EMBARQUE: " + venta.EmbarqueDir, 40, "   ", false));
                texto.AppendLine(SplitStringPreserving("DIR. EMBARQUE: " + venta.EmpDirAgencia, 40, "   ", false));
                texto.AppendLine("");
                if (TipoImpresion == TipoReimprimir)
                {
                    texto.AppendLine("^FEC. REIMP.: " + DataUtility.ObtenerFechaDelSistema() + " - " + DataUtility.Obtener12HorasDelSistema() + " - " + venta.UsuarioCodigo);
                    texto.AppendLine("");
                }
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("");
                texto.AppendLine(PadBoth("**** CONTROL INTERNO *****", 45));
                texto.AppendLine("");
                texto.AppendLine("");
                texto.AppendLine("");
                texto.AppendLine("");
                texto.AppendLine("");
                texto.AppendLine("");
                texto.AppendLine("");
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("^" + PadBoth("USUARIO CONFORME", 45));
            }
            // Impresora Matricial
            else if (venta.TipoImpresora == 3)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 10CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 80));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA DE VENTA") + " ELECTRONICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 80));
                texto.AppendLine(PadBoth(venta.EmbarqueDir + " - " + venta.EmpDirAgencia, 80));
                texto.AppendLine("FECHA Y HORA DE EMISION".PadRight(25) + (": " + venta.EmisionFecha + " - " + venta.EmisionHora + " - " + "CAJ.: " + venta.CajeroNom).PadRight(65));
                if (venta.BoletoTipo == "F")
                {
                    texto.AppendLine("NRO. RUC".PadRight(25) + (": " + venta.PasRuc).PadRight(65));
                    texto.AppendLine("CLIENTE".PadRight(25) + (": " + venta.PasRazSocial).PadRight(65));
                    texto.AppendLine("DIRECCION".PadRight(25) + (": " + venta.PasDireccion).PadRight(65));
                }
                texto.AppendLine("DOC. CLIENTE".PadRight(25) + (": " + venta.DocNumero).PadRight(65));
                texto.AppendLine("PASAJERO".PadRight(25) + (": " + venta.PasNombreCom).PadRight(65));
                texto.AppendLine("CONDICION DE VENTA".PadRight(25) + (": " + venta.NomTipVenta).PadRight(65));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("    DESCRIPCION DEL SERVICIO".PadRight(30) + "TOTAL".PadLeft(50));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine(("|||SER. DE TRAN. DE LA RUTA " + venta.NomOriPas + " - " + venta.NomDesPas + " / " + "SERVICIO:").PadRight(70) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(10));
                texto.AppendLine(venta.NomServicio + " / " + "NRO. ASIENTO: " + venta.NumeAsiento + " / " + "PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine(" / " + "DNI: " + venta.DocNumero + " / " + "FECHA VIAJE: " + venta.FechaViaje + " / " + "HORA VIAJE: " + venta.EmbarqueHora);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(39) + PadBoth("OP.INAFECTA", 25) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(10));
                texto.AppendLine("Autorizado mediante: ".PadRight(40) + PadBoth("OP. GRAVADA", 25) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(10));
                texto.AppendLine("N° " + venta.ResAut_FE + "/Sunat ".PadRight(40) + PadBoth("OP. EXONERADA", 25) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(10));
                texto.AppendLine("I.G.V. 18%".PadLeft(65) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(10));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(65) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(10));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("Representación impresa de la Factura Electrónica");
                texto.AppendLine("  Consulte el documento en: " + venta.LinkPag_FE);
                texto.AppendLine(SplitStringPreserving("Al recibir el presente documento, acepto todos los términos y/o condiciones del contratado del servicio de transporte publicados en los letreros o en los banners y detallados en la página.", 80, "", false));
                texto.AppendLine("");
                if (!string.IsNullOrEmpty(venta.PolizaNum))
                {
                    texto.AppendLine("CUBIERTO " + venta.PolizaNum + " " + venta.PolizaFechaReg + " - " + venta.PolizaFechaVen);
                    texto.AppendLine("");
                }
                texto.AppendLine(new string('-', 25));
                texto.AppendLine("  USUARIO CONFORME");
                texto.AppendLine(".");
                if (TipoImpresion == TipoReimprimir)
                {
                    texto.AppendLine("");
                    texto.AppendLine("FECHA DE REIMPRESION: " + DataUtility.ObtenerFechaDelSistema() + " - " + DataUtility.Obtener12HorasDelSistema() + " - " + venta.UsuarioCodigo);
                }
            }
            // Impresora Matricial (Formato reducido)
            else if (venta.TipoImpresora == 4)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 17CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 65));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA DE VENTA") + " ELECTRONICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 65));
                texto.AppendLine(PadBoth(venta.EmbarqueDir + " - " + venta.EmpDirAgencia, 65));
                texto.AppendLine("EMITIDO".PadRight(25) + (": " + venta.EmisionFecha + " - " + venta.EmisionHora + " - " + "CAJ.: " + venta.CajeroNom).PadRight(40));
                if (venta.BoletoTipo == "F")
                {
                    texto.AppendLine("NRO. RUC".PadRight(25) + (": " + venta.PasRuc + " - " + venta.PasRazSocial).PadRight(40));
                    texto.AppendLine("DIRECCION".PadRight(25) + (": " + venta.PasDireccion).PadRight(40));
                }
                texto.AppendLine("CLIENTE".PadRight(25) + (": " + venta.DocNumero + " - " + venta.PasNombreCom).PadRight(40));
                texto.AppendLine("COD. VENTA".PadRight(25) + (": " + venta.NomTipVenta).PadRight(40));
                texto.AppendLine(new string('-', 70));
                texto.AppendLine("    DESCRIPCION DEL SERVICIO".PadRight(25));
                texto.AppendLine(new string('-', 70));
                texto.AppendLine(("SERVICIO DE TRANSPORTE DE LA RUTA " + venta.NomOriPas + " - " + venta.NomDesPas + " / " + "SERVICIO:").PadRight(56));
                texto.AppendLine(venta.NomServicio + " / " + "NRO. ASIENTO: " + venta.NumeAsiento + " / " + "PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine(" / " + "DNI: " + venta.DocNumero + " / " + "FECHA VIAJE: " + venta.FechaViaje + " / " + "HORA VIAJE: " + venta.EmbarqueHora);
                texto.AppendLine(new string('-', 70));
                texto.AppendLine("SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 70));
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(32) + PadBoth("OP.INAFECTA", 20) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(8));
                texto.AppendLine("Autorizado mediante: ".PadRight(32) + PadBoth("OP. GRAVADA", 20) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(8));
                texto.AppendLine("N° " + venta.ResAut_FE + "/Sunat ".PadRight(32) + PadBoth("I.G.V. 18%", 20) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(0M).PadLeft(8));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(52) + "S/".PadLeft(5) + DataUtility.ConvertDecimalToStringWithTwoDecimals(venta.PrecioCan).PadLeft(8));
                texto.AppendLine(new string('-', 70));
                texto.AppendLine(SplitStringPreserving("  Consulte el documento en: " + venta.LinkPag_FE, 65, "", false));
                texto.AppendLine("Representación impresa de la Factura Electrónica");
                texto.AppendLine("");
                if (!string.IsNullOrEmpty(venta.PolizaNum))
                {
                    texto.AppendLine("CUBIERTO " + venta.PolizaNum + " " + venta.PolizaFechaReg + " - " + venta.PolizaFechaVen);
                    texto.AppendLine("");
                }
                texto.AppendLine(".");
                if (TipoImpresion == TipoReimprimir)
                {
                    texto.AppendLine("");
                    texto.AppendLine("FECHA DE REIMPRESION: " + DataUtility.ObtenerFechaDelSistema() + " - " + DataUtility.Obtener12HorasDelSistema() + " - " + venta.UsuarioCodigo);
                }
            }

            texto.AppendLine(" ");

            byte[] encodedText = Encoding.Default.GetBytes(texto.ToString());
            var boletoBase64 = Convert.ToBase64String(encodedText);

            return boletoBase64;
        }

        private static string NameOfTipoVenta(string TipoPago, string FlagVenta)
        {
            var retorno = "EFECTIVO";
            if (FlagVenta == "V")
            {
                if (TipoPago == "03")
                    retorno = "TARJETA DE CREDITO";
            }
            else
            {
                if (FlagVenta == "1")
                    retorno = "CREDITO";
                else if (FlagVenta == "I")
                    retorno = "BOLETO REMOTO";
                else if (FlagVenta == "O")
                    retorno = "REINTEGRO";
                else if (FlagVenta == "7")
                    retorno = "PASE DE CORTESIA";
                else if (FlagVenta == "8")
                    retorno = "CANJE PROMOCIONAL";
            }
            return retorno;
        }
        #endregion

        #region "LIQUIDACION"

        public static string WriteLiquidacion(LiquidacionEntity liquidacion)
        {
            StringBuilder texto = new StringBuilder();
            texto.AppendLine(SplitStringPreserving("EMPRESA :" + liquidacion.Empresa, 30, "||||^", false));
            texto.AppendLine(" ");
            texto.AppendLine(SplitStringPreserving("LIQUIDACIÓN DE CAJERO " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), 30, "|||^"));
            texto.AppendLine(" ");
            texto.AppendLine(SplitStringPreserving("SUCURSAL:" + liquidacion.Sucursal, 30, "||||^", false));
            texto.AppendLine(SplitStringPreserving("P.VENTA :" + liquidacion.PuntoVenta, 30, "||||^", false));
            texto.AppendLine(SplitStringPreserving("USUARIO :" + liquidacion.Usuario, 30, "||||^", false));
            texto.AppendLine(new string('-', 42));
            texto.AppendLine(SplitStringPreserving("INGRESOS", 30, "|||^"));
            texto.AppendLine(" ");
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "VENTA DE PASAJES", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.PasIng)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "ING. VENTA REMOTA", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.VenRem)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "VENTA DE RUTAS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.Venrut)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "ENCOMIENDAS Y GIROS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.VenEnc)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "VENTA DE EXCESOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.VenExe)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "FACTURA LIBRE", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.FacLib)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "GIROS RECIBIDOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.GirRec)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "COBRANZAS DESTINO", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.CobDes)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "COBRANZAS DELIVERY", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.CobDel)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "INGRESOS DE CAJA", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.IngCaj)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "INGRESO DE DETRACCIÓN", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.IngDet)));
            texto.AppendLine(String.Format("{0,-24}{1,8}", "|||^" + "TOTAL INGRESOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.TotalAfecto)));
            texto.AppendLine(new string('-', 42));
            texto.AppendLine(SplitStringPreserving("EGRESOS", 30, "|||^"));
            texto.AppendLine(" ");
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "REMOTOS EMITIDOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.RemEmi)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "BOLETOS CREDITO", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.BolCre)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "WEB EMITIDOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.WebEmi)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "RED BUS EMITIDOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.RedBus)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "TIENDA VIRTUAL", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.TieVir)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "DELIVERY EMITIDOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.DelEmi)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "VENTA TARJETA PASAJES", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.Ventar)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "VTA. TARJETA ENCOMIENDAS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.Enctar)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "EGRESOS DE CAJA", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.EgrCaj)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "GIROS ENTREGADOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.GirEnt)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "BOL. ANUL. OTRA FECHA", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.BolAnF)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "VALES ANULADOS REMOTOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.ValAnR)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "POR PAGAR ENCOMIENDAS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.EncPag)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "CUENTA CTE GUIAS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.Ctagui)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "CUENTA CTE CAN", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.CtaCan)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "NOTAS CRED. EMITIDAS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.Notcre)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "TOTAL DE DETRACCIÓN", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.Totdet)));
            texto.AppendLine(String.Format("{0,-34}{1,8}", "||||^" + "GASTOS RUTA", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.Gasrut)));
            texto.AppendLine(String.Format("{0,-24}{1,8}", "|||^" + "TOTAL EGRESOS", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.TotalInafecto)));
            texto.AppendLine(new string('-', 42));
            texto.AppendLine(String.Format("{0,-24}{1,8}", "|||^" + "TOTAL (ING - EGR)", string.Format(CultureInfo.InvariantCulture, "{0:f2}", liquidacion.Total)));
            texto.AppendLine(new string('-', 42));
            texto.AppendLine(" ");
            texto.AppendLine(" ");
            texto.AppendLine(" ");
            texto.AppendLine(" ");
            texto.AppendLine(" ");
            texto.AppendLine(" ");
            texto.AppendLine(new string('-', 42));
            texto.AppendLine(SplitStringPreserving("CONFORMIDAD DEL CAJERO " + liquidacion.Usuario, 42, "||||^"));
            texto.AppendLine(" ");
            texto.AppendLine(" ");
            byte[] encodedText = Encoding.Default.GetBytes(texto.ToString());
            var boletoBase64 = Convert.ToBase64String(encodedText);

            return boletoBase64;
        }
        #endregion

        public static string SplitStringPreserving(string str, int length, string starOFline = "", bool center = true)
        {
            StringBuilder sb = new StringBuilder();
            string[] words = str.Split(' ');
            var parts = new Dictionary<int, string>();
            string part = string.Empty;
            int partCounter = 0;
            foreach (var word in words)
            {
                if (part.Length + word.Length < length)
                {
                    part += string.IsNullOrEmpty(part) ? word : " " + word;
                }
                else
                {
                    parts.Add(partCounter, part);
                    part = word;
                    partCounter++;
                }
            }
            parts.Add(partCounter, part);
            foreach (var item in parts)
            {
                if (center)
                    sb.AppendLine(starOFline + PadBoth(item.Value, length));
                else
                    sb.AppendLine(starOFline + item.Value);
            }

            return sb.ToString().Substring(0, sb.Length - 2);
        }

        public static string PadBoth(string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }
}
