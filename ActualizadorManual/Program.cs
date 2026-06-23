using CapaServicios.Intefaces;
using CapaServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinetFileLogger;

namespace ActualizadorManual
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);



       IConfiguracionActualizacion configuracion = new ConfiguracionActualizacionFromApi();


         ActualizacionService actu= new ActualizacionService();
            cLog logger = new cLog("cLog_ActualizadorManual_");

            Application.Run(new FormPrincipal(configuracion,actu, logger));
        }
    }
}
