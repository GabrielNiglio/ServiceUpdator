using CapaServicios;
using CapaServicios.Classes;
using CapaServicios.Intefaces;
using CinetConfigurationSerices;
using CinetConfigurationSerices.Interfaces;
using CinetFileLogger;
using CinetInstalacionServices;
using CinetInstalacionServices.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ActualizadorManual
{
    public partial class FormPrincipal : Form
    {


        private readonly IConfiguracionActualizacion configuracion;// = new ConfiguracionActualizacionFromApi();


        private readonly ActualizacionService actu;// = new ActualizacionService();
        private readonly cLog logger;

        public object IEnviromentConfigurationHelper { get; private set; }

        public FormPrincipal(IConfiguracionActualizacion configuracion, ActualizacionService actu, cLog log) : this()
        {
            this.configuracion = configuracion;
            this.actu = actu;
            this.logger = log;
        }

        private FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnProfit_Click(object sender, EventArgs e)
        {




            Task task = new Task(() =>
            {
                actualizarAplicacion("profit");
            });

            task.Start();

        }



        private Action<string, string> getAccionLog(string appName)
        {

            Action<string, string> registrarEstado = (string tarea, string texto) =>
            {
                this.BeginInvoke((Action)(() =>
                {
                    label1.Text = texto;
                }));
                if (tarea != null)
                {
                    logger.EscribeLog(tarea + "-" + appName, texto);
                }

            };

            return registrarEstado;
        }





        private void actualizarAplicacion(string nombreAplicacion)
        {

            RegistroUpdater reg = configuracion.getAplicacionActualizable(nombreAplicacion);


            Action<string, string> registrarEstado = (string tarea, string texto) =>
            {
                this.BeginInvoke((Action)(() =>
                {
                    label1.Text = texto;
                }));

                logger.EscribeLog(tarea + "-" + reg.aplicacio, texto);

            };

            actu.actualizar(reg, registrarEstado);

        }



        private void btnMozos_Click(object sender, EventArgs e)
        {

            RegistroUpdater reg = new RegistroUpdater()
            {
                aplicacio = "AMozosApi",
                ejecutable = "MozosApi.exe",
                esServicio = true,
                rutaHasta = "C:\\cinet\\CinetMozos",
                rutaDesde = "C:\\CINET\\ACTUALIZACIONES\\RUTA1\\CinetMozos.zip"
            };

            var accionLog = this.getAccionLog(reg.aplicacio);

            EjecutarTareaAsync(() => actu.actualizar(reg, accionLog));







        }

        private void btnAutoCompletar_Click(object sender, EventArgs e)
        {
            txtBko.Text = txtServer.Text + (chkBkoLocal.Checked ? "\\" + cmdInstanciasLocales.Text : ",1433");
            txtComanda.Text = txtServer.Text + (chkComaLocal.Checked ? "\\" + cmdInstanciasLocales.Text : ",1434");

        }

        private void btnODBC_Click(object sender, EventArgs e)
        {
            string rutaComanda = txtComanda.Text;
            string rutaBackoffice = txtBko.Text;

            IEnviromentConfigurationHelper env = new WindowsConfigurationHelper();

            string rutaLocal = env.getNombreEquipo() + "\\" + cmdInstanciasLocales.Text;


            env.setODBC("backoffice", rutaBackoffice, "backoffice");
            env.setODBC("comanda", rutaComanda, "comanda");
            env.setODBC("zonaentrega", rutaComanda, "comanda");
            env.setODBC("cinet_pdv", rutaLocal, "backoffice");
            env.setODBC("sqlempresas", rutaLocal, "empresas");
        }

        private void btnUpdater_Click(object sender, EventArgs e)
        {
            IEnviromentConfigurationHelper env = new WindowsConfigurationHelper();


            List<string> aplicativos = configuracion.getNombresAplicaciones();

            aplicativos.ForEach(a =>
            {
                env.setUpdaterReg("RUTA1", a, txtServer.Text);
            });

        }


        private void EjecutarTareaAsync(Action accion)
        {
            deshabilitar();
            Task.Run(() =>
            {
                try { accion(); }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }).ContinueWith(t => habilitar());
        }


        private void btnBajar_Click(object sender, EventArgs e)
        {

            IFileDownloadService fDonwload = new TestFileDownload();

            EjecutarTareaAsync(() =>
            {

                foreach (var chk in chkListApps.CheckedItems)
                {
                    string appName = chk.ToString();
                    var accionLog = getAccionLog(appName);

                    fDonwload.bajarAplicativo(appName, accionLog).Wait();
                }

            });


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " v" + Assembly.GetExecutingAssembly().GetName().Version;



            List<string> listaApps = configuracion.getNombresAplicaciones();

            foreach (string app in listaApps)
            {
                chkListApps.Items.Add(app);
            }

            IEnviromentConfigurationHelper env = new WindowsConfigurationHelper();

            var instancias = env.getInstanciasSqlServer().Result;

            cmdInstanciasLocales.DataSource = instancias;


            this.Enabled = false;




        }

        private void habilitar()
        {
            this.BeginInvoke((Action)(() =>
            {
                Enabled = true;
                Show();
            }));
        }

        private void deshabilitar()
        {
            this.BeginInvoke((Action)(() =>
            {
                Enabled = false;
            }));
        }

        private void btnActuCostos_Click(object sender, EventArgs e)
        {

            EjecutarTareaAsync(() =>
            {

                foreach (var chk in chkListApps.CheckedItems)
                {
                    string appName = chk.ToString();

                    actualizarAplicacion(appName);
                }

            });
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();


            PasswordForm passwordForm = new PasswordForm();
            passwordForm.accionAlFinalizar = habilitar;
            passwordForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IEnviromentConfigurationHelper env = new WindowsConfigurationHelper();
            txtServer.Text = env.getNombreEquipo();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                // Configura el User-Agent si es necesario
                client.Headers.Add("User-Agent", "PitufoGato/1.0");

                string url = "https://mostaza-app-cinet.s3.us-west-2.amazonaws.com/locales-propios/DescargaLocal.zip";


                // Realiza la solicitud con WebClient

                Int64 bytes_esperados = 0;
                using (var readder = client.OpenRead(url))
                {
                    bytes_esperados = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                }

                string filename = "parche.zip";

                client.DownloadFile(url, filename);

                FileInfo fi = new FileInfo(filename);

                long size = fi.Length;



                MessageBox.Show($"Bajado: {size} / Esperado: {bytes_esperados}  Correcto: {size == bytes_esperados} ");

            }


            // System.Net.WebClient client = new System.Net.WebClient(); client.Headers.Add("user-agent", " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");

        }
    }
}
