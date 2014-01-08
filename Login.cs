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
    public partial class Login : Form
    {
        private string _user;
        public string usernamelogin
        {
            get { return _user; }
        }

        public Login()
        {
            InitializeComponent();
        }

        private Boolean statuslogin(string user, string password)
        {

            user = user.ToLower();
            password = password.ToLower();
            DBConnect koneksi_db = new DBConnect();
            MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
            db.Open();
            MySqlCommand dbcmd = db.CreateCommand();
            string sql = "select user_login,password_login from user_system";
            dbcmd.CommandText = sql;
            MySqlDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                if ((reader.GetString(0).ToString().ToLower() == user) && (reader.GetString(1).ToString().ToLower() == password))
                {
                    return true;
                }
            }
            db.Close();
            return false;
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            if (usernametextbox.Text != "" && passwordtextbox.Text != "")
            {
                string user = usernametextbox.Text;
                md5 buat_enkripsi = new md5();
                string pass = buat_enkripsi.GetMD5(passwordtextbox.Text);
                if (statuslogin(user, pass) == true)
                {
                    this.DialogResult = DialogResult.OK;
                    _user = user;
                    _user = usernamelogin;
                }
                else
                {
                    MessageBox.Show("Username atau password salah");
                    usernametextbox.Clear();
                    passwordtextbox.Clear();
                    usernametextbox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Ada field yang kosong");
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString("dd/MM/yyyy");
            usernametextbox.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString("HH:mm:ss");
            //MessageBox.Show("Tick");
            
        }
    }
}
