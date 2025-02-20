using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompProg5_group_relaxation
{
    public partial class formMedia : Form
    {
        public formMedia()
        {
            InitializeComponent();

        }

        private Dictionary<string, string> musicList = new Dictionary<string, string>();
        private Dictionary<string, string> videoList = new Dictionary<string, string>();

        private int selectedPhotoIndex = 0;
        private IEnumerable<string> photosList;

        private bool isMusic;

        

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (var music in musicList)
            {
                listBox1.Items.Add(music.Key);
            }

            isMusic = true;

            ShowPictureBox();
            SelectPhoto();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            foreach (var video in videoList)
            {
                listBox2.Items.Add(video.Key);
            }

            isMusic = false;

            ShowWMP();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mediaTypeList = isMusic ? musicList : videoList;
            var selectedMedia = mediaTypeList[listBox1.SelectedItem.ToString()];

            axWindowsMediaPlayer1.URL = selectedMedia;
            if (isMusic)
            {
                selectedPhotoIndex = 0;
                SelectPhoto();
                StartStopTimer();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
            StartStopTimer();
        }

        private void StartStopTimer()
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();
            }
        }

        private void ShowPictureBox()
        {
            pictureBox1.Visible = true;
            pictureBox1.BringToFront();
        }

        private void ShowWMP()
        {
            axWindowsMediaPlayer1.BringToFront();
            axWindowsMediaPlayer1.Visible = true;

            pictureBox1.Visible = false;
        }

        

        private Dictionary<string, string> GetAssetList(string folderPath)
        {



            var assetList = new Dictionary<string, string>();
            

            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath);
                
                foreach (var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    assetList.Add(fileName, file);
                }
            }
            else
            {
                MessageBox.Show("No media folder found");
            }

            return assetList;
        }



        private void SelectPhoto()
        {
            var photoPath = photosList.ElementAt(selectedPhotoIndex);
            pictureBox1.Load(photoPath);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();

            if (isMusic)
                timer1.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isMusic)
                timer1.Stop();

            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                axWindowsMediaPlayer1.Ctlcontrols.pause();

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mediaTypeList = isMusic ? musicList : videoList;
            var selectedMedia = mediaTypeList[listBox2.SelectedItem.ToString()];

            axWindowsMediaPlayer1.URL = selectedMedia;
            if (isMusic)
            {
                selectedPhotoIndex = 0;
                SelectPhoto();
                StartStopTimer();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            selectedPhotoIndex = selectedPhotoIndex >= photosList.Count() - 1 ? 0 : selectedPhotoIndex + 1;

            SelectPhoto();
        }

        private void formMedia_Load(object sender, EventArgs e)
        {
            var musicFolderPath = Path.Combine(Application.StartupPath, "Assets", "Media", "Music");
            musicList = GetAssetList(musicFolderPath);

            var videoFolderPath = Path.Combine(Application.StartupPath, "Assets", "Media", "Videos");
            videoList = GetAssetList(videoFolderPath);
            try
            {
                var photosFolderPath = Path.Combine(Application.StartupPath, "Assets", "Media", "Photos");
                photosList = Directory.GetFiles(photosFolderPath);
            }
            catch (Exception)
            {
                MessageBox.Show("No photos found in the folder");
            }
        }
    }
}
