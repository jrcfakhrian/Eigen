using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{
    public partial class Input_NIP : Form
    {
        public Input_NIP()
        {
            InitializeComponent();
        }

        //Hide Close Button
        private const int WS_SYSMENU = 0x80000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
        }

        string usernamewelcome = "";
        string jabat = "";
        private void tampillogin()
        {
            this.Hide();
            Login fmLogin = new Login();
            DialogResult dr = fmLogin.ShowDialog();
            if (dr != DialogResult.OK)
            {
                this.Close();
            }
            else
            {
                //this.Text = "Input NIP : " + fmLogin.usernamelogin;
                usernamewelcome = fmLogin.usernamelogin;
                this.Show();
            }

            if (usernamewelcome != "")
            {
                CariPengguna cp = new CariPengguna();

                if (cp.statusCariUser2(usernamewelcome) == true)
                {
                    DBConnect koneksi_db = new DBConnect();
                    MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
                    db.Open();
                    MySqlCommand dbcmd2 = db.CreateCommand();
                    string sql = "select user_login,Jab from user_system";
                    dbcmd2.CommandText = sql;
                    MySqlDataReader reader = dbcmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        if ((reader.GetString(0).ToString().ToLower() == usernamewelcome))
                        {
                            //label_Nama.Text = reader.GetString(1).ToString();
                            jabat = reader.GetString(1).ToString();
                            if (jabat == "Admin")
                            {
                                button_Daftar.Visible = true;
                            }
                        }
                    }
                    db.Close();
                }
            }
        }

        private void Input_NIP_Load(object sender, EventArgs e)
        {
            //button_Daftar.Visible = false;
            DBConnect koneksi_db = new DBConnect();
            MySqlConnection conn = new MySqlConnection(koneksi_db.koneksi());
            try
            {
                conn.Open();
                tampillogin();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi ke database gagal", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(ex.Message);
                Close();
            }

        }

        private void button_logout_Click(object sender, EventArgs e)
        {
            button_Daftar.Visible = false;
            tampillogin();
        }

        private void button_Masuk_Click(object sender, EventArgs e)
        {
            CariPengguna cp = new CariPengguna();
            if (cp.statuscariuser3(NIP_Textbox.Text) == true)
            {
                if (cp.statuscariuser(NIP_Textbox.Text) == false)
                {
                    FrmPrincipal frmmain = new FrmPrincipal(NIP_Textbox.Text);
                    DialogResult dr = frmmain.ShowDialog();
                    NIP_Textbox.Clear();
                    NIP_Textbox.Focus();
                }
                else
                {
                    MessageBox.Show("Anda Sudah Melakukan Absensi untuk hari ini");
                    NIP_Textbox.Clear();
                    NIP_Textbox.Focus();
                }
            }
            else
            {
                MessageBox.Show("N.I.P yang anda masukkan tidak terdaftar di database");
                NIP_Textbox.Clear();
                NIP_Textbox.Focus();
            }
        }

        private void button_Daftar_Click(object sender, EventArgs e)
        {
            DaftarPeg Daftar = new DaftarPeg();
            DialogResult dr = Daftar.ShowDialog();
        }

        private void NIP_Textbox_TextChanged(object sender, EventArgs e)
        {
            Exception X = new Exception();

            TextBox T = (TextBox)sender;

            try
            {
                if (T.Text != "-")
                {
                    int x = int.Parse(T.Text);
                }
            }
            catch (Exception)
            {
                try
                {
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception) { }
            }
        }
    }
}
