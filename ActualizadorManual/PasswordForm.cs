using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActualizadorManual
{
    public partial class PasswordForm : Form
    {

        public Action accionAlFinalizar;



        public PasswordForm()
        {
            InitializeComponent();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("cmos1212"))
            {

                this.accionAlFinalizar();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Clave invalida","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void PasswordForm_Shown(object sender, EventArgs e)
        {

            textBox1.Focus();
        }
    }
}
