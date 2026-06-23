namespace InstaladorComanda
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnActualizarParametrosPdv = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.grpRemoto = new System.Windows.Forms.GroupBox();
            this.txtNombreRem = new System.Windows.Forms.ComboBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.cmbBaseRem = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkRemoto = new System.Windows.Forms.CheckBox();
            this.txtPosHistoria = new System.Windows.Forms.TextBox();
            this.txtEstado = new System.Windows.Forms.TextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpRemoto.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnIniciar
            // 
            this.btnIniciar.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciar.Location = new System.Drawing.Point(25, 400);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(229, 53);
            this.btnIniciar.TabIndex = 3;
            this.btnIniciar.Text = "Instalar Servicios";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnActualizarParametrosPdv
            // 
            this.btnActualizarParametrosPdv.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizarParametrosPdv.Location = new System.Drawing.Point(282, 379);
            this.btnActualizarParametrosPdv.Name = "btnActualizarParametrosPdv";
            this.btnActualizarParametrosPdv.Size = new System.Drawing.Size(431, 74);
            this.btnActualizarParametrosPdv.TabIndex = 0;
            this.btnActualizarParametrosPdv.Text = "Configurar PDV";
            this.btnActualizarParametrosPdv.UseVisualStyleBackColor = true;
            this.btnActualizarParametrosPdv.Click += new System.EventHandler(this.btnActualizarParametrosPdv_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(25, 348);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(229, 20);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(25, 374);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.Size = new System.Drawing.Size(229, 20);
            this.textBox2.TabIndex = 2;
            // 
            // grpRemoto
            // 
            this.grpRemoto.Controls.Add(this.txtNombreRem);
            this.grpRemoto.Controls.Add(this.btnConectar);
            this.grpRemoto.Controls.Add(this.cmbBaseRem);
            this.grpRemoto.Controls.Add(this.label3);
            this.grpRemoto.Controls.Add(this.label2);
            this.grpRemoto.Location = new System.Drawing.Point(282, 239);
            this.grpRemoto.Name = "grpRemoto";
            this.grpRemoto.Size = new System.Drawing.Size(431, 134);
            this.grpRemoto.TabIndex = 4;
            this.grpRemoto.TabStop = false;
            this.grpRemoto.Text = "PDV remoto";
            this.grpRemoto.Visible = false;
            this.grpRemoto.Enter += new System.EventHandler(this.grpRemoto_Enter);
            // 
            // txtNombreRem
            // 
            this.txtNombreRem.FormattingEnabled = true;
            this.txtNombreRem.Location = new System.Drawing.Point(132, 38);
            this.txtNombreRem.Name = "txtNombreRem";
            this.txtNombreRem.Size = new System.Drawing.Size(180, 21);
            this.txtNombreRem.TabIndex = 5;
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(318, 38);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(66, 20);
            this.btnConectar.TabIndex = 4;
            this.btnConectar.Text = "Connectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // cmbBaseRem
            // 
            this.cmbBaseRem.FormattingEnabled = true;
            this.cmbBaseRem.Location = new System.Drawing.Point(132, 87);
            this.cmbBaseRem.Name = "cmbBaseRem";
            this.cmbBaseRem.Size = new System.Drawing.Size(252, 21);
            this.cmbBaseRem.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Nombre base:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nombre equipo:";
            // 
            // chkRemoto
            // 
            this.chkRemoto.AutoSize = true;
            this.chkRemoto.Location = new System.Drawing.Point(282, 216);
            this.chkRemoto.Name = "chkRemoto";
            this.chkRemoto.Size = new System.Drawing.Size(135, 17);
            this.chkRemoto.TabIndex = 5;
            this.chkRemoto.Text = "Configura remotamente";
            this.chkRemoto.UseVisualStyleBackColor = true;
            this.chkRemoto.CheckedChanged += new System.EventHandler(this.chkRemoto_CheckedChanged);
            // 
            // txtPosHistoria
            // 
            this.txtPosHistoria.Location = new System.Drawing.Point(282, 12);
            this.txtPosHistoria.Multiline = true;
            this.txtPosHistoria.Name = "txtPosHistoria";
            this.txtPosHistoria.ReadOnly = true;
            this.txtPosHistoria.Size = new System.Drawing.Size(241, 184);
            this.txtPosHistoria.TabIndex = 6;
            // 
            // txtEstado
            // 
            this.txtEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEstado.Location = new System.Drawing.Point(25, 12);
            this.txtEstado.Multiline = true;
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.ReadOnly = true;
            this.txtEstado.Size = new System.Drawing.Size(229, 159);
            this.txtEstado.TabIndex = 6;
            this.txtEstado.TextChanged += new System.EventHandler(this.txtEstado_TextChanged);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(529, 12);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(184, 184);
            this.checkedListBox1.TabIndex = 7;
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Location = new System.Drawing.Point(25, 182);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(229, 154);
            this.checkedListBox2.TabIndex = 8;
            this.checkedListBox2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox2_ItemCheck);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Location = new System.Drawing.Point(282, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 441);
            this.panel1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 476);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkedListBox2);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.txtEstado);
            this.Controls.Add(this.txtPosHistoria);
            this.Controls.Add(this.chkRemoto);
            this.Controls.Add(this.grpRemoto);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnActualizarParametrosPdv);
            this.Controls.Add(this.btnIniciar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpRemoto.ResumeLayout(false);
            this.grpRemoto.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnActualizarParametrosPdv;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox grpRemoto;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.ComboBox cmbBaseRem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkRemoto;
        private System.Windows.Forms.ComboBox txtNombreRem;
        private System.Windows.Forms.TextBox txtPosHistoria;
        private System.Windows.Forms.TextBox txtEstado;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Panel panel1;
    }
}

