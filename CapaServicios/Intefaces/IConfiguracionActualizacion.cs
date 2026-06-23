using CapaServicios.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaServicios.Intefaces
{
    public interface IConfiguracionActualizacion
    {




       // List<RegistroUpdater> getAplicacionesActualizablesRutinaManual();
        List<RegistroUpdater> getAplicacionesActualizables();

        RegistroUpdater getAplicacionActualizable(string nombre);


        List<string> getNombresAplicaciones();
    }
}
