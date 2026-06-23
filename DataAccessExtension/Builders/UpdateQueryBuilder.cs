using InstaladorComanda.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatos.Builders
{
    public class UpdateQueryBuilder : QueryBuilder
    {
        private string tabla { get; set; }




        private List<Tupla> sets = new List<Tupla>();

        private List<Tupla> wheres = new List<Tupla>();

        public UpdateQueryBuilder(string tabla)
        {
            this.tabla = tabla;
        }

        public UpdateQueryBuilder addSet(string column, object value)
        {
            this.sets.Add(Tupla.create(column, value));
            return this;
        }

        public UpdateQueryBuilder addWhere(string column, object value)
        {
            this.wheres.Add(Tupla.create(column, value));
            return this;
        }

        public override string toQuery()
        {

            List<string> setList = new List<string>();
            List<string> whereList = new List<string>();

            foreach (Tupla tuplaSet in sets)
            {
                string sSet = $"{tuplaSet.getColumna()} = {tuplaSet.getValor()}";
                setList.Add(sSet);
            }

            foreach (Tupla tuplaWhere in wheres)
            {
                string sWhere = $"{tuplaWhere.getColumna()} = {tuplaWhere.getValor()}";
                whereList.Add(sWhere);
            }

            string sSets = string.Join(", ", setList);
            string sWheres = string.Join(" and ", whereList);

            string query = "";
            //query += $"set dateformat dmy; ";
            query += $"update {this.tabla} set {sSets} where {sWheres}";
            //query += $";";
            return query;
        }


    }
}
