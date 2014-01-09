using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{

    

    public partial class DaftarPeg : Form
    {
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

        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<Image<Gray, byte>> trainingImages2 = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;
        public bool ketemu = false;
        public string NIP_Enroll, Nama_Enroll;

        public DaftarPeg()
        {
            InitializeComponent();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            eye = new HaarCascade("haarcascade_eye.xml");

            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces2/TrainedLabelsAll.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face_1" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces2/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                //MessageBox.Show("Persiapan untuk memasukkan Data Wajah  " + NIP_Enroll + ".", "Load Face Training", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            //label4.Text = "";
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
                //ketemu = true;
            }

            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            //label41.Text = names;
            names = "";



            //Clear the list(vector) of names
            NamePersons.Clear();

        }

        private void button_Batal_Click(object sender, EventArgs e)
        {
            textBox_Nama.Clear();
            textBox_Alamat.Clear();
            textBox_NIP.Clear();
            textBox_TmpLhr.Clear();
            textBox_Jabatan.Clear();
            comboBox_PendAkhir.ResetText();
            dateTimePicker_TglLhr.ResetText();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private void button_Enroll_Click(object sender, EventArgs e)
        {
            CariPengguna cp = new CariPengguna();
            if (cp.statuscariuser3(textBox_NIP.Text) == true)
            {
                MessageBox.Show("N.I.P yang anda masukkan sudah terdaftar di database");
            }
            else
            {

                int tahun_lahir = Convert.ToInt32(dateTimePicker_TglLhr.Value.ToString("yyyy"));
                int tahun_izin = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 17;
                if (textBox_Nama.Text == "" || textBox_Alamat.Text == "" || textBox_NIP.Text == "" || textBox_TmpLhr.Text == "" || textBox_Jabatan.Text == "" || comboBox_PendAkhir.SelectedIndex < 0)
                //if(textBox_Nama.Text == "")
                {
                    MessageBox.Show("Isi semua data dengan benar");
                }
                else if (tahun_lahir >= tahun_izin)
                {
                    MessageBox.Show("Usia Pegawai Minimal 17 Tahun");
                    dateTimePicker_TglLhr.Focus();
                }
                else
                {

                    textBox_Nama.ReadOnly = true;
                    textBox_Alamat.ReadOnly = true;
                    textBox_NIP.ReadOnly = true;
                    textBox_TmpLhr.ReadOnly = true;
                    textBox_Jabatan.ReadOnly = true;
                    comboBox_PendAkhir.Enabled = false;
                    dateTimePicker_TglLhr.Enabled = false;

                    button_Enroll.Enabled = false;
                    button_Batal.Enabled = false;

                    button_AddFace.Enabled = true;

                    Group_Cat.Enabled = true;

                    NIP_Enroll = textBox_NIP.Text;
                    Nama_Enroll = textBox_Nama.Text;
                    grabber = new Capture();
                    grabber.QueryFrame();
                    //Initialize the FrameGraber event

                    Application.Idle += new EventHandler(FrameGrabber);
                }
            }
        }

        private void button_AddFace_Click(object sender, EventArgs e)
        {
            try
            {
                int training_ke = Convert.ToInt32(Label_Training.Text);

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
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                trainingImages2.Add(TrainedFace);
                labels.Add(Nama_Enroll);

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels" + NIP_Enroll + ".txt", trainingImages2.ToArray().Length.ToString() + "%");
                File.WriteAllText(Application.StartupPath + "/TrainedFaces2/TrainedLabelsAll.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages2.ToArray().Length + 1; i++)
                {
                    trainingImages2.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face_" + NIP_Enroll + "" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels" + NIP_Enroll + ".txt", labels.ToArray()[i - 1] + "%");
                }

                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces2/face_1" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces2/TrainedLabelsAll.txt", labels.ToArray()[i - 1] + "%");
                    Label_Training.Text = Convert.ToString(training_ke - 1);

                    if (Label_Training.Text == "4")
                    {
                        Cat_1.Visible = false;
                        Cat_2.Visible = true;
                        Cat_3.Visible = false;
                        Cat_4.Visible = false;
                        Cat_5.Visible = false;
                    }
                    else if (Label_Training.Text == "3")
                    {
                        Cat_1.Visible = false;
                        Cat_2.Visible = false;
                        Cat_3.Visible = true;
                        Cat_4.Visible = false;
                        Cat_5.Visible = false;
                    }
                    else if (Label_Training.Text == "2")
                    {
                        Cat_1.Visible = false;
                        Cat_2.Visible = false;
                        Cat_3.Visible = false;
                        Cat_4.Visible = true;
                        Cat_5.Visible = false;
                    }
                    else if (Label_Training.Text == "1")
                    {
                        Cat_1.Visible = false;
                        Cat_2.Visible = false;
                        Cat_3.Visible = false;
                        Cat_4.Visible = false;
                        Cat_5.Visible = true;
                    }
                    else if (Label_Training.Text == "0")
                    {
                        button_AddFace.Enabled = false;
                        button1.Enabled = true;
                        Group_Cat.Enabled = false;
                    }
                }

                MessageBox.Show(Nama_Enroll + "´s face detected and added", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                DBConnect koneksi_db = new DBConnect();
                MySqlConnection db = new MySqlConnection(koneksi_db.koneksi());
                db.Open();
                MySqlCommand dbcmd = db.CreateCommand();
                string sql = "insert into data_peg values ('" + textBox_NIP.Text + "','" + textBox_Nama.Text + "','" + textBox_Alamat.Text + "','" + textBox_Jabatan.Text + "','" + textBox_TmpLhr.Text + "','" + dateTimePicker_TglLhr.Value.ToString("yyyy-MM-dd") + "','" + comboBox_PendAkhir.SelectedIndex.ToString() + "');";
                dbcmd.CommandText = sql;
                MySqlDataAdapter sqladapter = new MySqlDataAdapter(sql, koneksi_db.koneksi());
                DataSet mydataset = new DataSet();
                sqladapter.Fill(mydataset);
                //id_pembayaran = Convert.ToInt32(dbcmd.ExecuteScalar());
                db.Close();
                MessageBox.Show("Data Pegawai Baru Berhasil Disimpan");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Data Tidak Berhasil Disimpan");
            }
        }

        private void DaftarPeg_Load(object sender, EventArgs e)
        {
            int data_training = Convert.ToInt32(Label_Training.Text);
            if (data_training == 0)
            {
                button_AddFace.Enabled = false;
            }
        }

        private void Label_Training_Click(object sender, EventArgs e)
        {
        }

        private void textBox_NIP_TextChanged(object sender, EventArgs e)
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
