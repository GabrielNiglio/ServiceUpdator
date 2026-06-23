using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaladorComanda.Exceptions
{
    public class DataAccessException : Exception
    {
        public DataAccessException(string error, string query) : base($"ERROR: {error}; QUERY: {query}")
        {
        }
    }
}
