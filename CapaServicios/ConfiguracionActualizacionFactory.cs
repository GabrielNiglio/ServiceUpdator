using CapaServicios.Intefaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaServicios
{
    public class ConfiguracionActualizacionFactory
    {
        public static IConfiguracionActualizacion getDefault() {
            string updatingEngine = ConfigurationManager.AppSettings["updater"]?.ToLower() ?? "backoffice";
            return get(updatingEngine);
        }

        public static IConfiguracionActualizacion get(string configuracion)
        {
            IConfiguracionActualizacion actuConf;
            if (configuracion.ToLower() == "api")
            {
                actuConf = new ConfiguracionActualizacionFromApi();
            }
            else if (configuracion.ToLower() == "registro")
            {
                actuConf = new ConfiguracionActualizacionFromRegistro();
            }
            else
            {
                actuConf = new ConfiguracionActualizacionFromBackoffice();
            }

            return actuConf;
        }
    }
}
