
//Multiple face detection and recognition in real time
//Using EmguCV cross platform .Net wrapper to the Intel OpenCV image processing library for C#.Net
//Writed by Sergio Andrés Guitérrez Rojas
//"Serg3ant" for the delveloper comunity
// Sergiogut1805@hotmail.com
//Regards from Bucaramanga-Colombia ;)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels= new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;
        public bool ketemu = true;

        public string nip_pegawai;
        public string path_trainingface;
        public string folder_trainingface;
        public FrmPrincipal(string nip, string path, string folder)
        {
            InitializeComponent();
            nip_pegawai = nip;
            path_trainingface = path;
            folder_trainingface = folder;
            load_traineface(nip, path, folder);
        }

        private void load_traineface(string nip, string path, string folder)
        {
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                //string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces2/TrainedLabels" + nip + ".txt");
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/" + folder + "/TrainedLabels" + path + ".txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;


                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face_" + nip_pegawai + "" + tf + ".bmp";
                    //trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/" + folder + "/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Tidak ada data training wajah untuk N.I.P " + nip_pegawai + ".", "Load Face Training", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ketemu = false;
                button_SimpanAbsen.Enabled = false;
            }
        }

        //private void input_absen(string identik)
        private void input_absen()
        {
            string sql;

            DBConnect koneksi_db = new DBConnect();
            MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
            db.Open();
            MySqlCommand dbcmd = db.CreateCommand();
            sql = "insert into absensi values (null,'" + Label_NIP.Text + "',now(),'" + Label_Nama.Text + "');";
            dbcmd.CommandText = sql;
            MySqlDataAdapter sqladapter = new MySqlDataAdapter(sql, koneksi_db.koneksi());
            DataSet mydataset = new DataSet();
            sqladapter.Fill(mydataset);
            //id_pembayaran = Convert.ToInt32(dbcmd.ExecuteScalar());
            db.Close();
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            label4.Text = "Tidak Ada";
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);

                    name = recognizer.Recognize(result);

                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                NamePersons[t - 1] = name;
                NamePersons.Add("");


                //Set the number of faces detected on the scene
                label3.Text = facesDetected[0].Length.ToString();
            }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                //names = names + NamePersons[nnn] + ", ";
                names = names + NamePersons[nnn];
            }

            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            label4.Text = names;
            if (label4.Text != "")
            {
                Label_Nama.Text = label4.Text;
                //Label_NIP.Text = "test";

                DBConnect koneksi_db = new DBConnect();
                MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
                db.Open();
                MySqlCommand dbcmd = db.CreateCommand();
                string sql = "select nim from data_peg where nama like'%" + label4.Text + "%'";
                dbcmd.CommandText = sql;
                MySqlDataReader reader = dbcmd.ExecuteReader();
                while (reader.Read())
                {
                    Label_NIP.Text = reader.GetString(0).ToString();
                }
                db.Close();
                //button_SimpanAbsen.Enabled = true;
                //label11.Visible = false;
            }
            else
            {
                //button_SimpanAbsen.Enabled = false;
                //label11.Visible = true;
            }

            names = "";

            //ketemu = true;

            //Clear the list(vector) of names
            NamePersons.Clear();

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string host = "localhost";
            string user = "root";
            string password = "";
            string database = "absensi_peg";
            string connStr = "server=" + host + ";user=" + user + ";database=" + database + ";password=" + password + ";";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MessageBox.Show("Koneksi berhasil");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            if (nip_pegawai != "1")
            {
                Label_NIP.Text = nip_pegawai;
                this.Text = "Absensi Karyawan N.I.P : " + nip_pegawai;
            }
            else
            {
                Label_NIP.Text = "-";
                this.Text = "Absensi Karyawam";
            }
            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
            label7.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void button_SimpanAbsen_Click(object sender, EventArgs e)
        {
            CariPengguna cp = new CariPengguna();
            if (Label_NIP.Text != "-" && Label_Nama.Text != "-")
            {
                if (cp.statuscariuser3(Label_NIP.Text) == true)
                {
                    if (cp.statuscariuser(Label_NIP.Text) == false)
                    {
                        label11.Visible = false;
                        try
                        {
                            //Trained face counter
                            ContTrain = ContTrain + 1;

                            //Get a gray frame from capture device
                            gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                            //Face Detector
                            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                            face,
                            1.2,
                            10,
                            Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                            new Size(20, 20));

                            //Action for each element detected
                            foreach (MCvAvgComp f in facesDetected[0])

                            //for (int i = 1; i<=1; i++)
                            {
                                TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                                break;
                            }

                            //resize face detected image for force to compare the same size with the 
                            //test image with cubic interpolation type method
                            TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                            if (ketemu == true)
                            {
                                //CariPengguna cp = new CariPengguna();

                                if (cp.statuscariuser(nip_pegawai) == false)
                                {
                                    //string identik_dgn = "face_" + nip_pegawai + "_" + label6.Text + "";
                                    //input_absen(identik_dgn);
                                    input_absen();
                                    ketemu = false;
                                }
                            }

                            MessageBox.Show("Data Absensi " + Label_Nama.Text + " Berhasil Disimpan", "Save to Database OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Wajah tidak terdeteksi", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Anda Sudah Melakukan Absensi untuk hari ini","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Wajah Anda Sulit Di Identifikasi, Silahkan Coba Untuk Mencari NIP Anda","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                Txt_NIP.Focus();
            }  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Btn_CariNIP_Click(object sender, EventArgs e)
        {
            CariPengguna cp = new CariPengguna();
            if (cp.statuscariuser3(Txt_NIP.Text) == true)
            {
                string nip_baru = Txt_NIP.Text;
                string path_baru = nip_baru;
                string folder_baru = "TrainedFaces";
                //load_traineface(nip_baru);
                FrmPrincipal frm = new FrmPrincipal(nip_baru, path_baru, folder_baru);
                this.Close();
                frm.Show();
            }
            else
            {
                MessageBox.Show("N.I.P yang anda masukkan tidak terdaftar di database");
                Txt_NIP.Clear();
                Txt_NIP.Focus();
            }
        }

        private void Txt_NIP_TextChanged(object sender, EventArgs e)
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