using SisComWeb.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SisComWeb.CuadreImpresora
{
    public class Cuadre
    {
        public static string WriteText(VentaRealizada venta)
        {
            StringBuilder texto = new StringBuilder();
            texto.AppendLine(SplitStringPreserving(venta.EmpRazSocial, 30, "|||^"));
            texto.AppendLine(SplitStringPreserving("R.U.C. " + venta.EmpRuc, 30, "|||^"));
            texto.AppendLine(SplitStringPreserving(venta.EmpDireccion, 42, "^"));
            texto.AppendLine(SplitStringPreserving("Agencia: " + venta.EmpDirAgencia, 42, "^"));
            texto.AppendLine("|||^" + PadBoth((venta.BoletoTipo == "F" ? "FACTURA" : "BOLETA") + " DE VENTA ELECTRONICA", 29));
            texto.AppendLine("|^" + PadBoth(String.Format(venta.BoletoTipo + venta.BoletoSerie + "-" + venta.BoletoNum), 17));
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
            texto.AppendLine(String.Format("@@@{0}|{1}|{2}{3}|{4}|0|{5}|{6}|{7}|{8}",
                                            venta.EmpRuc, venta.DocTipo, venta.BoletoTipo,
                                            venta.BoletoSerie, venta.BoletoNum, venta.PrecioCan,
                                            venta.EmisionFecha, venta.DocTipo, venta.DocNumero));
            texto.AppendLine(" ");
            byte[] encodedText = Encoding.Default.GetBytes(texto.ToString());

            var aux = Convert.ToBase64String(encodedText);

            return aux;
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
            return sb.ToString();
        }

        public static string PadBoth(string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }
}
