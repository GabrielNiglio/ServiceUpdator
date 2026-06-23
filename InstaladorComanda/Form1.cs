using CapaServicios;
using InstaladorComanda.Herramientas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Xml.Linq;
using CapaServicios.Classes;

namespace InstaladorComanda
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        private void EjecutarTareaAsync(Action accion)
        {
            Action habilitar = () =>
            {

                this.BeginInvoke((Action)(() =>
                {
                    btnActualizarParametrosPdv.Enabled = true;
                    btnIniciar.Enabled = true;
                    btnConectar.Enabled = true;
                }));

            };
            Action deshabilitar = () =>
            {
                this.BeginInvoke((Action)(() =>
                {
                    btnActualizarParametrosPdv.Enabled = false;
                    btnIniciar.Enabled = false;
                    btnConectar.Enabled = false;
                }));
            };



            deshabilitar();

            Task.Run(() =>
            {
                try { accion(); }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                        mensaje += " - " + e.Message;
                    }


                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }).ContinueWith(t => habilitar());
        }



        private void btnIniciar_Click(object sender, EventArgs e)
        {

            string texto1 = textBox1.Text;
            string texto2 = textBox2.Text;

            textBox1.Text = "";
            textBox2.Text = "";

            if (texto1 != "server123" || texto1 != texto2)
            {
                MessageBox.Show("SE ESTA TRANTANDO DE INSTALAR SERVICIOS, INGRESE LA CLAVE DOS VECES");
                return;
            }

            ConfiguradorCinet configurador = new ConfiguradorCinet();
            ConfiguracionActualizacionFromBackoffice configUpd = new ConfiguracionActualizacionFromBackoffice();
            CinetDownloader downloader = new CinetDownloader();
            ActualizacionService actu = new ActualizacionService();



            EjecutarTareaAsync(() =>
            {

                configurador.crearArchivoConfigConexiones();

                configurador.crearTablas();

                var lista = new List<string>();
                foreach (var item in checkedListBox2.CheckedItems)
                {
                    lista.Add(item.ToString().ToUpper());
                }

                configurador.insertRegUpdater(lista);

                List<RegistroUpdater> appActualizables = configUpd.getAplicacionesActualizables();

                IEnumerable<RegistroUpdater> registros = appActualizables
                    .Where(a => lista.Contains(a.aplicacion.ToUpper()));

                foreach (var registro in registros)
                {
                    downloader.bajarAplicativo(registro, logAction).Wait();
                }


                foreach (var registro in registros)
                {
                    actu.actualizar(registro, logAction);
                }

                if (lista.Contains("WSCOMANDA"))
                {
                    configurador.modificarAppsettingsWSComanda();
                    configurador.insertComandas();
                    configurador.insertarParametrosWSComanda();
                }

                System.Diagnostics.Process.Start("http://localhost:8050");

            });




        }

        private void logAction(string arg1, string arg2)
        {



            this.BeginInvoke((Action)(() =>
            {

                if (arg1 == "W")
                {
                    MessageBox.Show(arg2);
                }
                else
                {

                    txtEstado.Text = arg1 + " " + arg2;
                }

            }));
        }

        private void btnActualizarParametrosPdv_Click(object sender, EventArgs e)
        {
            //    Process p = new Process();
            //    p.StartInfo.FileName = $"sc";
            //    p.StartInfo.UseShellExecute = false;
            //    p.StartInfo.CreateNoWindow = true;
            //    p.StartInfo.RedirectStandardOutput = true;
            //    p.StartInfo.RedirectStandardError = true;
            //    p.StartInfo.Arguments = $"create xd binpath=xd start= auto";

            //    logAction("Actualizacion", "Creando servicio");



            //    // Manejar el evento de salida de error
            //    //p.ErrorDataReceived += (xsender, xe) =>
            //    //{

            //    //    string message =+ xe.Data;
            //    //    logAction("W", message);

            //    //};

            //    p.Start();

            ////    p.BeginOutputReadLine();
            //    p.BeginErrorReadLine();




            string nombreServerRem = txtNombreRem.Text;
            string nombreBaseRem = cmbBaseRem.Text;

            EjecutarTareaAsync(() =>
            {
                logAction("X", "Se inicia la actualizacion de parametros...");

                ConfiguradorCinet configurador = new ConfiguradorCinet();
                if (chkRemoto.Checked)
                {
                    configurador.actualizarParametrosPdv(true, nombreServerRem, nombreBaseRem);
                }
                else
                {
                    configurador.actualizarParametrosPdv();
                }

                this.BeginInvoke((Action)(() =>
                {
                    txtNombreRem.Text = "";
                    cmbBaseRem.Text = "";
                    cmbBaseRem.DataSource = new List<string>();
                    txtPosHistoria.Text = nombreServerRem + " (" + nombreBaseRem + ")" + "\r\n" + txtPosHistoria.Text;

                    string nomEquipoRemoto = nombreServerRem.Split('\\')[0].Trim().Split(',')[0].Trim();
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        var item = checkedListBox1.Items[i];
                        if (nomEquipoRemoto.Contains(item.ToString().Trim()))
                        {

                            checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                        }


                    }


                }));

                logAction("X", "ACTALIZACION DE PARAMETROS FINALIZADA!");
            });



        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            string rutaServer = txtNombreRem.Text;
            EjecutarTareaAsync(() =>
            {
                ConfiguradorCinet configurador = new ConfiguradorCinet();
                List<string> listadoBases = configurador.getBasesPdv(rutaServer);
                this.BeginInvoke((Action)(() =>
                {
                    cmbBaseRem.DataSource = listadoBases;
                }));
            });
        }

        private void chkRemoto_CheckedChanged(object sender, EventArgs e)
        {
            bool isRemoto = chkRemoto.Checked;
            grpRemoto.Visible = isRemoto;

            EjecutarTareaAsync(() =>
            {
                if (isRemoto)
                {

                    ConfiguradorCinet configurador = new ConfiguradorCinet();

                    List<string> getEquipos = configurador.getEquiposLocal();

                    this.BeginInvoke((Action)(() =>
                    {

                        if (checkedListBox1.Items.Count == 0)
                        {
                            checkedListBox1.SelectionMode = SelectionMode.None;


                            foreach (string item in getEquipos)
                            {

                                checkedListBox1.Items.Add(item);
                            }

                        }

                        txtNombreRem.DataSource = getEquipos;
                    }));

                }
            });
        }

        private class AplJsonConf
        {
            public List<Apl> aplicativos { get; set; }
        }


        private class Apl
        {
            public string aplciacion { get; set; }
            public bool check { get; set; }

            public Apl(string aplciacion, bool check)
            {
                this.aplciacion = aplciacion;
                this.check = check;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Instalador Automatico v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            List<Apl> json;
            try
            {

                using (StreamReader sr = new StreamReader("aplicativos.json"))
                {
                    string sJson = sr.ReadToEnd();
                    json = JsonConvert.DeserializeObject<AplJsonConf>(sJson).aplicativos;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hay un error con el archivo aplicacion.json: "+ex.Message+ "\n\n¿Ha revisado el archivo?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                json = new List<Apl> {
                    new Apl("WSUpdatorLauncher", true),
                    new Apl("WSComanda", true),
                    new Apl("ServiceUpdator", true),
                    new Apl("CUpdator", true)
                };
            }
            foreach (Apl apl in json)
            {

                checkedListBox2.Items.Add(apl.aplciacion, apl.check);
            }







        }

        private void txtEstado_TextChanged(object sender, EventArgs e)
        {

        }

        private void grpRemoto_Enter(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
      
        }
    }
}
