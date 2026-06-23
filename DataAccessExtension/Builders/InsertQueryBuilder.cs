using BaseDatos.Builders;
using InstaladorComanda.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatos
{


    public class InsertQueryBuilder : QueryBuilder
    {

        private const string dateformat = "dd/MM/yyyy";

        public string tabla { get; set; }

        public List<Tupla> tuplas;

        public InsertQueryBuilder(string tabla)
        {
            this.tabla = tabla;
            this.tuplas = new List<Tupla>();
        }

        public override string toQuery()
        {
            List<String> columnas = new List<string>();
            List<String> valores = new List<string>();

            foreach (var tupla in tuplas)
            {
                columnas.Add(tupla.getColumna());
                valores.Add(tupla.getValor());
            }

            string sSqlColumnas = string.Join(", ", columnas);
            string sSqlValores = string.Join(", ", valores);



            string query = "";
            //  query += $"set dateformat dmy; ";
            query += $"insert into {this.tabla} ({sSqlColumnas}) values ({sSqlValores})";
            //  query += $";";
            return query;

        }

        public InsertQueryBuilder addTupla(string columna, object valor)
        {
            tuplas.Add(Tupla.create(columna, valor));
            return this;
        }
    }
}
