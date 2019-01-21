using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Utility
{
    public class Log
    {
        private ILog oLog;
        private static Log instance;

        public static Log Instance(Type tipo)
        {

            try
            {
                if (instance == null)
                {
                    string ruta = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                    log4net.GlobalContext.Properties["FilePath"] = ruta.Substring(6, ruta.Length - 6);
                    log4net.Config.XmlConfigurator.Configure();

                }
                instance = new Log(tipo);
                return instance;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private Log(Type tipo)
        {

            try
            {
                oLog = LogManager.GetLogger(tipo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Error(string mensaje)
        {

            oLog.Error(mensaje);

        }

        public void Error(string mensaje, Exception ex)
        {

            try
            {
                oLog.Error(mensaje, ex);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Informacion(string mensaje)
        {
            oLog.Info(mensaje);
        }

        public void Informacion(string mensaje, Exception ex)
        {
            oLog.Info(mensaje, ex);
        }

    }
}
