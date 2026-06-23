using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaServicios.Classes
{
    public class RegistroUpdater
    {

        public void setParametro(string parametro, string valor)
        {
            switch (parametro)
            {
                case "aplicacion": this.aplicacion = valor; break;
                case "rutaDesde": this.rutaDesde = valor; break;
                case "rutaHasta": this.rutaHasta = valor; break;
                case "rutaDescarga": this.rutaDescarga = valor; break;
                case "ejecutable": this.ejecutable = valor; break;
                case "esServicio": this.esServicio = valor.Equals("S"); break;
                case "forzado": this.forzado = valor.Equals("S"); break;
                case "unico": this.unico = valor.Equals("S"); break;
                case "soloManual": this.soloManual = valor.Equals("N"); break;
            }
        }

        public string aplicacion { get; set; }
        public string rutaDesde { get; set; }
        public string rutaHasta { get; set; }
        public string rutaDescarga { get; set; }

        public string ejecutable { get; set; }

        public bool esServicio { get; set; } = false;
        public bool forzado { get; set; } = false;
        public bool unico { get; set; } = false;
        public bool soloManual { get; set; }
    }
}
