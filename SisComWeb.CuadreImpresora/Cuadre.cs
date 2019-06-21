using SisComWeb.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace SisComWeb.CuadreImpresora
{
    public class Cuadre
    {
        private static readonly string TipoImprimir = ConfigurationManager.AppSettings["tipoImprimir"].ToString();
        private static readonly string TipoReimprimir = ConfigurationManager.AppSettings["tipoReimprimir"].ToString();

        public static string WriteText(VentaRealizadaEntity venta, string TipoImpresion)
        {
            StringBuilder texto = new StringBuilder();
            // Impresora Térmica
            if (venta.TipImpresora == 1)
            {
                texto.AppendLine(SplitStringPreserving(venta.EmpRazSocial, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving("R.U.C. " + venta.EmpRuc, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving(venta.EmpDireccion, 45, "^"));
                texto.AppendLine(SplitStringPreserving("Agencia: " + venta.EmpDirAgencia, 45, "^"));
                texto.AppendLine("|||^" + PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " ELECTRÓNICA", 30));
                texto.AppendLine("|^" + PadBoth(string.Format(venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum), 18));
                texto.AppendLine("");
                texto.AppendLine("^FECHA DE EMISIÓN : " + venta.EmisionFecha + " - " + venta.EmisionHora);
                texto.AppendLine("^NRO. DOCUMENTO   : " + venta.DocNumero);
                texto.AppendLine("^PASAJERO         : " + venta.PasNombreCom);
                if (venta.BoletoTipo == "F")
                {
                    texto.AppendLine("^NRO. RUC         : " + venta.PasRuc);
                    texto.AppendLine("^CLIENTE          : " + venta.PasRazSocial);
                    texto.AppendLine("^DIRECCION        : " + venta.PasDireccion);
                }
                texto.AppendLine("^COD. CAJERO      : " + venta.CajeroCod);
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("^DETALLE DEL SERVICIO");
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("||||^SERVICIO DE TRANSPORTE");
                texto.AppendLine("^ORIGEN           : " + venta.NomOriPas);
                texto.AppendLine("^DESTINO          : " + venta.NomDesPas);
                texto.AppendLine("||||^SERVICIO      : " + venta.NomServicio);
                texto.AppendLine("|||^FECHA VIAJE    : " + venta.FechaViaje);
                texto.AppendLine("|||^HORA DE SALIDA : " + venta.EmbarqueHora);
                texto.AppendLine("|||^ASIENTO        : " + venta.NumeAsiento);
                texto.AppendLine(SplitStringPreserving("DIR.EMBARQUE: " + venta.EmbarqueDir, 30, "||||^", false));
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("|||^TOTAL" + ("S/ " + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture)).PadLeft(20));
                texto.AppendLine("^SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("^CÓDIGO: " + venta.CodigoX_FE);
                texto.AppendLine(PadBoth("^Autorizado mediante", 45));
                texto.AppendLine(PadBoth("^N° XXXXX/SUNAT", 45));
                texto.AppendLine(PadBoth("^Representación impresa del comprobante.", 45));
                texto.AppendLine(PadBoth("^Términos y condiciones, visite", 45));
                texto.AppendLine(SplitStringPreserving(venta.LinkPag_FE, 45, "^", true));
                texto.AppendLine(SplitStringPreserving("Debe presentarse 30 minutos antes de la hora de salida.", 45, "^", true));
                texto.AppendLine(SplitStringPreserving("Al abordar el bus debe presentar la representación impresa o digital del comprobante.", 45, "^", true));
                texto.AppendLine(SplitStringPreserving("No se aceptan cambios ni devoluciones, conservar su comprobante ante cualquier eventualidad.", 45, "^", true));
                texto.AppendLine("");
                if (!string.IsNullOrEmpty(venta.PolizaNum))
                {
                    texto.AppendLine(SplitStringPreserving("CUBIERTO " + venta.PolizaNum + " " + venta.PolizaFechaReg + " - " + venta.PolizaFechaVen, 45, "^"));
                    texto.AppendLine("");
                }
                texto.AppendLine(string.Format("@@@{0}|{1}|{2}{3}|{4}|0|{5}|{6}|{7}|{8}",
                                                venta.EmpRuc, venta.DocTipo, venta.BoletoTipo,
                                                venta.BoletoSerie, venta.BoletoNum, venta.PrecioCan,
                                                venta.EmisionFecha, venta.DocTipo, venta.DocNumero));
                if (TipoImpresion == TipoReimprimir)
                {
                    texto.AppendLine("");
                    texto.AppendLine("^FEC. REIMP.: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture) + " - " + venta.CajeroCod);
                }
            }
            // Impresora Matricial
            else if (venta.TipImpresora == 3)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 10CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 80));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " ELECTRONICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 80));
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
                texto.AppendLine(("|||SER. DE TRAN. DE LA RUTA " + venta.NomOriPas + " - " + venta.NomDesPas + " / " + "SERVICIO:").PadRight(70) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine(venta.NomServicio + " / " + "NRO. ASIENTO: " + venta.NumeAsiento + " / " + "PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine(" / " + "DNI: " + venta.DocNumero + " / " + "FECHA VIAJE: " + venta.FechaViaje + " / " + "HORA VIAJE: " + venta.EmbarqueHora);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(39) + PadBoth("OP.INAFECTA", 25) + "S/".PadLeft(5) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("Autorizado mediante: ".PadRight(40) + PadBoth("OP. GRAVADA", 25) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("N° XXXXX/Sunat ".PadRight(40) + PadBoth("OP. EXONERADA", 25) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("I.G.V. 18%".PadLeft(65) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(65) + "S/".PadLeft(5) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
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
                    texto.AppendLine("FECHA DE REIMPRESION: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture) + " - " + venta.CajeroCod);
                }
            }
            // Impresora Matricial (Formato reducido)
            else if (venta.TipImpresora == 4)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 17CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 65));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " ELECTRONICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 65));
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
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(32) + PadBoth("OP.INAFECTA", 20) + "S/".PadLeft(5) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(8));
                texto.AppendLine("Autorizado mediante: ".PadRight(32) + PadBoth("OP. GRAVADA", 20) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(8));
                texto.AppendLine("N° XXXXX/Sunat ".PadRight(32) + PadBoth("I.G.V. 18%", 20) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(8));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(52) + "S/".PadLeft(5) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(8));
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
                    texto.AppendLine("FECHA DE REIMPRESION: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture) + " - " + venta.CajeroCod);
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
            if (venta.TipImpresora == 1)
            {
                texto.AppendLine(SplitStringPreserving(venta.EmpRazSocial, 30, "||||^"));
                texto.AppendLine(SplitStringPreserving("R.U.C. " + venta.EmpRuc, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving(venta.EmpDireccion, 45, "^"));
                texto.AppendLine(SplitStringPreserving("Agencia: " + venta.EmpDirAgencia, 45, "^"));
                texto.AppendLine("||^" + PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " ELECTRÓNICA", 30));
                texto.AppendLine("|^" + PadBoth(string.Format(venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum), 18));
                texto.AppendLine("");
                texto.AppendLine("^FECHA DE EMISIÓN : " + venta.EmisionFecha + " - " + venta.EmisionHora);
                if (venta.BoletoTipo == "F")
                {
                    texto.AppendLine("^NRO. RUC         : " + venta.PasRuc);
                    texto.AppendLine("^CLIENTE          : " + venta.PasRazSocial);
                    texto.AppendLine("^DIRECCION        : " + venta.PasDireccion);
                }
                texto.AppendLine("^COD. CAJERO      : " + venta.CajeroCod);
                texto.AppendLine(new string('-', 42));
                texto.AppendLine("");
                texto.AppendLine("^TIPO VENTA: " + venta.NomTipVenta);
                texto.AppendLine(new string('-', 42));
                texto.AppendLine(" # DESCRIPCIÓN                      TOTAL");
                texto.AppendLine(new string('-', 42));
                texto.AppendLine(" 1 SERVICIO DE TRANSPORTE EN LA     " + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture));
                texto.AppendLine("   RUTA: " + venta.NomOriPas + " - " + venta.NomDesPas);
                texto.AppendLine("   SERVICIO: " + venta.NomServicio);
                texto.AppendLine("   NRO ASIENTO: " + venta.NumeAsiento);
                texto.AppendLine("   PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine("   DNI: " + venta.DocNumero);
                texto.AppendLine("   FECHA VIAJE: " + venta.FechaViaje);
                texto.AppendLine("   HORA EMBARQUE: " + venta.EmbarqueHora);
                texto.AppendLine(SplitStringPreserving("LUGAR EMBARQUE: " + venta.EmbarqueDir, 40, "   ", false));
                texto.AppendLine(SplitStringPreserving("DIR. EMBARQUE: " + venta.EmbarqueDirAgencia, 40, "   ", false));
                texto.AppendLine("");
                if (TipoImpresion == TipoReimprimir)
                {
                    texto.AppendLine("^FEC. REIMP.: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture) + " - " + venta.CajeroCod);
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
            else if (venta.TipImpresora == 3)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 10CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 80));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " ELECTRONICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 80));
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
                texto.AppendLine(("|||SER. DE TRAN. DE LA RUTA " + venta.NomOriPas + " - " + venta.NomDesPas + " / " + "SERVICIO:").PadRight(70) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine(venta.NomServicio + " / " + "NRO. ASIENTO: " + venta.NumeAsiento + " / " + "PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine(" / " + "DNI: " + venta.DocNumero + " / " + "FECHA VIAJE: " + venta.FechaViaje + " / " + "HORA VIAJE: " + venta.EmbarqueHora);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(39) + PadBoth("OP.INAFECTA", 25) + "S/".PadLeft(5) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("Autorizado mediante: ".PadRight(40) + PadBoth("OP. GRAVADA", 25) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("N° XXXXX/Sunat ".PadRight(40) + PadBoth("OP. EXONERADA", 25) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("I.G.V. 18%".PadLeft(65) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(65) + "S/".PadLeft(5) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(10));
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
                    texto.AppendLine("FECHA DE REIMPRESION: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture) + " - " + venta.CajeroCod);
                }
            }
            // Impresora Matricial (Formato reducido)
            else if (venta.TipImpresora == 4)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 17CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 65));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " ELECTRONICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 65));
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
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(32) + PadBoth("OP.INAFECTA", 20) + "S/".PadLeft(5) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(8));
                texto.AppendLine("Autorizado mediante: ".PadRight(32) + PadBoth("OP. GRAVADA", 20) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(8));
                texto.AppendLine("N° XXXXX/Sunat ".PadRight(32) + PadBoth("I.G.V. 18%", 20) + "S/".PadLeft(5) + 0.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(8));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(52) + "S/".PadLeft(5) + venta.PrecioCan.ToString("0.##", CultureInfo.InvariantCulture).PadLeft(8));
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
                    texto.AppendLine("FECHA DE REIMPRESION: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture) + " - " + venta.CajeroCod);
                }
            }

            texto.AppendLine(" ");

            byte[] encodedText = Encoding.Default.GetBytes(texto.ToString());
            var boletoBase64 = Convert.ToBase64String(encodedText);

            return boletoBase64;
        }

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
