using CinetInstalacionServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using static ICSharpCode.SharpZipLib.Zip.FastZip;

namespace CinetInstalacionServices
{

    public class TestFileDownload : IFileDownloadService
    {







        public async Task bajarAplicativo(string aplicativo, Action<string, string> logAccion)
        {

            System.IO.Directory.CreateDirectory("C:\\Cinet");
            System.IO.Directory.CreateDirectory("C:\\Cinet\\Comprimidos");
            var url = $"http://cinetsoporte.ddns.net:84/mensaje/Aplicativos/zip/{aplicativo}";
            var cinet_route = $"C:\\cinet";
            var zip_dst = $"{cinet_route}\\Comprimidos\\{aplicativo}.zip";
            var client = new HttpClient();
            logAccion("Instalar", $"Bajando {aplicativo} de: {url}");
            // var response = await client.GetAsync(url);

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += (s, e) =>
                {
                    logAccion(null, $"Bajando {aplicativo} ({e.ProgressPercentage}%)");
                };

                await wc.DownloadFileTaskAsync(new System.Uri(url), zip_dst);

            }
            logAccion("Instalar", $"Descomprimiendo {aplicativo} \nde: {zip_dst}\na: {cinet_route}");


            var zipfile = new FastZip();
            zipfile.ExtractZip(zip_dst, cinet_route, Overwrite.Never, null, "", null, false);

            logAccion("Instalar", $"Completado...");


        }
    }
}
