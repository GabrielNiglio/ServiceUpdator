using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaladorComanda.Builders
{
    public class TableBuilder : QueryBuilder
    {
        private List<string> columnas;
        private List<string> primarys;
        private string nombreTabla;

        public TableBuilder(string nombreTabla)
        {
            this.columnas = new List<string>();
            this.primarys = new List<string>();
            this.nombreTabla = nombreTabla;
        }

        public TableBuilder addColumna(string nombre, string tipo, bool nulleable = true, bool isPK = false, bool autoInc = false)
        {

            if (isPK)
            {
                this.primarys.Add(nombre);
            }
            string nullCondition = nulleable ? "NULL" : "NOT NULL";
            string autoIncCondition = autoInc ? "IDENTITY(1,1) " : "";
            columnas.Add($"   {nombre} {tipo} {nullCondition} {autoIncCondition} ");

            return this;
        }


        public override string toQuery()
        {
            char SALTO_LINEA = '\n';
            string query = $"CREATE TABLE {this.nombreTabla}(" + SALTO_LINEA;

            query += String.Join("," + SALTO_LINEA, columnas) + SALTO_LINEA;

            if (this.primarys.Count > 0)
            {

                query += "PRIMARY KEY (" + String.Join(",", primarys) + ")" + SALTO_LINEA;
            }

            query += ");";

            return query;
        }
    }

}
