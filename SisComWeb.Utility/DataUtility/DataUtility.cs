using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SisComWeb.Utility
{
    public class DataUtility
    {
        public static string MontoSolesALetras(string numero)
        {
            string Valor = string.Empty;

            // ********Declara variables de tipo cadena************
            string palabras, entero, dec, flag;
            palabras = string.Empty;
            dec = string.Empty;
            entero = string.Empty;
            flag = string.Empty;

            // ********Declara variables de tipo entero***********
            int num, x, y;
            flag = "N";

            // **********Número Negativo***********
            if (numero.Substring(1, 1) == "-")
            {
                numero = numero.Substring(2, numero.ToString().Length - 1).ToString();
                palabras = "menos ";
            }

            // **********Si tiene ceros a la izquierda*************
            for (x = 0; x <= numero.ToString().Length - 1; x++)
            {
                if (numero.Substring(0, 1) == "0")
                {
                    numero = numero.Substring(2, numero.ToString().Length - 1).ToString().Trim();
                    if (numero.ToString().Trim().Length == 0)
                        palabras = string.Empty;
                }
                else
                    break;
            }

            // *********Dividir parte entera y decimal************
            for (y = 0; y <= numero.Length - 1; y++)
            {
                if (numero.Substring(y, 1) == ".")
                    flag = "S";
                else if (flag == "N")
                    entero = entero + numero.Substring(y, 1);
                else
                    dec = dec + numero.Substring(y, 1);
            }

            if (dec.Length == 1)
                dec = dec + "0";

            // **********proceso de conversión***********
            flag = "N";

            if (decimal.Parse(numero) <= 999999999)
            {
                for (y = entero.Length; y >= 1; y += -1)
                {
                    num = (entero.Length) - (y - 1);
                    switch (y)
                    {
                        case 3:
                        case 6:
                        case 9:
                            {
                                // **********Asigna las palabras para las centenas***********
                                switch (entero.Substring(num - 1, 1))
                                {
                                    case "1":
                                        {
                                            if (entero.Substring((num - 1) + 1, 1) == "0" & entero.Substring((num - 1) + 2, 1) == "0")
                                                palabras = palabras + "cien ";
                                            else
                                                palabras = palabras + "ciento ";
                                            break;
                                        }

                                    case "2":
                                        {
                                            palabras = palabras + "doscientos ";
                                            break;
                                        }

                                    case "3":
                                        {
                                            palabras = palabras + "trescientos ";
                                            break;
                                        }

                                    case "4":
                                        {
                                            palabras = palabras + "cuatrocientos ";
                                            break;
                                        }

                                    case "5":
                                        {
                                            palabras = palabras + "quinientos ";
                                            break;
                                        }

                                    case "6":
                                        {
                                            palabras = palabras + "seiscientos ";
                                            break;
                                        }

                                    case "7":
                                        {
                                            palabras = palabras + "setecientos ";
                                            break;
                                        }

                                    case "8":
                                        {
                                            palabras = palabras + "ochocientos ";
                                            break;
                                        }

                                    case "9":
                                        {
                                            palabras = palabras + "novecientos ";
                                            break;
                                        }
                                }

                                break;
                            }
                        case 2:
                        case 5:
                        case 8:
                            {
                                // *********Asigna las palabras para las decenas************
                                switch (entero.Substring(num - 1, 1))
                                {
                                    case "1":
                                        {
                                            if (entero.Substring(num, 1) == "0")
                                            {
                                                flag = "S";
                                                palabras = palabras + "diez ";
                                            }
                                            if (entero.Substring(num, 1) == "1")
                                            {
                                                flag = "S";
                                                palabras = palabras + "once ";
                                            }
                                            if (entero.Substring(num, 1) == "2")
                                            {
                                                flag = "S";
                                                palabras = palabras + "doce ";
                                            }
                                            if (entero.Substring(num, 1) == "3")
                                            {
                                                flag = "S";
                                                palabras = palabras + "trece ";
                                            }
                                            if (entero.Substring(num, 1) == "4")
                                            {
                                                flag = "S";
                                                palabras = palabras + "catorce ";
                                            }
                                            if (entero.Substring(num, 1) == "5")
                                            {
                                                flag = "S";
                                                palabras = palabras + "quince ";
                                            }
                                            if (int.Parse(entero.Substring(num, 1)) > 5)
                                            {
                                                flag = "N";
                                                palabras = palabras + "dieci";
                                            }

                                            break;
                                        }

                                    case "2":
                                        {
                                            if (entero.Substring(num, 1) == "0")
                                            {
                                                palabras = palabras + "veinte ";
                                                flag = "S";
                                            }
                                            else
                                            {
                                                palabras = palabras + "veinti";
                                                flag = "N";
                                            }

                                            break;
                                        }

                                    case "3":
                                        {
                                            if (entero.Substring((num - 1) + 1, 1) == "0")
                                            {
                                                palabras = palabras + "treinta ";
                                                flag = "S";
                                            }
                                            else
                                            {
                                                palabras = palabras + "treinta y ";
                                                flag = "N";
                                            }

                                            break;
                                        }

                                    case "4":
                                        {
                                            if (entero.Substring(num, 1) == "0")
                                            {
                                                palabras = palabras + "cuarenta ";
                                                flag = "S";
                                            }
                                            else
                                            {
                                                palabras = palabras + "cuarenta y ";
                                                flag = "N";
                                            }

                                            break;
                                        }

                                    case "5":
                                        {
                                            if (entero.Substring(num, 1) == "0")
                                            {
                                                palabras = palabras + "cincuenta ";
                                                flag = "S";
                                            }
                                            else
                                            {
                                                palabras = palabras + "cincuenta y ";
                                                flag = "N";
                                            }

                                            break;
                                        }

                                    case "6":
                                        {
                                            if (entero.Substring(num, 1) == "0")
                                            {
                                                palabras = palabras + "sesenta ";
                                                flag = "S";
                                            }
                                            else
                                            {
                                                palabras = palabras + "sesenta y ";
                                                flag = "N";
                                            }

                                            break;
                                        }

                                    case "7":
                                        {
                                            if (entero.Substring(num, 1) == "0")
                                            {
                                                palabras = palabras + "setenta ";
                                                flag = "S";
                                            }
                                            else
                                            {
                                                palabras = palabras + "setenta y ";
                                                flag = "N";
                                            }

                                            break;
                                        }

                                    case "8":
                                        {
                                            if (entero.Substring(num, 1) == "0")
                                            {
                                                palabras = palabras + "ochenta ";
                                                flag = "S";
                                            }
                                            else
                                            {
                                                palabras = palabras + "ochenta y ";
                                                flag = "N";
                                            }

                                            break;
                                        }

                                    case "9":
                                        {
                                            if (entero.Substring(num, 1) == "0")
                                            {
                                                palabras = palabras + "noventa ";
                                                flag = "S";
                                            }
                                            else
                                            {
                                                palabras = palabras + "noventa y ";
                                                flag = "N";
                                            }

                                            break;
                                        }
                                }

                                break;
                            }
                        case 1:
                        case 4:
                        case 7:
                            {
                                // *********Asigna las palabras para las unidades*********
                                switch (entero.Substring(num - 1, 1))
                                {
                                    case "1":
                                        {
                                            if (flag == "N")
                                            {
                                                if (y == 1)
                                                    palabras = palabras + "uno ";
                                                else
                                                    palabras = palabras + "un ";
                                            }

                                            break;
                                        }

                                    case "2":
                                        {
                                            if (flag == "N")
                                                palabras = palabras + "dos ";
                                            break;
                                        }

                                    case "3":
                                        {
                                            if (flag == "N")
                                                palabras = palabras + "tres ";
                                            break;
                                        }

                                    case "4":
                                        {
                                            if (flag == "N")
                                                palabras = palabras + "cuatro ";
                                            break;
                                        }

                                    case "5":
                                        {
                                            if (flag == "N")
                                                palabras = palabras + "cinco ";
                                            break;
                                        }

                                    case "6":
                                        {
                                            if (flag == "N")
                                                palabras = palabras + "seis ";
                                            break;
                                        }

                                    case "7":
                                        {
                                            if (flag == "N")
                                                palabras = palabras + "siete ";
                                            break;
                                        }

                                    case "8":
                                        {
                                            if (flag == "N")
                                                palabras = palabras + "ocho ";
                                            break;
                                        }

                                    case "9":
                                        {
                                            if (flag == "N")
                                                palabras = palabras + "nueve ";
                                            break;
                                        }
                                }

                                break;
                            }
                    }

                    // ***********Asigna la palabra mil***************
                    if (y == 4)
                    {
                        if (entero.Substring(6, 1) != "0" | entero.Substring(5, 1) != "0" | entero.Substring(4, 1) != "0" | (entero.Substring(6, 1) == "0" & entero.Substring(5, 1) == "0" & entero.Substring(4, 1) == "0" & entero.Length <= 6))
                            palabras = palabras + "mil ";
                    }

                    // **********Asigna la palabra millón*************
                    if (y == 7)
                    {
                        if (entero.Length == 7 & entero.Substring(0, 1) == "1")
                            palabras = palabras + "millón ";
                        else
                            palabras = palabras + "millones ";
                    }
                }

                // **********Une la parte entera y la parte decimal*************
                if (dec != string.Empty && int.Parse(dec) > 0)
                    Valor = palabras + "con " + Convert.ToString(Convert.ToInt32(dec)) + "/100 soles";
                else
                    Valor = palabras + "con 00/100 soles";
            }
            else
                Valor = string.Empty;

            return Valor.ToUpper();
        }

        public static string ConvertListToXml<T>(T lista, string nombreXmlRoot)
        {
            string cadenaXml = string.Empty;

            Encoding utf8noBOM = new UTF8Encoding(false);
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = utf8noBOM
            };
            XmlSerializer ser = new XmlSerializer(typeof(T), new XmlRootAttribute(nombreXmlRoot));
            StringBuilder sb = new StringBuilder();
            using (XmlWriter xml = XmlWriter.Create(sb, settings))
            {
                ser.Serialize(xml, lista);
            };
            cadenaXml = sb.ToString();

            return cadenaXml;
        }

        public static string ObtenerColorHexadecimal(string _Color)
        {
            string colorHexadecimal = string.Empty;

            var auxColor = long.Parse(_Color);
            int b = (int)(auxColor / 65536);
            int g = (int)((auxColor - b * 65536) / 256);
            int r = (int)(auxColor - b * 65536 - g * 256);

            Color colorRGB = Color.FromArgb(r, g, b);
            colorHexadecimal = "#" + colorRGB.R.ToString("X2") + colorRGB.G.ToString("X2") + colorRGB.B.ToString("X2");

            return colorHexadecimal;
        }

        public static object GetReaderValue(IDataReader reader, string columnName)
        {
            DataTable schema = reader.GetSchemaTable();
            DataRow[] rows = schema.Select(string.Format("ColumnName='{0}'", columnName));
            if ((rows != null) && (rows.Length > 0))
            {
                return reader[columnName];
            }
            else return null;
        }

        public static Int64 ObjectToInt64(IDataReader reader, string columnName)
        {
            return ObjectToInt64(GetReaderValue(reader, columnName));
        }

        public static Int64 ObjectToInt64(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0 : Convert.ToInt64(obj);
        }

        public static Int16 ObjectToInt16(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? Int16.MinValue : Convert.ToInt16(obj);
        }

        public static Int16 ObjectToInt16(IDataReader reader, string columnName)
        {
            return ObjectToInt16(GetReaderValue(reader, columnName));
        }

        public static Int32 ObjectToInt32(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0 : Convert.ToInt32(obj);
        }

        public static Int32? ObjectToInt32Null(object obj)
        {
            Int32? valor = null;
            if ((obj == null) || (obj == DBNull.Value))
            { return valor; }
            else
            { valor = Convert.ToInt32(obj); }
            return valor;
        }

        public static Int32 ObjectToInt32(IDataReader reader, string columnName)
        {
            return ObjectToInt32(GetReaderValue(reader, columnName));
        }

        public static decimal ObjectToDecimal(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0.00M : Convert.ToDecimal(obj);
        }

        public static decimal ObjectToDecimal(IDataReader reader, string columnName)
        {
            return ObjectToDecimal(GetReaderValue(reader, columnName));
        }

        public static decimal StrToDecimal(string obj)
        {
            return ((obj == null) || (obj == "")) ? 0.00M : Convert.ToDecimal(obj);
        }

        public static decimal StrToDecimal(IDataReader reader, string columnName)
        {
            return StrToDecimal(ObjectToString(GetReaderValue(reader, columnName)));
        }

        public static int ObjectToInt(object obj)
        {
            return ObjectToInt32(obj);
        }

        public static int ObjectToInt(IDataReader reader, string columnName)
        {
            return ObjectToInt(GetReaderValue(reader, columnName));
        }

        public static double ObjectToDouble(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0 : Convert.ToDouble(obj);
        }

        public static double ObjectToDouble(IDataReader reader, string columnName)
        {
            return ObjectToDouble(GetReaderValue(reader, columnName));
        }

        public static bool ObjectToBool(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? false : Convert.ToBoolean(obj);
        }

        public static bool ObjectToBool(IDataReader reader, string columnName)
        {
            return ObjectToBool(GetReaderValue(reader, columnName));
        }

        public static string ObjectToString(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);
        }

        public static string ObjectToString(IDataReader reader, string columnName)
        {
            return ObjectToString(GetReaderValue(reader, columnName));
        }

        public static DateTime ObjectToDateTime(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? DateTime.MinValue : Convert.ToDateTime(obj);
        }

        public static TimeSpan ObjectToTimeSpan(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? TimeSpan.MinValue : TimeSpan.Parse(obj.ToString());
        }

        public static DateTime? ObjectToDateTimeNull(object obj)
        {
            DateTime? valor = null;
            if ((obj == null) || (obj == DBNull.Value))
            { return valor; }
            else
            { valor = Convert.ToDateTime(obj); }
            return valor;
        }

        public static DateTime ObjectToDateTime(IDataReader reader, string columnName)
        {
            return ObjectToDateTime(GetReaderValue(reader, columnName));
        }

        public static TimeSpan ObjectToTimeSpan(IDataReader reader, string columnName)
        {
            return ObjectToTimeSpan(GetReaderValue(reader, columnName));
        }

        public static DateTime StringToDateTime(string str)
        {
            return ((str == "")) ? DateTime.MinValue : Convert.ToDateTime(str);
        }

        public static DateTime StringToDateTime(IDataReader reader, string columnName)
        {
            return StringToDateTime(ObjectToString(GetReaderValue(reader, columnName)));
        }

        public static byte ObjectToByte(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? byte.MinValue : Convert.ToByte(obj);
        }

        public static byte ObjectToByte(IDataReader reader, string columnName)
        {
            return ObjectToByte(GetReaderValue(reader, columnName));
        }

        public static int StringToInt(string str)
        {
            return ((str == null) || (str == "")) ? 0 : Convert.ToInt32(str);
        }

        public static int StringToInt(IDataReader reader, string columnName)
        {
            return StringToInt(ObjectToString(GetReaderValue(reader, columnName)));
        }

        public static object IntToDBNull(int int1)
        {
            return ((int1 == 0)) ? DBNull.Value : (object)int1;
        }

        public static object IntToDBNull(IDataReader reader, string columnName)
        {
            return IntToDBNull(ObjectToInt(GetReaderValue(reader, columnName)));
        }

        public static object Int32ToDBNull(Int32 int1)
        {
            return ((int1 == 0)) ? DBNull.Value : (object)int1;
        }

        public static object Int32ToDBNull(IDataReader reader, string columnName)
        {
            return Int32ToDBNull(ObjectToInt32(GetReaderValue(reader, columnName)));
        }

        public static object Int64ToDBNull(Int64 int1)
        {
            return ((int1 == 0)) ? DBNull.Value : (object)int1;
        }

        public static object Int64ToDBNull(IDataReader reader, string columnName)
        {
            return Int64ToDBNull(ObjectToInt64(GetReaderValue(reader, columnName)));
        }

        public static object DateTimeToDBNull(DateTime date)
        {
            return ((date == DateTime.MinValue)) ? DBNull.Value : (object)date;
        }

        public static object DateTimeToDBNull(IDataReader reader, string columnName)
        {
            return DateTimeToDBNull(ObjectToDateTime(GetReaderValue(reader, columnName)));
        }

        public static string ObjectDecimalToStringFormatMiles(object obj)
        {
            return ObjectToDecimal(obj).ToString("#,#0.00");
        }

        public static string ObjectDecimalToStringFormatMiles(IDataReader reader, string columnName)
        {
            return ObjectDecimalToStringFormatMiles(GetReaderValue(reader, columnName));
        }

        public static string BoolToString(bool flag)
        {
            return flag ? "1" : "0";
        }

        public static string BoolToString(IDataReader reader, string columnName)
        {
            return BoolToString(ObjectToBool(GetReaderValue(reader, columnName)));
        }

        public static bool StringToBool(string flag)
        {
            return flag.Equals("1");
        }

        public static bool StringToBool(IDataReader reader, string columnName)
        {
            return StringToBool(ObjectToString(GetReaderValue(reader, columnName)));
        }

        public static string IntToString(int int1)
        {
            return ((int1 == 0)) ? "" : Convert.ToString(int1);
        }

        public static string IntToString(IDataReader reader, string columnName)
        {
            return IntToString(ObjectToInt(GetReaderValue(reader, columnName)));
        }

        public string GenerarXML(string[] array, bool cabecera)
        {

            string retorna;

            MemoryStream memory_stream = new MemoryStream();

            XmlTextWriter xml_text_writer = new XmlTextWriter(memory_stream, System.Text.Encoding.UTF8);

            xml_text_writer.Formatting = Formatting.Indented;
            xml_text_writer.Indentation = 4;



            GeneraCabecera(xml_text_writer, cabecera, 'A');

            xml_text_writer.WriteStartElement("string-array");


            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                    xml_text_writer.WriteElementString("null", "");
                else
                    xml_text_writer.WriteElementString("string", array[i]);
            }


            xml_text_writer.WriteEndElement();

            GeneraCabecera(xml_text_writer, cabecera, 'C');


            xml_text_writer.Flush();

            // Declaramos un StreamReader para mostrar el resultado.
            StreamReader stream_reader = new StreamReader(memory_stream);

            memory_stream.Seek(0, SeekOrigin.Begin);
            retorna = stream_reader.ReadToEnd();


            xml_text_writer.Close();
            stream_reader.Close();
            stream_reader.Dispose();

            return retorna;

        }

        private void GeneraCabecera(XmlTextWriter xmlTextWriter, bool genera, char estado)
        {

            if (genera)
            {
                switch (estado)
                {
                    case 'A':
                        xmlTextWriter.WriteStartDocument();
                        break;
                    case 'C':
                        xmlTextWriter.WriteEndDocument();
                        break;
                    default:
                        break;
                }
            }


        }

        public static DateTime StringddmmyyyyToDate(string strDate)
        {
            //dd/mm/yyyy
            int year = ObjectToInt(strDate.Split('/')[2]);
            int month = ObjectToInt(strDate.Split('/')[1]);
            int day = ObjectToInt(strDate.Split('/')[0]);
            return new DateTime(year, month, day);
        }

        public static string BytesToString(byte[] byt)
        {
            return System.Text.Encoding.Default.GetString(byt);
        }

        public static string ObjectToXML(Object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            string xml = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                xs.Serialize(ms, obj);
                using (StreamReader sr = new StreamReader(ms))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    xml = sr.ReadToEnd();
                }
            }
            return xml;
        }

        public static string StringToSlug(string str)
        {
            str = str.Normalize(System.Text.NormalizationForm.FormD);
            str = new Regex(@"[^a-zA-Z0-9 ]").Replace(str, "").Trim();
            str = new Regex(@"[\/_| -]+").Replace(str, " ");
            return str;
        }

        public static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich <= stFormD.Length - 1; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark || stFormD[ich].ToString() == "̃")
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
