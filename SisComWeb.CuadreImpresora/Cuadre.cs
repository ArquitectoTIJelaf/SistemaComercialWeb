using SisComWeb.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SisComWeb.CuadreImpresora
{
    public class Cuadre
    {
        public static string WriteText(VentaRealizadaEntity venta)
        {
            StringBuilder texto = new StringBuilder();
            // Impresora Térmica
            if (venta.TipImpresora == 1)
            {
                texto.AppendLine(SplitStringPreserving(venta.EmpRazSocial, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving("R.U.C. " + venta.EmpRuc, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving(venta.EmpDireccion, 42, "^"));
                texto.AppendLine(SplitStringPreserving("Agencia: " + venta.EmpDirAgencia, 42, "^"));
                texto.AppendLine("|||^" + PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " DE VENTA ELECTRONICA", 29));
                texto.AppendLine("|^" + PadBoth(string.Format(venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum), 17));
                texto.AppendLine(" ");
                texto.AppendLine("^FECHA DE EMISION  : " + venta.EmisionFecha + "-" + venta.EmisionHora);
                texto.AppendLine("^NRO DOC           : " + venta.DocNumero);
                texto.AppendLine("^PASAJERO          : " + venta.PasNombreCom);
                texto.AppendLine("^COD.              : " + venta.CajeroCod);
                texto.AppendLine("---------------------------------------");
                texto.AppendLine("^DETALLE DE SERVICIO               ");
                texto.AppendLine("---------------------------------------");
                texto.AppendLine("||||^SERVICIO DE TRANSPORTE        ");
                texto.AppendLine("^ORIGEN            : " + venta.NomOriPas);
                texto.AppendLine("^DESTINO           : " + venta.NomDesPas);
                texto.AppendLine("||||^SERVICIO        : " + venta.NomServicio);
                texto.AppendLine("|||^FECHA VIAJE    : " + venta.FechaViaje);
                texto.AppendLine("|||^HORA DE SALIDA : " + venta.EmbarqueHora);
                texto.AppendLine("|||^ASIENTO        : " + venta.NumeAsiento);
                texto.AppendLine(SplitStringPreserving("DIREC.EMBARQUE : " + venta.EmbarqueDir, 40, "||||^", false));
                texto.AppendLine("^---------------------------------------");
                texto.AppendLine("|||^TOTAL        S/ " + venta.PrecioCan.ToString("F2").PadRight(12));
                texto.AppendLine("^SON : " + venta.PrecioDes);
                texto.AppendLine("---------------------------------------");
                texto.AppendLine("^CODIGO : 98OyLYeKsSmBVaz/XSG2v1xmKbE=   ");
                texto.AppendLine("^          Autorizado mediante          ");
                texto.AppendLine("^             N° XXXXX/SUNAT             ");
                texto.AppendLine("^ Representación impresa del comprobante ");
                texto.AppendLine("^     Términos y condiciones, visite     ");
                texto.AppendLine("^" + PadBoth(venta.LinkPag_FE, 39));
                texto.AppendLine("^Debe presentarse 30 minutos antes de la hora de salida.");
                texto.AppendLine("^  Al abordar el bus debe presentar la  ");
                texto.AppendLine("^  representación impresa o digital del  ");
                texto.AppendLine("^              comprobante.              ");
                texto.AppendLine("^       No se aceptan cambios ni       ");
                texto.AppendLine("^      devoluciones, Conservar su      ");
                texto.AppendLine("^      comprobante ante cualquier      ");
                texto.AppendLine("^             eventualidad             ");
                texto.AppendLine(string.Format("@@@{0}|{1}|{2}{3}|{4}|0|{5}|{6}|{7}|{8}",
                                                venta.EmpRuc, venta.DocTipo, venta.BoletoTipo,
                                                venta.BoletoSerie, venta.BoletoNum, venta.PrecioCan,
                                                venta.EmisionFecha, venta.DocTipo, venta.DocNumero));
                texto.AppendLine(" ");
            }
            // Impresora Matricial
            else if (venta.TipImpresora == 3)
            {
                texto.AppendLine("@@@");
                texto.AppendLine("DRAFT 10CPI");
                texto.AppendLine("FACT_ELE");
                texto.AppendLine("@@@");
                texto.AppendLine(PadBoth(venta.EmpRazSocial + " - " + "R.U.C. " + venta.EmpRuc, 80));
                texto.AppendLine(PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA DE VENTA") + " ELECTRÓNICA: " + venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum, 80));
                texto.AppendLine(PadBoth(venta.EmbarqueDir + " - " + venta.EmpDirAgencia, 80));
                texto.AppendLine("FECHA Y HORA DE EMISION".PadRight(30) + (": " + venta.EmisionFecha + " - " + venta.EmisionHora + " - " + "CAJ.: " + venta.CajeroNom).PadRight(50));
                texto.AppendLine("NRO RUC".PadRight(30) + (": " + venta.PasRuc).PadRight(50));
                texto.AppendLine("CLIENTE".PadRight(30) + (": " + venta.PasRazSocial).PadRight(50));
                texto.AppendLine("DIRECCION".PadRight(30) + (": " + venta.PasDireccion).PadRight(50));
                texto.AppendLine("DOC. CLIENTE".PadRight(30) + (": " + venta.DocNumero).PadRight(50));
                texto.AppendLine("CONDICION DE VENTA".PadRight(30) + (": " + venta.NomTipVenta).PadRight(50));
                texto.AppendLine("PASAJERO".PadRight(30) + (": " + venta.PasNombreCom).PadRight(50));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("    DESCRIPCION DEL SERVICIO".PadRight(30) + "TOTAL".PadLeft(50));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine(("|||SERV. DE TRANSP. DE LA RUTA " + venta.NomOriPas + " - " + venta.NomDesPas + " / " + "SERVICIO:").PadRight(70) + venta.PrecioCan.ToString("F2", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine(venta.NomServicio + " / " + "NRO ASIENTO: " + venta.NumeAsiento + " / " + "PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine(" / " + "DNI: " + venta.DocNumero + " / " + "FECHA VIAJE: " + venta.FechaViaje + " / " + "HORA VIAJE: " + venta.EmbarqueHora);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("SON: " + venta.PrecioDes);
                texto.AppendLine(new string('-', 80));
                texto.AppendLine(("CODIGO: " + venta.CodigoX_FE).PadRight(40) + PadBoth("OP. GRAVADA", 25) + "S/".PadLeft(5) + 0.ToString("F2", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("autorizado mediante: ".PadRight(40) + PadBoth("OP. INAFECTA", 25) + "S/".PadLeft(5) + venta.PrecioCan.ToString("F2", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("N° XXXXX/Sunat ".PadRight(40) + PadBoth("OP. EXONERADA", 25) + "S/".PadLeft(5) + 0.ToString("F2", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("I.G.V. 18%".PadLeft(65) + "S/".PadLeft(5) + 0.ToString("F2", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine("IMPORTE TOTAL".PadLeft(65) + "S/".PadLeft(5) + venta.PrecioCan.ToString("F2", CultureInfo.InvariantCulture).PadLeft(10));
                texto.AppendLine(new string('-', 80));
                texto.AppendLine("Representación impresa de la Factura Electrónica");
                texto.AppendLine("  Consulte el documento en: " + venta.LinkPag_FE);
                texto.AppendLine(SplitStringPreserving("Al recibir el presente documento, acepto todos los términos y/o condiciones del contratado del servicio de transporte publicados en los letreros o en los banners y detallados en la página.", 80, "", false));
                texto.AppendLine("");
                texto.AppendLine(new string('-', 25));
                texto.AppendLine("USUARIO CONFORME");
                texto.AppendLine(".");

                texto.AppendLine(" ");
            }

            byte[] encodedText = Encoding.Default.GetBytes(texto.ToString());
            var boletoBase64 = Convert.ToBase64String(encodedText);

            return boletoBase64;
        }

        public static string WriteTextCopy(VentaRealizadaEntity venta)
        {
            StringBuilder texto = new StringBuilder();

            // Impresora Térmica
            if (venta.TipImpresora == 1)
            {
                texto.AppendLine(SplitStringPreserving(venta.EmpRazSocial, 30, "||||^"));
                texto.AppendLine(SplitStringPreserving("R.U.C. " + venta.EmpRuc, 30, "|||^"));
                texto.AppendLine(SplitStringPreserving(venta.EmpDireccion, 42, ""));
                texto.AppendLine(SplitStringPreserving("Agencia: " + venta.EmpDirAgencia, 42, "^"));
                texto.AppendLine("||^" + PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " DE VENTA ELECTRONICA", 29));
                texto.AppendLine("|^" + PadBoth(string.Format(venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum), 17));
                texto.AppendLine(" ");
                texto.AppendLine("FECHA DE EMISION  : " + venta.EmisionFecha + " - " + venta.EmisionHora);
                texto.AppendLine("^CAJERO                : " + venta.CajeroCod + " - " + venta.CajeroNom);
                texto.AppendLine("---------------------------------------");
                texto.AppendLine("");
                texto.AppendLine("^TIPO VENTA: " + venta.NomTipVenta);
                texto.AppendLine("---------------------------------------");
                texto.AppendLine(" # DESCRIPCION                    TOTAL ");
                texto.AppendLine("---------------------------------------");
                texto.AppendLine(" 1 SERVICIO DE TRANSPORTE EN LA     " + venta.PrecioCan.ToString("F2", CultureInfo.InvariantCulture));
                texto.AppendLine("   RUTA: " + venta.NomOriPas + " - " + venta.NomDesPas);
                texto.AppendLine("   SERVICIO: " + venta.NomServicio);
                texto.AppendLine("   NRO ASIENTO: " + venta.NumeAsiento);
                texto.AppendLine("   PASAJERO: " + venta.PasNombreCom);
                texto.AppendLine("   DNI: " + venta.DocNumero);
                texto.AppendLine("   FECHA VIAJE: " + venta.FechaViaje);
                texto.AppendLine("   HORA EMBARQUE: " + venta.EmbarqueHora);
                texto.AppendLine(SplitStringPreserving("LUGAR EMBARQUE: " + venta.EmbarqueDir, 40, "   ", false));
                texto.AppendLine(SplitStringPreserving("DIREC. EMBARQUE: " + venta.EmbarqueDirAgencia, 40, "   ", false));
                texto.AppendLine("   ");
                texto.AppendLine("---------------------------------------");
                texto.AppendLine(" ");
                texto.AppendLine("      **** CONTROL INTERNO ***** ");
                texto.AppendLine(" ");
                texto.AppendLine(" ");
                texto.AppendLine(" ");
                texto.AppendLine(" ");
                texto.AppendLine(" ");
                texto.AppendLine(" ");
                texto.AppendLine(" ");
                texto.AppendLine("^---------------------------------------");
                texto.AppendLine("^            USUARIO CONFORME     ");
                texto.AppendLine(" ");
            }

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
