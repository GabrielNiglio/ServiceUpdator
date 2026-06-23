using CapaServicios;
using CapaServicios.Classes;
using CapaServicios.Intefaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CUpdator
{
    public partial class FormCUpdator : Form
    {

        private readonly string aplicacion;
        private readonly IConfiguracionActualizacion configuracionActualizacion;
        private readonly string runId;

        public FormCUpdator(string aplicacion)
        {
            InitializeComponent();
            this.aplicacion = aplicacion;
            this.runId = Guid.NewGuid().ToString();

        }

       private void logu(string s)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("C:\\cinet\\logu.txt", true))
                {
                    sw.WriteLine(s);
                }
            }
            catch (Exception ex) { }
        }

        public FormCUpdator(string aplicacion, IConfiguracionActualizacion configuracionActualizacion) : this(aplicacion)
        {
            this.configuracionActualizacion = configuracionActualizacion;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }


        private void escribirRenglonCsv(StreamWriter sw, params object[] celdas)
        {



            var celd = celdas.Select(o => $"\"{o.ToString().Replace("\"", "\"\"")}\"");

            string renglon = string.Join(";", celd);

            sw.WriteLine(renglon);



        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            int paso = 1;
            logu("Iniciando formulario con: " + aplicacion);

            ActualizacionService actu = new ActualizacionService();
            logu((paso++).ToString());

            Action<string, string> logInfoAction = (s1, s2) =>
            {

                this.BeginInvoke((Action)(() =>
                {

                    logu("haciendo algo: " + string.Join(" ", s2));


                    if (!s2.ToLower().Equals("fin"))
                    {

                        Info2.Text = s2;
                        Info2.MaximumSize = new Size(this.Width - 10, 0);
                        Info2.AutoSize = true;
                    }
                    try
                    {

                        DateTime ahora = DateTime.Now;
                        string ymd = ahora.ToString("yyyy_MM_dd");
                        string exePath = Assembly.GetEntryAssembly().Location;
                        string exeDir = Path.GetDirectoryName(exePath);
                        string logFile = exeDir + "\\cLog_CUpdator_" + ymd + ".csv";

                        bool nuevo = !File.Exists(logFile);

                        using (StreamWriter sw = new StreamWriter(logFile, true))
                        {
                            if (nuevo)
                            {
                                escribirRenglonCsv(sw, "fyh", "uuid", "tipo","mensaje");
                            }
                            escribirRenglonCsv(sw,ahora.ToString("yyyy-MM-dd HH:mm:ss"),this.runId,s1,s2);
                        }
                    }
                    catch (Exception ex) {

                        logu("Error");
                    }


                }));
            };
          

           logu((paso++).ToString());

            Task.Run(() =>
            {

                try
                {



                    List<RegistroUpdater> lista;
                    if (aplicacion != "-serv")
                    {
                        lista = getAplicacion();
                    }
                    else
                    {
                        lista = getAplicacionesServicios();
                    }


                    foreach (var updaterData in lista)
                    {
                        logu("empezando actualizar");

                        this.BeginInvoke((Action)(() =>
                        {
                            label1.Text = "Aplicacion: " + updaterData.aplicacion.ToString();
                            label2.Text = "Desde: " + updaterData.rutaDesde.ToString();
                            label3.Text = "Hasta: " + updaterData.rutaHasta.ToString() + "\\" + updaterData.ejecutable.ToString();

                        }));

                        actu.actualizar(updaterData, logInfoAction);
                        logu("terminando de actualizar");
                    }

                 
                }
                catch (Exception ex)
                {

                    logu(ex.Message); 
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    logu(ex.Message); 
                    }


                    logInfoAction("", ex.Message);
                }

            }).ContinueWith(t => { Thread.Sleep(10000); logInfoAction("","Se cierra"); Application.Exit(); });




        }

        private List<RegistroUpdater> getAplicacionesServicios()
        {


            List<RegistroUpdater> listadoCompleto =  configuracionActualizacion.getAplicacionesActualizables();

            return listadoCompleto.Where(r=>r.soloManual).ToList();

        }

        private List<RegistroUpdater> getAplicacion()
        {

            logu("Buscando que actualizar. " + configuracionActualizacion.GetType().ToString());
            RegistroUpdater updaterDataDeApp = configuracionActualizacion.getAplicacionActualizable(aplicacion);

            logu("terminando: Buscando que actualizar");

            if (updaterDataDeApp == null)
            {
                throw new Exception($"No se encontró registrada la aplicacion: {aplicacion}");
            }

            logu("invocando el proceso de actualizar");


            List<RegistroUpdater> lista = new List<RegistroUpdater>();
            lista.Add(updaterDataDeApp);

            return lista;
        }
    }
}
