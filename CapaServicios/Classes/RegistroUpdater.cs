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
            switch (parametro.ToUpper())
            {
                case "APLICACION": this.aplicacion = valor; break;
                case "RUTADESDE": this.rutaDesde = valor; break;
                case "RUTAHASTA": this.rutaHasta = valor; break;
                case "RUTADESCARGA": this.rutaDescarga = valor; break;
                case "EJECUTABLE": this.ejecutable = valor; break;
                case "ESSERVICIO": this.esServicio = valor.Equals("S"); break;
                case "FORZADO": this.forzado = valor.Equals("S"); break;
                case "UNICO": this.unico = valor.Equals("S"); break;
                case "SOLOMANUAL": this.soloManual = valor.Equals("S"); break;
                case "RUTADESDELOC": this.rutaDesdeLoc = valor; break;
                case "RUTADESDEREM1": this.rutaDesdeRem1 = valor; break;
            }
        }
        
        private string _aplicacion;


        public string aplicacion { get=>_aplicacion; set=>_aplicacion = value.ToUpper(); }
        public string rutaDesde { get; set; }
        public string rutaHasta { get; set; }
        public string rutaDescarga { get; set; }

        public string ejecutable { get; set; }

        public bool esServicio { get; set; } = false;
        public bool forzado { get; set; } = false;
        public bool unico { get; set; } = false;
        public bool soloManual { get; set; }
        public string rutaDesdeLoc { get; set; }
        public string rutaDesdeRem1 { get; private set; }
    }
}
