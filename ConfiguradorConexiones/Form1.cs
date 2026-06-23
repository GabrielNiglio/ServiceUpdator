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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IntentarLogin();
        }

        private void IntentarLogin()
        {
            if (textBox1.Text == "server123")
            {
                panel1.Visible = false;
                this.menuStrip1.Visible = true;
            }
            else
            {
                textBox1.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void conexioneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form nuevo = new FormConfigurarConexiones();

            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           if( e.KeyChar == 13)
            {

            this.IntentarLogin();
            }
        }
    }
}
