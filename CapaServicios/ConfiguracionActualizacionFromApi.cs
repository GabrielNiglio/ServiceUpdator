using CapaServicios.Classes;
using CapaServicios.Intefaces;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CapaServicios
{
    public class ConfiguracionActualizacionFromApi : IConfiguracionActualizacion
    {
        public RegistroUpdater getAplicacionActualizable(string nombre)
        {

            var registros = this.getAplicacionesActualizables();

            RegistroUpdater registroUpdater = registros.Where(r => r.aplicacion.Equals(nombre)).FirstOrDefault();

            return registroUpdater;
        }

        /// <summary>
        /// Obtiene una lista de registros de actualizaciones de aplicaciones desde una fuente remota o local.
        /// Primero consulta con la api, si la api no responde, revisa que el archivo exista en %temp%.
        /// Si tampoco existe en %temp% lo busca en el apppath.
        /// </summary>
        /// <returns>
        /// Una lista de objetos RegistroUpdater que representan las aplicaciones que pueden ser actualizadas.
        /// </returns>
        public List<RegistroUpdater> getAplicacionesActualizables()
        {
            // Define una cadena para almacenar los datos JSON.
            string jsonData;

            // Establece la ruta del archivo JSON.
            string rutaJson = System.IO.Path.GetTempPath() + "\\updater.json";

            try
            {
                // Crea una instancia de HttpClient para realizar solicitudes HTTP.
                HttpClient client = new HttpClient();

                // Obtiene la URL base desde la configuración.
                string urlBase = ConfigurationManager.AppSettings["ruta"];

                // Construye la URL completa para obtener los datos de actualización.
                string url = urlBase + "/api/Configuracion/update";

                // Realiza una solicitud GET para obtener los datos de actualización.
                var respuesta = client.GetAsync(url).Result;

                // Verifica si la respuesta es exitosa (código de estado 200 - OK).
                if (respuesta.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Error en la solicitud HTTP");
                }

                // Lee los datos JSON de la respuesta HTTP.
                jsonData = respuesta.Content.ReadAsStringAsync().Result;

                // Escribe los datos JSON en un archivo local.
                using (StreamWriter writer = new StreamWriter(rutaJson))
                {
                    writer.Write(jsonData);
                }
            }
            catch (Exception e)
            {
                // En caso de error, si el archivo local no existe, se utiliza una ruta predeterminada.
                if (!File.Exists(rutaJson))
                {
                    rutaJson = "updater.json";
                }

                // Lee los datos JSON desde el archivo local.
                using (StreamReader reader = new StreamReader(rutaJson))
                {
                    jsonData = reader.ReadToEnd();
                }
            }

            // Deserializa los datos JSON en una lista de objetos RegistroUpdater.
            var lista = JsonConvert.DeserializeObject<List<RegistroUpdater>>(jsonData);

            // Devuelve la lista de registros de actualizaciones.
            return lista;
        }

 
        public List<string> getNombresAplicaciones()
        {
            var listaApps = getAplicacionesActualizables().Select(u => u.aplicacion).ToList();

            return listaApps;
        }
    }
}
