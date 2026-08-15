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

            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                return;
            }


            DateTime start = DateTime.Now;

            var cts = new CancellationTokenSource();
            var token = cts.Token;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IConfiguracionActualizacion actuConf = ConfiguracionActualizacionFactory.getDefault();

            FormServiceUpdator form = new FormServiceUpdator(actuConf, token);

            Thread hilo = new Thread(() =>
            {
                try
                {
                    BabyServerLocal server = new InfoController(start, actuConf, form);

                    server.setPuerto(8050);
                    server.iniciar(token);

                }
                catch (Exception e)
                {
                }
            });
            hilo.Start();

            Application.Run(form);

            cts.Cancel();

        }
    }
}
