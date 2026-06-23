using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatos.Builders
{
    public class Tupla
    {
        public static Tupla create(String columna, object valor)
        {

            bool usaComillas = false;
            if (valor == null)
            {
                valor = "null";
            }
            else if (valor is string sValor)
            {

                valor = sValor.Replace("'", "''");
                usaComillas = true;
            }
            else if (valor is char vChar)
            {
                valor = (vChar == '\0' ? "" : vChar.ToString());
                usaComillas = true;
            }
            else if (valor is DateTime vDateTime)
            {
                valor = vDateTime.ToString("dd/MM/yyyy HH:mm:ss");
                usaComillas = true;
            }else if (valor is  SqlFunction vFunc)
            {
                valor = vFunc.statement; 
            }

            return new Tupla(columna, valor.ToString(), usaComillas);
        }



        private string columna { get; set; }
        private string valor { get; set; }
        private bool usaComillas { get; set; }

        public Tupla(string columna, string valor, bool usaComillas)
        {
            this.columna = columna;
            this.valor = valor;
            this.usaComillas = usaComillas;
        }

        public string getValor()
        {
            string retorno = this.valor?.ToString() ?? "null";

            if (usaComillas)
            {
                retorno = $"'{retorno.ToString()}'";
            }
            return retorno;
        }

        public string getColumna()
        {
            return $"[{this.columna}]";
        }
    }

}
