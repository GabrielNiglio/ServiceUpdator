using ConexionesManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfiguradorConexiones
{
    public partial class FormConfigurarConexiones : Form
    {

        private List<string> nombresConexiones;

        GestorConexiones gestorConexiones = new GestorConexiones();


        public FormConfigurarConexiones()
        {
            InitializeComponent();
        }

        private void FormConfigurarConexiones_Load(object sender, EventArgs e)
        {

            var conexiones = gestorConexiones.leerConexiones();

            nombresConexiones = conexiones.Keys.ToList();

            comboBox1.DataSource = nombresConexiones;




        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombreConexionSeleccionada = comboBox1.SelectedItem.ToString();

            var ConexionSelecciondad = gestorConexiones.leerConexiones()[nombreConexionSeleccionada];

            txtServer.Text = ConexionSelecciondad.server;
            txtDatabase.Text = ConexionSelecciondad.database;
            txtPassword.Text = ConexionSelecciondad.password.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string nombreConexionSeleccionada = comboBox1.SelectedItem.ToString();

                var Conexiones = gestorConexiones.leerConexiones();

                var ConexionSelecciondad = Conexiones[nombreConexionSeleccionada];



                ConexionSelecciondad.server = txtServer.Text;
                ConexionSelecciondad.database = txtDatabase.Text;
                ConexionSelecciondad.password = int.Parse(txtPassword.Text);

                gestorConexiones.guardarConexiones(Conexiones);

                MessageBox.Show("Conexión guardada correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la conexión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
