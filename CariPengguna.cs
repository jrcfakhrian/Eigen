using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{
    class CariPengguna
    {
        public Boolean statuscariuser(string nim)
        {
            string waktu_awal, waktu_akhir;

            waktu_awal = DateTime.Now.ToString("yyyy-MM-dd 01:00:00");
            waktu_akhir = DateTime.Now.ToString("yyyy-MM-dd 23:00:00");
            DBConnect koneksi_db = new DBConnect();
            MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
            db.Open();
            MySqlCommand dbcmd = db.CreateCommand();
            string sql = "select nim from absensi where nim='" + nim + "' and tgl_absen between '" + waktu_awal + "' and '" + waktu_akhir + "'";
            dbcmd.CommandText = sql;
            MySqlDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                if ((reader.GetString(0).ToString() == nim))
                {
                    return true;
                }
            }
            db.Close();
            return false;
        }

        public Boolean statusCariUser2(string user)
        {
            DBConnect koneksi_db = new DBConnect();
            MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
            db.Open();
            MySqlCommand dbcmd = db.CreateCommand();
            string sql = "select user_login from user_system where user_login='" + user + "'";
            dbcmd.CommandText = sql;
            MySqlDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                if ((reader.GetString(0).ToString() == user))
                {
                    return true;
                }
            }
            db.Close();
            return false;
        }

        public Boolean statuscariuser3(string nim)
        {
            DBConnect koneksi_db = new DBConnect();
            MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
            db.Open();
            MySqlCommand dbcmd = db.CreateCommand();
            string sql = "select nim from data_peg where nim='" + nim + "'";
            dbcmd.CommandText = sql;
            MySqlDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                if ((reader.GetString(0).ToString() == nim))
                {
                    return true;
                }
            }
            db.Close();
            return false;
        }

        public Boolean statuscariuser4(string nama)
        {
            DBConnect koneksi_db = new DBConnect();
            MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
            db.Open();
            MySqlCommand dbcmd = db.CreateCommand();
            string sql = "select nim,nama from data_peg where nama='" + nama + "'";
            dbcmd.CommandText = sql;
            MySqlDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                if ((reader.GetString(0).ToString() == nama))
                {
                    return true;
                }
            }
            db.Close();
            return false;
        }
    }
}
