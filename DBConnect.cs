using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiFaceRec
{
    class DBConnect
    {
        string host = "localhost";
        string user = "root";
        string database = "absensi_peg";
        string password = "";

        public string koneksi()
        {
            return "server=" + host + "; user=" + user + "; database=" + database + "; password=" + password + ";";
        }
    }
}
