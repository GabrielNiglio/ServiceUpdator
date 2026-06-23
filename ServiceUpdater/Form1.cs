using CapaServicios;
using CapaServicios.Intefaces;
using CinetFileLogger;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Media;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceUpdater
{
    public partial class Form1 : Form
    {

        private readonly cLog logger;
        private readonly IConfiguracionActualizacion configuracionActualizacion = new ConfiguracionActualizacionFromRegistro();




        public Form1(string[] args)
        {
            InitializeComponent();

            this.logger = new cLog("cLog_Updateador_");
        }


        private void ciclar(Object myObject, EventArgs myEventArgs)
        {
            Task task = new Task(actualizar);
            task.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ciclar(null, null);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(ciclar);
            timer.Interval = 60 * 1000;
            timer.Start();
        }

        private void actualizar()
        {
            try
            {

                logger.EscribeLog("Iniciando", "Se inicia");

                var listaApps = configuracionActualizacion.getAplicacionesActualizables();

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
                        sercivioActu.actualizar(app, true, registrarEstado);
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

                    foreach (var serv in sc)
                    {
                        if (serv.ServiceName.ToLower().Equals(app.aplicacion.ToLower()))
                        {
                            servicio = serv;
                            break;

                        }
                    }

                    if (servicio != null)
                    {
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

                        logger.EscribeLog("Validar Zip", rutaZip + "(" + fechaZip + ")");
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
    }
}
