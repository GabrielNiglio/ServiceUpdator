namespace ActualizadorManual
{
    partial class FormPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.btnProfit = new System.Windows.Forms.Button();
            this.btnMozos = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBko = new System.Windows.Forms.TextBox();
            this.txtComanda = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAutoCompletar = new System.Windows.Forms.Button();
            this.btnODBC = new System.Windows.Forms.Button();
            this.btnUpdater = new System.Windows.Forms.Button();
            this.btnBajar = new System.Windows.Forms.Button();
            this.cmdInstanciasLocales = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chkBkoLocal = new System.Windows.Forms.CheckBox();
            this.chkComaLocal = new System.Windows.Forms.CheckBox();
            this.chkListApps = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnProfit
            // 
            this.btnProfit.Enabled = false;
            this.btnProfit.Location = new System.Drawing.Point(210, 391);
            this.btnProfit.Name = "btnProfit";
            this.btnProfit.Size = new System.Drawing.Size(182, 57);
            this.btnProfit.TabIndex = 0;
            this.btnProfit.Text = "Abrir y actualizar profit";
            this.btnProfit.UseVisualStyleBackColor = true;
            this.btnProfit.Click += new System.EventHandler(this.btnProfit_Click);
            // 
            // btnMozos
            // 
            this.btnMozos.Location = new System.Drawing.Point(25, 391);
            this.btnMozos.Name = "btnMozos";
            this.btnMozos.Size = new System.Drawing.Size(179, 57);
            this.btnMozos.TabIndex = 1;
            this.btnMozos.Text = "Actualizar Mozos";
            this.btnMozos.UseVisualStyleBackColor = false;
            this.btnMozos.Click += new System.EventHandler(this.btnMozos_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // txtBko
            // 
            this.txtBko.Location = new System.Drawing.Point(107, 126);
            this.txtBko.Name = "txtBko";
            this.txtBko.Size = new System.Drawing.Size(256, 20);
            this.txtBko.TabIndex = 3;
            // 
            // txtComanda
            // 
            this.txtComanda.Location = new System.Drawing.Point(107, 154);
            this.txtComanda.Name = "txtComanda";
            this.txtComanda.Size = new System.Drawing.Size(256, 20);
            this.txtComanda.TabIndex = 4;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(107, 70);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(256, 20);
            this.txtServer.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Nombre Server";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ruta Backoffice";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ruta Comanda";
            // 
            // btnAutoCompletar
            // 
            this.btnAutoCompletar.Location = new System.Drawing.Point(436, 126);
            this.btnAutoCompletar.Name = "btnAutoCompletar";
            this.btnAutoCompletar.Size = new System.Drawing.Size(72, 47);
            this.btnAutoCompletar.TabIndex = 7;
            this.btnAutoCompletar.Text = "Auto Completar";
            this.btnAutoCompletar.UseVisualStyleBackColor = true;
            this.btnAutoCompletar.Click += new System.EventHandler(this.btnAutoCompletar_Click);
            // 
            // btnODBC
            // 
            this.btnODBC.Location = new System.Drawing.Point(54, 195);
            this.btnODBC.Name = "btnODBC";
            this.btnODBC.Size = new System.Drawing.Size(142, 40);
            this.btnODBC.TabIndex = 8;
            this.btnODBC.Text = "Configurar ODBCs";
            this.btnODBC.UseVisualStyleBackColor = true;
            this.btnODBC.Click += new System.EventHandler(this.btnODBC_Click);
            // 
            // btnUpdater
            // 
            this.btnUpdater.Location = new System.Drawing.Point(241, 195);
            this.btnUpdater.Name = "btnUpdater";
            this.btnUpdater.Size = new System.Drawing.Size(151, 40);
            this.btnUpdater.TabIndex = 9;
            this.btnUpdater.Text = "Configurar Updater";
            this.btnUpdater.UseVisualStyleBackColor = true;
            this.btnUpdater.Click += new System.EventHandler(this.btnUpdater_Click);
            // 
            // btnBajar
            // 
            this.btnBajar.Location = new System.Drawing.Point(648, 304);
            this.btnBajar.Name = "btnBajar";
            this.btnBajar.Size = new System.Drawing.Size(119, 53);
            this.btnBajar.TabIndex = 10;
            this.btnBajar.Text = "Bajar aplicaciones";
            this.btnBajar.UseVisualStyleBackColor = true;
            this.btnBajar.Click += new System.EventHandler(this.btnBajar_Click);
            // 
            // cmdInstanciasLocales
            // 
            this.cmdInstanciasLocales.FormattingEnabled = true;
            this.cmdInstanciasLocales.Location = new System.Drawing.Point(107, 96);
            this.cmdInstanciasLocales.Name = "cmdInstanciasLocales";
            this.cmdInstanciasLocales.Size = new System.Drawing.Size(256, 21);
            this.cmdInstanciasLocales.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Instancia Local";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(648, 363);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 62);
            this.button1.TabIndex = 14;
            this.button1.Text = "Actualizar y abrir aplicativos";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnActuCostos_Click);
            // 
            // chkBkoLocal
            // 
            this.chkBkoLocal.AutoSize = true;
            this.chkBkoLocal.Location = new System.Drawing.Point(370, 128);
            this.chkBkoLocal.Name = "chkBkoLocal";
            this.chkBkoLocal.Size = new System.Drawing.Size(52, 17);
            this.chkBkoLocal.TabIndex = 15;
            this.chkBkoLocal.Text = "Local";
            this.chkBkoLocal.UseVisualStyleBackColor = true;
            // 
            // chkComaLocal
            // 
            this.chkComaLocal.AutoSize = true;
            this.chkComaLocal.Location = new System.Drawing.Point(371, 156);
            this.chkComaLocal.Name = "chkComaLocal";
            this.chkComaLocal.Size = new System.Drawing.Size(52, 17);
            this.chkComaLocal.TabIndex = 16;
            this.chkComaLocal.Text = "Local";
            this.chkComaLocal.UseVisualStyleBackColor = true;
            // 
            // chkListApps
            // 
            this.chkListApps.CheckOnClick = true;
            this.chkListApps.FormattingEnabled = true;
            this.chkListApps.Location = new System.Drawing.Point(629, 9);
            this.chkListApps.Name = "chkListApps";
            this.chkListApps.Size = new System.Drawing.Size(159, 289);
            this.chkListApps.TabIndex = 17;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(369, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Es server";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(347, 304);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 19;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.chkListApps);
            this.Controls.Add(this.chkComaLocal);
            this.Controls.Add(this.chkBkoLocal);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmdInstanciasLocales);
            this.Controls.Add(this.btnBajar);
            this.Controls.Add(this.btnUpdater);
            this.Controls.Add(this.btnODBC);
            this.Controls.Add(this.btnAutoCompletar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.txtComanda);
            this.Controls.Add(this.txtBko);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMozos);
            this.Controls.Add(this.btnProfit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPrincipal";
            this.Text = "Actualizador Manual";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProfit;
        private System.Windows.Forms.Button btnMozos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBko;
        private System.Windows.Forms.TextBox txtComanda;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAutoCompletar;
        private System.Windows.Forms.Button btnODBC;
        private System.Windows.Forms.Button btnUpdater;
        private System.Windows.Forms.Button btnBajar;
        private System.Windows.Forms.ComboBox cmdInstanciasLocales;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkBkoLocal;
        private System.Windows.Forms.CheckBox chkComaLocal;
        private System.Windows.Forms.CheckedListBox chkListApps;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

