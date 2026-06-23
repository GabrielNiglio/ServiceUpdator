using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatos.Builders
{
    public class SqlFunction
    {
        public string statement { get; }

        public SqlFunction(string statement)
        {
            this.statement = statement;
        }
    }
}
