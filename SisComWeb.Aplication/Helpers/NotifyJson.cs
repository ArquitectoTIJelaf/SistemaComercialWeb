
namespace SisComWeb.Aplication.Helpers
{
    /// <summary>
    /// Helper para constriuir una notificación con formato Json
    /// </summary>
    public class NotifyJson
    {
        /// <summary>
        /// Construye Json con el tipo de notificación y mensaje vacio
        /// </summary>
        /// <param name="tipoNotificacion"></param>
        /// <returns></returns>
        internal static object BuildJson(KindOfNotify tipoNotificacion)
        {
            return new
            {
                tipoNotificacion = tipoNotificacion.ToString(),
                string.Empty
            };
        }

        /// <summary>
        ///  Construye Json con el tipo de notificación y mensaje como parámetro
        /// </summary>
        /// <param name="tipoNotificacion"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        internal static object BuildJson(KindOfNotify tipoNotificacion, string mensaje)
        {
            return new
            {
                tipoNotificacion = tipoNotificacion.ToString(),
                mensaje
            };
        }
    }
}