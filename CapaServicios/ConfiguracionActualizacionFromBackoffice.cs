using CapaServicios.Classes;
using CapaServicios.Intefaces;
using InstaladorComanda;
using InstaladorComanda.DataAccessExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

            RegistroUpdater registroUpdater = registros
                .Where(r => r.aplicacion.ToUpper() == (nombre.ToUpper()))
                .FirstOrDefault();

            return registroUpdater;
        }

        public List<RegistroUpdater> getAplicacionesActualizables()
        {
            List<RegistroUpdater> registroUpdatersTemp = new List<RegistroUpdater>();
            var registroUpdatersDB = new List<RegistroUpdater>();

            string rutaTemp = Path.Combine(Path.GetTempPath(), "upd_config.json");
            try
            {

                if (File.Exists(rutaTemp))
                {
                    string apa = "";
                    using (StreamReader sr = new StreamReader(rutaTemp))
                    {
                        registroUpdatersTemp = JsonConvert.DeserializeObject<List<RegistroUpdater>>(sr.ReadToEnd()) 
                            ?? new List<RegistroUpdater>();



                    }
                    try
                    {
                        using (StreamWriter x = new StreamWriter("c:\\cinet\\xd.txt", append: false))
                        {

                            x.WriteLine(rutaTemp);
                            x.WriteLine(apa);
                        }
                    }
                    catch { }

                }

                foreach (var registro in registroUpdatersTemp)
                {
                    registro.aplicacion = registro.aplicacion.ToUpper();
                }

            }
            catch { }
            try
            {
                ConnectionFactory connectionFactory = new ConnectionFactory();

                using (ConexionGeneral connBackoffice = connectionFactory.connectBackoffice())
                {
                    registroUpdatersDB = connBackoffice.GetRegistroUpdaters();


                }


            }
            catch
            {

            }
            var appsEnBko = registroUpdatersDB.Select(r => r.aplicacion.ToUpper()).ToHashSet();
            var registroUpdaters = registroUpdatersTemp
                .Where(r => !appsEnBko.Contains(r.aplicacion.ToUpper()))
                .ToList();

            registroUpdaters.AddRange(registroUpdatersDB);

            try
            {
                using (StreamWriter sw = new StreamWriter(rutaTemp))
                {
                    sw.Write(JsonConvert.SerializeObject(registroUpdaters));

                }
            }
            catch { }


            return registroUpdaters;

        }

        public List<string> getNombresAplicaciones()
        {
            var listaApps = getAplicacionesActualizables().Select(u => u.aplicacion).ToList();

            return listaApps;
        }
    }
}
