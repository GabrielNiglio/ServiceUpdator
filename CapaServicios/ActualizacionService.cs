

using CapaServicios.Classes;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace CapaServicios
{
    public class ActualizacionService
    {
        public abstract class Aplicacion
        {
            protected RegistroUpdater registro { get; set; }

            public Aplicacion(RegistroUpdater registro)
            {
                this.registro = registro;
            }

            public abstract void start(Action<String, string> loggAction);
            public abstract void stop(Action<String, string> loggAction);

        }

        public class AplicacionEjecutable : Aplicacion
        {

            private bool ejecutaExe { get; set; }


            public AplicacionEjecutable(RegistroUpdater registro, bool ejecutaExe = true) : base(registro)
            {
                this.ejecutaExe = ejecutaExe;
            }

            public override void start(Action<string, string> loggAction)
            {
                if (!ejecutaExe)
                {

                    loggAction("Actualizar", "No se inicia la aplicacion, porque se actualizó con un proceso automatico");
                    return;
                }

                Process p = new Process();
                p.StartInfo.FileName = $"{registro.rutaHasta}\\{registro.ejecutable}";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = registro.rutaHasta;
                int xd = Process.GetProcessesByName(registro.ejecutable).Length;

                bool esUnico = registro.unico;
                int abiertos = Process.GetProcessesByName(registro.ejecutable.Replace(".exe", "")).Length;

                loggAction("Actualizar", $"Es unica: {esUnico}, Abiertos: {abiertos}");

                if (!esUnico || abiertos == 0)
                {
                    loggAction("Actualizar", "Iniciando aplicacion...");
                    p.Start();
                    loggAction("Actualizar", "Se inició correctamente");
                }
                else
                {
                    loggAction("Actualizar", "Se decidió no abrir");

                }

            }

            public override void stop(Action<string, string> loggAction)
            {
                loggAction("Actualizar", $"Deteniendo aplicacion...");
                string nombreSinExtension = Path.GetFileNameWithoutExtension(registro.ejecutable);
                //Process[] pro = Process.GetProcessesByName(nombreSinExtension);



                bool cerrados = false;
                Stopwatch sw = Stopwatch.StartNew();
                DateTime ultimoCiclo = DateTime.MinValue;
                while (!cerrados && sw.ElapsedMilliseconds < 30000)
                {
                    foreach (Process proc in Process.GetProcessesByName(nombreSinExtension))
                    {
                        try
                        {
                            proc.Kill();

                        }
                        catch
                        {

                        }
                    }
                    cerrados = Process.GetProcessesByName(nombreSinExtension)
                        .All(p => p.HasExited);

                }


                loggAction("Actualizar", $"Se detuvo correctamente");

            }
            public void stop2(Action<string, string> loggAction)
            {
                Process p = new Process();
                p.StartInfo.FileName = @"taskkill";
                p.StartInfo.Arguments = $"/IM {registro.ejecutable} /F";
                p.StartInfo.RedirectStandardError = true;  // Opcional, si necesitas también leer errores
                p.StartInfo.RedirectStandardOutput = true;  // Opcional, si necesitas también leer errores
                p.StartInfo.UseShellExecute = false; // Importante para redirigir la salida
                p.StartInfo.CreateNoWindow = true; // Si no quieres que aparezca una ventana de consola
                p.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        loggAction("Actualizar", e.Data); // Procesar los datos recibidos
                    }
                };

                p.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        loggAction("Actualizar", e.Data); // Procesar los datos recibidos
                    }
                };

                loggAction("Actualizar", $"Deteniendo aplicacion: " +
                    $"{p.StartInfo.FileName} {p.StartInfo.Arguments}");

                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();



                loggAction("Actualizar", $"Se detuvo correctamente (Code: {p.ExitCode})");
            }
        }

        public class AplicacionServicio : Aplicacion
        {
            private ServiceController servicio { get; set; }

            public AplicacionServicio(RegistroUpdater registro, ServiceController servicio) : base(registro)
            {
                this.servicio = servicio;
            }

            public override void start(Action<String, string> loggAction)
            {


                loggAction("Actualizar", "Iniciando servicio "+servicio.DisplayName+"...");
                try
                {
                    if (servicio.Status != ServiceControllerStatus.Running)
                    {
                        servicio.Start();
                        loggAction("Actualizar", "Se inició el servicio correctamente");
                    }
                    else
                    {
                        loggAction("Actualizar", "El servicio ya estaba iniciado");

                    }

                }
                catch (Exception ex)
                {
                    loggAction("Actualizar", ex.Message);

                    // loggAction("Actualizar", ex.Message + "//" + ex.StackTrace + "////" + ex.Data);
                }



            }


            public override void stop(Action<String, string> loggAction)
            {

                loggAction("Actualizar", "Iniciando deteniencion de servicio...");
                try
                {
                    ServiceControllerStatus[] estados =
                        new[] { ServiceControllerStatus.Stopped, ServiceControllerStatus.StopPending };



                    if (servicio.Status == ServiceControllerStatus.Stopped)
                    {
                        loggAction("Actualizar", "Ya se encontraba detenido");

                    }
                    else if (servicio.Status == ServiceControllerStatus.StopPending)
                    {
                        loggAction("Actualizar", "Se encuentra siendo detenido");

                    }
                    else
                    {
                        servicio.Stop();
                    }



                    servicio.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(2));
                    servicio.Refresh();
                    loggAction("Actualizar", $"El servicio ahora esta: {servicio.Status}");

                }
                catch (Exception ex)
                {
                    loggAction("Actualizar", ex.Message + "//" + ex.StackTrace + "////" + ex.Data);
                }



            }

        }



        public void borrarServico(string servicio, Action<string, string> logAction)
        {

            Process p = new Process();
            p.StartInfo.FileName = $"sc";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = $"delete {servicio}";

            logAction("Actualizacion", "Borrando servicio");


            string respuesat = p.StartInfo.FileName + p.StartInfo.Arguments + "\n";



            p.Start();
            respuesat += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            logAction("W", respuesat);

            logAction("Actualizacion", "El servicio se borró correctamente");
        }


        public void crearServicio(RegistroUpdater infoApp, Action<string, string> logAction)
        {
            string rutaExe = infoApp.rutaHasta + "\\" + infoApp.ejecutable;

            Process p = new Process();
            p.StartInfo.FileName = $"sc";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = $"create {infoApp.aplicacion} binPath={rutaExe} start= auto";

            logAction("Actualizacion", "Creando servicio");


            string respuesat = p.StartInfo.FileName + p.StartInfo.Arguments + "\n";



            p.Start();
            respuesat += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            logAction("W", respuesat);

            logAction("Actualizacion", "El servicio se creó correctamente");
        }

        public AplicacionEjecutable obtenerAplicacionEjecutable(RegistroUpdater registro, bool ejecutaExe = true)
        {
            AplicacionEjecutable app = new AplicacionEjecutable(registro, ejecutaExe);

            return app;

        }

        public ServiceController buscarServicio(string nombre)
        {
            var sc = ServiceController.GetServices();

            ServiceController servicio = null;
            foreach (var serv in sc)
            {
                if (serv.ServiceName.ToLower().Equals(nombre.ToLower()))
                {
                    servicio = serv;
                    break;

                }
            }
            return servicio;

        }

        public AplicacionServicio obtenerAplicacionServicio(RegistroUpdater registro, Action<string, string> logAction)
        {

            ServiceController servicio = buscarServicio(registro.aplicacion);

            if (servicio == null)
            {
                crearServicio(registro, logAction);
                servicio = buscarServicio(registro.aplicacion);
            }

            AplicacionServicio app = new AplicacionServicio(registro, servicio);

            return app;

        }
        private void descomprimirZip(Aplicacion app, RegistroUpdater registro, Action<String, String> logAction)
        {


            string rutaZip = registro.rutaDesdeLoc ?? registro.rutaDesde;
            string rutaCarpeta = registro.rutaHasta;
            string rutaExe = registro.rutaHasta + @"\" + registro.ejecutable;


            FileInfo fZip = new FileInfo(rutaZip);
            FileInfo fExe = new FileInfo(rutaExe);


            string fechaExe = fExe.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
            string fechaZip = "1912-12-12 12:12:12";

            string msgValidarZip = rutaZip + ":";

            try
            {
                fechaZip = fZip.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
                msgValidarZip += fZip.Exists ? $"({fechaZip})" : $"( NO EXISTE EL ARCHIVO )";
            }
            catch (Exception e)
            {
                msgValidarZip += $"({e.Message})";
            }
            string mensajeFin = ".";

            try
            {

                logAction("Validar Zip", msgValidarZip);

                logAction("Validar Exe", rutaExe + "(" + fechaExe + ")");

                bool actualiza = fechaZip.CompareTo(fechaExe) > 0;

                if (actualiza)
                {
                    int reintentos = 0;
                    bool finalizado = false;
                    while (!finalizado && reintentos < 3)
                    {

                        app.stop(logAction);
                        logAction("Actualizar", $"Descomprimiendo archivos... (Reintentos: {reintentos})");
                        try
                        {
                            var zipfile = new FastZip();
                            zipfile.ExtractZip(rutaZip, rutaCarpeta, "");
                            finalizado = true;
                        }
                        catch (Exception ex)
                        {
                            logAction("Actualizar", ex.Message);
                        }
                        reintentos++;
                    }
                    mensajeFin = "Actualizado";


                }
                else
                {
                    mensajeFin = "No hay pendientes de actualizacion";
                }


            }
            catch (Exception ex)
            {

                mensajeFin = "Error: " + ex.Message;
            }


            app.start(logAction);

            logAction("Actualizar", mensajeFin);

        }

        public void actualizar(RegistroUpdater registro, Action<String, String> logAction, bool ejecutaExe = true)
        {

            Aplicacion app = null;

            if (registro.esServicio)
            {
                app = obtenerAplicacionServicio(registro, logAction);
            }
            else
            {
                app = obtenerAplicacionEjecutable(registro, ejecutaExe);
            }

            if (app != null)
            {
                if (registro.rutaDesdeRem1 != null)
                {
                    bajarZip(app, registro, logAction);
                }

                descomprimirZip(app, registro, logAction);
            }

            logAction("Actualizar", "Fin");


        }

        public class RespuestaInfo
        {
            public string aplicacion { get; set; }
            public string fecha { get; set; }
            public string urlDescarga { get; set; }
        }

        private void bajarZip(Aplicacion app, RegistroUpdater registro, Action<string, string> logAction)
        {
            try
            {

                HttpClient cliente = new HttpClient();


                logAction($"DESCARGA-{registro.aplicacion}", $"Accediendo...");
                HttpResponseMessage resp = cliente.GetAsync(registro.rutaDesdeRem1).Result;

                string jInfo = resp.Content.ReadAsStringAsync().Result;

                RespuestaInfo info = JsonConvert.DeserializeObject<RespuestaInfo>(jInfo);

                string fechaRem = info.fecha;
                logAction($"DESCARGA-{registro.aplicacion}", $"{info.urlDescarga} ({info.fecha})");

                FileInfo fI = new FileInfo(registro.rutaDesdeLoc);
                string fechaLoc = fI.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");

                logAction($"DESCARGA-{registro.aplicacion}", $"{registro.rutaDesdeLoc} ({fechaLoc})");

                if (fechaLoc.CompareTo(fechaRem) < 0)
                {
                    descargarArchivo(registro, info);

                }
            }
            catch (Exception ex)
            {
                logAction($"DESCARGA-{registro.aplicacion}", "ERRROR: " + ex.ToString());
            }
        }

        private static void descargarArchivo(RegistroUpdater registro, RespuestaInfo info)
        {
            using (WebClient wc = new WebClient())
            {
                if (File.Exists(registro.rutaDesdeLoc))
                {
                    File.Delete(registro.rutaDesdeLoc);
                }

                string directorio = Path.GetDirectoryName(registro.rutaDesdeLoc);

                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                wc.DownloadFile(info.urlDescarga, registro.rutaDesdeLoc);
            }
        }
    }
}