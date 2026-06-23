using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ICSharpCode.SharpZipLib.Zip.FastZip;
using CapaServicios.Classes;

namespace CapaServicios
{
    public class CinetDownloader
    {


        public async Task bajarAplicativo(RegistroUpdater registro, Action<string, string> logAccion)
        {

            System.IO.Directory.CreateDirectory("C:\\Cinet");
            System.IO.Directory.CreateDirectory("C:\\Cinet\\Comprimidos");
            var url = registro.rutaDescarga;
            var cinet_route = $"C:\\cinet";
            var zip_dst = $"{cinet_route}\\Comprimidos\\{registro.aplicacion}.zip";
            var client = new HttpClient();
            logAccion("Instalar", $"Bajando {registro.aplicacion} de: {url}");
            // var response = await client.GetAsync(url);

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += (s, e) =>
                {
                    logAccion(null, $"Bajando {registro.aplicacion} ({e.ProgressPercentage}%)");
                };

                await wc.DownloadFileTaskAsync(new System.Uri(url), zip_dst);

            }
            logAccion("Instalar", $"Descomprimiendo {registro.aplicacion} \nde: {zip_dst}\na: {cinet_route}");


            var zipfile = new FastZip();
            zipfile.ExtractZip(zip_dst, cinet_route, Overwrite.Never, null, "", null, false);




            logAccion("Instalar", $"Completado...");



        }


    }
}
