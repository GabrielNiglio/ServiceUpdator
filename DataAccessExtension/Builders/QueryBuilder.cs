using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaladorComanda.Builders
{
    public abstract class QueryBuilder
    {
        public abstract string toQuery();

        public string toQueryAtomica()
        {
            string query = $"set dateformat dmy; ";
            query += this.toQuery();
            query += $";";
            return query;
        }

    }
}
