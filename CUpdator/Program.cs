using CapaServicios;
using CapaServicios.Classes;
using CapaServicios.Intefaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CUpdator
{
    internal static class Program
    {

        static void logu(string s)
        {

            try
            {
                using (StreamWriter sw = new StreamWriter("C:\\cinet\\logu.txt", true))
                {
                    sw.WriteLine(s);
                }
            }
            catch (Exception ex) { }
        }



        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            logu("iniciando: " + string.Join(" ", args));


            IConfiguracionActualizacion actuConf = ConfiguracionActualizacionFactory.getDefault();


            try
            {
                var x = actuConf.getAplicacionActualizable(args[0]);
                logu("empezando: " + string.Join(" ", args) + " // " + x.esServicio);
            }
            catch (Exception ex)
            {
                logu("Error: " + ex.Message);
            }

            Form form = new FormCUpdator(args[0], actuConf);

            Application.Run(form);


        }
    }
}
