using CapaServicios;
using CapaServicios.Intefaces;
using CinetFileLogger;
using ConexionesManager;
using ICSharpCode.SharpZipLib.Zip;
using InstaladorComanda;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceUpdator
{
    public partial class FormServiceUpdator : Form
    {
        private DateTime ultimoDiaHparamLoc = DateTime.MinValue;
        private readonly cLog logger;
        private readonly IConfiguracionActualizacion configuracionActualizacion;

        private DateTime ultimoCiclo { get; set; }

        public DateTime getUltimoCiclo()
        {
            return ultimoCiclo;
        }

        public FormServiceUpdator(IConfiguracionActualizacion ActuConf)
        {
            InitializeComponent();

            this.logger = new cLog("cLog_Updateador_");
            this.configuracionActualizacion = ActuConf;
        }


        private void ciclar(Object myObject, EventArgs myEventArgs)
        {

            try
            {


                while (true)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    actualizar();

                    double esperar = Math.Max(60000 - sw.ElapsedMilliseconds, 0);
                    Thread.Sleep(TimeSpan.FromMilliseconds(esperar));
                }
            }
            catch (Exception ex) { 
                
            
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                GestorConexiones gc = new GestorConexiones();
                gc.actualizarDeArchivoBaseSiNoExiste();

            }
            catch { }

            try
            {

            ActualizacionService sercivioActu = new ActualizacionService();

            sercivioActu.borrarServico("ServiceUpdator", (a, b) => logger.EscribeLog(a, b));

            }
            catch { }
            
            Thread hilo = new Thread(() => ciclar(null, null));
            hilo.Start();
           
            //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            //timer.Tick += new EventHandler(ciclar);
            //timer.Interval = 60 * 1000;
            //timer.Start();
        }

        private void actualizar()
        {

            try
            {
                DateTime hoy = DateTime.Now.Date;
                if (hoy != ultimoDiaHparamLoc)
                {

                    var cf = new ConnectionFactory();
                    string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    cf.connectBackoffice().guardarHparamLoc("ServiceUpdator", version);
                    ultimoDiaHparamLoc = hoy;
                }
            }
            catch(Exception ex) {
                logger.EscribeLog("Escribiendo HPARAMLOC", ex.ToString());
            }

            try
            {
                ultimoCiclo = DateTime.Now;
                logger.EscribeLog("Iniciando", "Se inicia");

                var listaApps = configuracionActualizacion
                    .getAplicacionesActualizables()
                    .Where(a => a.esServicio || a.forzado)
                    .Where(a=>!a.soloManual)
                    .Where(a=> !a.aplicacion.Equals("ServiceUpdator",StringComparison.OrdinalIgnoreCase))
                    .ToList();



                ActualizacionService sercivioActu = new ActualizacionService();

                foreach (var app in listaApps)
                {

                    Action<string, string> registrarEstado = (string tarea, string texto) =>
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            txtEstado.Text = texto;
                        }));

                        logger.EscribeLog(tarea + "-" + app.aplicacion, texto);
                    };

                    try
                    {
                        sercivioActu.actualizar(app, registrarEstado, ejecutaExe: false);
                    }
                    catch (Exception ex)
                    {
                        registrarEstado("Salida abrupta", ex.Message + " // " + ex.StackTrace);
                    }

                }

                //Application.Exit();
            }
            catch (Exception ex)
            {
                logger.EscribeLog("Salida abrupta", ex.Message + " // " + ex.StackTrace);

            }

        }

        #region OBSOLETO



        [Obsolete]
        private void actualizar2()
        {


            Action<string, string> registrarEstado = (string tarea, string texto) =>
            {

                txtEstado.Text = texto;
                logger.EscribeLog(tarea + "-" + "", texto);
            };


            try
            {

                registrarEstado("Iniciando", "Se inicia");

                var listaApps = configuracionActualizacion.getAplicacionesActualizables();



                foreach (var app in listaApps)
                {

                    var sc = ServiceController.GetServices();

                    ServiceController servicio = null;

                    //Se busca el servicio en el listado de servicios
                    foreach (var serv in sc)
                    {
                        if (serv.ServiceName.ToLower().Equals(app.aplicacion.ToLower()))
                        {
                            servicio = serv;
                            break;

                        }
                    }

                    if (servicio != null)
                    {//-> Si se encuentra:

                        //Se preparan las task (Sin ejecutarse)
                        Task taskStop = new Task(() =>
                        {
                            try
                            {
                                servicio.Stop();
                            }
                            catch (Exception ex)
                            {
                                logger.EscribeLog("Actualizar-" + this.Text, ex.Message);
                            }

                        });

                        Task taskDescomprimir = new Task(() =>
                        {
                            try
                            {
                                var zipfile = new FastZip();
                                zipfile.ExtractZip(app.rutaDesde, app.rutaHasta, "");

                            }
                            catch (Exception ex)
                            {
                                logger.EscribeLog("Actualizar-" + this.Text, ex.Message);
                            }
                        });

                        Task taskStart = new Task(() =>
                        {
                            try
                            {
                                servicio.Start();

                            }
                            catch (Exception ex)
                            {
                                logger.EscribeLog("Actualizar-" + this.Text, ex.Message);
                            }
                        });


                        string rutaZip = app.rutaDesde;
                        string rutaExe = app.rutaHasta + @"\" + app.ejecutable;

                        FileInfo fZip = new FileInfo(rutaZip);
                        FileInfo fExe = new FileInfo(rutaExe);




                        string fechaExe = fExe.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
                        string fechaZip = fZip.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");

                        string mensajeFin = "";

                        if (fZip.Exists)
                        {

                            logger.EscribeLog("Validar Zip", rutaZip + "(" + fechaZip + ")");
                        }
                        else
                        {

                            logger.EscribeLog("Validar Zip", rutaZip + "( NO EXISTE )");
                        }

                        logger.EscribeLog("Validar Exe", rutaExe + "(" + fechaExe + ")");

                        if (fechaZip.CompareTo(fechaExe) > 0)
                        {
                            txtEstado.Text = "Deteniendo servicio...";
                            logger.EscribeLog("Actualizar-" + this.Text, txtEstado.Text);
                            taskStop.Start();
                            taskStop.Wait();


                            txtEstado.Text = "Descomprimiendo archivos...";
                            logger.EscribeLog("Actualizar-" + this.Text, txtEstado.Text);
                            taskDescomprimir.Start();
                            taskDescomprimir.Wait();

                            mensajeFin = "Actualizado";

                        }
                        else
                        {
                            mensajeFin = "No hay pendientes de actualizacion";

                        }



                        txtEstado.Text = "Iniciando servicio...";
                        logger.EscribeLog("Actualizar-" + this.Text, txtEstado.Text);
                        taskStart.Start();
                        taskStart.Wait();


                        txtEstado.Text = mensajeFin;
                    }

                }

                //Application.Exit();
            }
            catch (Exception ex)
            {

                registrarEstado("Salida abrupta", ex.Message + " // " + ex.StackTrace);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        #endregion

        private void FormServiceUpdator_Load(object sender, EventArgs e)
        {

        }
    }
}
