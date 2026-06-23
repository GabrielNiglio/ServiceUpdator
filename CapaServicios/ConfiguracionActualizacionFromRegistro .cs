using CapaServicios.Classes;
using CapaServicios.Intefaces;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaServicios
{
    public class ConfiguracionActualizacionFromRegistro : IConfiguracionActualizacion
    {
        public RegistroUpdater getAplicacionActualizable(string nombre)
        {

            var reg = Registry.CurrentUser.OpenSubKey("SOFTWARE");
            var regValues = reg.OpenSubKey(@"VB and VBA Program Settings\CINET\UPDATER\" + nombre);




            var registro = new RegistroUpdater();

            registro.rutaDesde = regValues.GetValue("Rutadesde1").ToString();
            registro.rutaHasta = regValues.GetValue("Rutahasta").ToString();
            registro.ejecutable = regValues.GetValue("Ejecutable").ToString();
            registro.aplicacion = nombre;
            registro.esServicio = false;
            if(regValues.GetValueNames().Contains("Servicio"))
            {
                string valor = regValues.GetValue("Servicio").ToString();
                registro.esServicio = valor.Equals("1");
            }
            return registro;
        }

        public List<string> getNombresAplicaciones()
        {
            List<string> listaApps = new List<string>()
            {
                "Profit",
                "ActualizaDatos",
                "PanelDePedidosYa",
                "CostosMostaza",
                "DescargaLocal",
                "PantallaComanda",
                "DualPoint_Llamador",
                "DualPoint_Caja",
                "CinetMozos",
            };

            return listaApps;
        }

        public List<RegistroUpdater> getAplicacionesActualizables()
        {
            List<String> nombresApp = new List<string> { "bancardapi", "cinetmozos" };

            var lista = new List<RegistroUpdater>();

            try
            {
                var reg = Registry.CurrentUser.OpenSubKey("SOFTWARE");
                var updaterFolder = reg.OpenSubKey(@"VB and VBA Program Settings\CINET\UPDATER");

                foreach (string appName in nombresApp)
                {
                    try
                    {
                        var registro = new RegistroUpdater();
                        registro.aplicacion = appName;

                        var regValues = updaterFolder.OpenSubKey(appName);

                        registro.rutaDesde = regValues.GetValue("Rutadesde1").ToString();
                        registro.rutaHasta = regValues.GetValue("Rutahasta").ToString();
                        registro.ejecutable = regValues.GetValue("Ejecutable").ToString();

                        lista.Add(registro);
                    }
                    catch { }
                }
            }
            catch { }
            return lista;
        }
    }
}
