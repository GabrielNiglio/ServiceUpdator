using CapaServicios.Intefaces;
using server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceUpdator.Web
{
    internal class InfoController : BabyServerLocal
    {

        private readonly DateTime start;
        private readonly IConfiguracionActualizacion actuConf;
        private readonly FormServiceUpdator form;


        private void configurar()
        {
            this.addEndpoint("/", doGetInfo);
            this.addEndpoint("/info", doAppInfo);

            //this.addEndpoint("/ruta", doDownloadFromUpd);

            //this.addEndpoint("/rutax", doDownloadFromRutax);
            this.addEndpoint("/descarga", doDescarga);

        }

        public InfoController(DateTime start, IConfiguracionActualizacion actuConf, FormServiceUpdator form)
        {
            this.start = start;
            this.actuConf = actuConf;
            this.form = form;
            configurar();
        }

        //private Object doDownloadFromUpd(HttpListenerRequest req, HttpListenerResponse res)
        //{
        //    var appName = req.QueryString["app"];
        //    var apps = actuConf.getAplicacionesActualizables();
        //    try
        //    {
        //        string ruta = apps.Where(app => app.aplicacion == appName).Select(app => app.rutaDesde).First();

        //        return new BabyServer.Archivo("CUpdator.zip", new StreamReader(ruta));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex;
        //    }
        //}

        private Object doDescarga(HttpListenerRequest req, HttpListenerResponse res)
        {
            var appName = req.QueryString["app"];
            var apps = actuConf.getAplicacionActualizable(appName);
            try
            {
                string ruta = apps.rutaDesdeLoc;

                string nombreZip = apps.rutaDesdeLoc.Split('\\').Last();

                return new BabyServerLocal.ArchivoPorRuta(nombreZip, ruta);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        //private Object doDownloadFromRutax(HttpListenerRequest req, HttpListenerResponse res)
        //{
        //    var appName = req.QueryString["app"];
        //    try
        //    {
        //        var zipName = appName + ".zip";
        //        var app = actuConf.getAplicacionActualizable("CUpdator");
        //        var ruta = app.rutaDesde.Replace("CUpdator.zip", zipName);
        //        return new BabyServer.Archivo(zipName, File.ReadAllBytes(ruta));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex;
        //    }
        //}

        public class RespAppInfo
        {
            public string aplicacion { get; set; }
            public string fecha { get; set; }
            public string urlDescarga { get; set; }
        }



        private Object doAppInfo(HttpListenerRequest req, HttpListenerResponse res)
        {
            var appName = req.QueryString["app"];


            RespAppInfo respAppInfo = new RespAppInfo();
            respAppInfo.aplicacion = appName;

            string rutaBase = req.Url
                 .ToString()
                 .Split('?')[0]
                 .Replace("/info", "");


            var appDara = actuConf.getAplicacionActualizable(appName);
            if (appDara == null || appDara.rutaDesdeLoc == null)
            {
                return respAppInfo;
            }

            respAppInfo.urlDescarga = $"{rutaBase}/descarga?app={appName}";


            FileInfo fi = new FileInfo(appDara.rutaDesdeLoc);
            string fechaExe = fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
            respAppInfo.fecha = fechaExe;



            return respAppInfo;


        }
        private Object doGetInfo(HttpListenerRequest req, HttpListenerResponse res)
        {

            var apps = actuConf.getAplicacionesActualizables();

            var versionesApp = apps.Select(app =>
            {
                string rutaCompletaExe = app.rutaHasta + "\\" + app.ejecutable;

                string version = "No existe";

                try
                {
                    version = FileVersionInfo.GetVersionInfo(rutaCompletaExe).FileVersion;
                }
                catch { }

                return new { app.aplicacio, version };
            });

            return new
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                desde = start,
                ultimoCiclo = form.getUltimoCiclo(),
                versiones = versionesApp,
                configuracion = apps
            };
        }






    }
}
