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

        private string aplicacion;
        private IConfiguracionActualizacion configuracionActualizacion;
        private readonly string runId;
        private readonly string[] args;

        public FormCUpdator(string[] args)
        {
            InitializeComponent();
            this.args = args;
            this.aplicacion = args[0];

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

        public FormCUpdator()
        {
          
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

            int paso = 0;

            logu((paso++).ToString());

            Task.Run(() =>
            {




                logu("iniciando: " + string.Join(" ", args));


                this.configuracionActualizacion = ConfiguracionActualizacionFactory.getDefault();
                var actuConf = this.configuracionActualizacion;



                //try
                //{
                //    var x = actuConf.getAplicacionActualizable(args[0]);
                //    logu("empezando: " + string.Join(" ", args) + " // " + x.esServicio);
                //}
                //catch (Exception ex)
                //{
                //    logu("Error: " + ex.Message);
                //}

                //if()

                paso = 1;



                logu("Iniciando formulario con: " + aplicacion);



                ActualizacionService actu = new ActualizacionService();
                logu((paso++).ToString());

                try
                {



                    List<RegistroUpdater> lista;
                    if (aplicacion != "-serv")
                    {
                        lista = getAplicacion();
                        if (!lista.Any())
                        {
                            throw new Exception("No se encuentra registrada la aplicacion: " + this.aplicacion);
                        }
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
                            label1.Text = "Aplicacion: " + updaterData?.aplicacion?.ToString();
                            label2.Text = "Desde: " + (updaterData?.rutaDesdeLoc?.ToString() ?? updaterData?.rutaDesde?.ToString());
                            label3.Text = "Hasta: " + updaterData?.rutaHasta?.ToString() + "\\" + updaterData?.ejecutable?.ToString();

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
                finally
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        timer1.Stop();
                    progressBar1.Value = progressBar1.Maximum;
                    }));
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            int falta = progressBar1.Maximum - progressBar1.Value;

            progressBar1.Value += (int)(falta*0.005);

        }
    }
}
