using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinetConfigurationSerices.Interfaces
{
    public interface IEnviromentConfigurationHelper
    {
        Task setODBC(string name, string server, string database);
        Task setUpdaterReg(string ruta, string aplicativo, String equipo = "");

        Task<String> runCMD(string comand);

        String getNombreEquipo();

        Task<List<string>> getInstanciasSqlServer();

    }
}
