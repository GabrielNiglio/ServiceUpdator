using CapaServicios;
using CapaServicios.Intefaces;
using server;
using ServiceUpdator.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceUpdator
{

  
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {

            DateTime start = DateTime.Now;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IConfiguracionActualizacion actuConf = ConfiguracionActualizacionFactory.getDefault();

            FormServiceUpdator form = new FormServiceUpdator(actuConf);

            Thread hilo = new Thread(() =>
            {
                try
                {
                BabyServerLocal server = new InfoController(start, actuConf, form);

                server.setPuerto(8050);
                server.iniciar();

                }catch(Exception e)
                {
                }
            });
            hilo.Start();

            Application.Run(form);
        }
    }
}
