using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinetInstalacionServices.Interfaces
{
    public interface IFileDownloadService
    {
        Task bajarAplicativo(string aplicativo, Action<string, string> logAccion);


    }
}
