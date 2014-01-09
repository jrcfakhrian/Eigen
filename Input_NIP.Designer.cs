namespace MultiFaceRec
{
    partial class Input_NIP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.NIP_Textbox = new System.Windows.Forms.TextBox();
            this.button_Masuk = new System.Windows.Forms.Button();
            this.button_logout = new System.Windows.Forms.Button();
            this.button_Daftar = new System.Windows.Forms.Button();
            this.Button_Absensi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input Your N.I.P ";
            // 
            // NIP_Textbox
            // 
            this.NIP_Textbox.Location = new System.Drawing.Point(12, 99);
            this.NIP_Textbox.MaxLength = 10;
            this.NIP_Textbox.Name = "NIP_Textbox";
            this.NIP_Textbox.Size = new System.Drawing.Size(151, 20);
            this.NIP_Textbox.TabIndex = 1;
            this.NIP_Textbox.TextChanged += new System.EventHandler(this.NIP_Textbox_TextChanged);
            // 
            // button_Masuk
            // 
            this.button_Masuk.Location = new System.Drawing.Point(171, 72);
            this.button_Masuk.Name = "button_Masuk";
            this.button_Masuk.Size = new System.Drawing.Size(101, 23);
            this.button_Masuk.TabIndex = 2;
            this.button_Masuk.Text = "Masuk";
            this.button_Masuk.UseVisualStyleBackColor = true;
            this.button_Masuk.Click += new System.EventHandler(this.button_Masuk_Click);
            // 
            // button_logout
            // 
            this.button_logout.Location = new System.Drawing.Point(12, 12);
            this.button_logout.Name = "button_logout";
            this.button_logout.Size = new System.Drawing.Size(54, 23);
            this.button_logout.TabIndex = 3;
            this.button_logout.Text = "Keluar";
            this.button_logout.UseVisualStyleBackColor = true;
            this.button_logout.Click += new System.EventHandler(this.button_logout_Click);
            // 
            // button_Daftar
            // 
            this.button_Daftar.Enabled = false;
            this.button_Daftar.Location = new System.Drawing.Point(157, 12);
            this.button_Daftar.Name = "button_Daftar";
            this.button_Daftar.Size = new System.Drawing.Size(115, 23);
            this.button_Daftar.TabIndex = 4;
            this.button_Daftar.Text = "Daftar Pengguna";
            this.button_Daftar.UseVisualStyleBackColor = true;
            this.button_Daftar.Click += new System.EventHandler(this.button_Daftar_Click);
            // 
            // Button_Absensi
            // 
            this.Button_Absensi.Location = new System.Drawing.Point(72, 12);
            this.Button_Absensi.Name = "Button_Absensi";
            this.Button_Absensi.Size = new System.Drawing.Size(79, 23);
            this.Button_Absensi.TabIndex = 5;
            this.Button_Absensi.Text = "Absensi";
            this.Button_Absensi.UseVisualStyleBackColor = true;
            this.Button_Absensi.Click += new System.EventHandler(this.Button_Absensi_Click);
            // 
            // Input_NIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 48);
            this.Controls.Add(this.Button_Absensi);
            this.Controls.Add(this.button_Daftar);
            this.Controls.Add(this.button_logout);
            this.Controls.Add(this.button_Masuk);
            this.Controls.Add(this.NIP_Textbox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Input_NIP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Absensi";
            this.Load += new System.EventHandler(this.Input_NIP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NIP_Textbox;
        private System.Windows.Forms.Button button_Masuk;
        private System.Windows.Forms.Button button_logout;
        private System.Windows.Forms.Button button_Daftar;
        private System.Windows.Forms.Button Button_Absensi;
    }
}