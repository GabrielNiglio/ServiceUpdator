using CapaServicios.Classes;
using CapaServicios.Intefaces;
using InstaladorComanda;
using InstaladorComanda.DataAccessExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaServicios
{
    public class ConfiguracionActualizacionFromBackoffice : IConfiguracionActualizacion
    {
        public RegistroUpdater getAplicacionActualizable(string nombre)
        {

            var registros = this.getAplicacionesActualizables();

            RegistroUpdater registroUpdater = registros.Where(r => r.aplicacion.Equals(nombre)).FirstOrDefault();

            return registroUpdater;
        }

        public List<RegistroUpdater> getAplicacionesActualizables()
        {
            List<RegistroUpdater> registroUpdaters = null;

            ConnectionFactory connectionFactory = new ConnectionFactory();

            using(ConexionGeneral connBackoffice = connectionFactory.connectBackoffice())
            {
                registroUpdaters = connBackoffice.GetRegistroUpdaters();
            }

            return registroUpdaters;

        }

        public List<string> getNombresAplicaciones()
        {
            var listaApps = getAplicacionesActualizables().Select(u => u.aplicacion).ToList();

            return listaApps;
        }
    }
}
