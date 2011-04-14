using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using IdSharp.Tagging.ID3v2;

namespace IdSharp.Tagging.Harness.WinForms
{
    public partial class MainForm : Form
    {
        private bool m_IsScanning;
        private bool m_CancelScanning;
        private string m_Filename;

        public MainForm()
        {
            InitializeComponent();

            AssemblyName assemblyName = AssemblyName.GetAssemblyName("IdSharp.Tagging.dll");
            lblVersion.Text = string.Format("IdSharp library version: {0}   PLEASE TEST ON BACKUPS", assemblyName.Version);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnChooseDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (m_IsScanning)
            {
                m_CancelScanning = true;
                return;
            }

            if (Directory.Exists(txtDirectory.Text))
                StartRecursiveScan(txtDirectory.Text);
            else
                MessageBox.Show("Directory does not exist");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (audioOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFile(audioOpenFileDialog.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFile(m_Filename);
        }

        private void btnRemoveID3v2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_Filename))
            {
                MessageBox.Show("No file loaded", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show(string.Format("Remove ID3v2 tag from '{0}'?", Path.GetFileName(m_Filename)), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Boolean success = ID3v2.ID3v2.RemoveTag(m_Filename);
                    if (success)
                        MessageBox.Show("ID3v2 tag successfully removed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("ID3v2 tag not found", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            btnRemoveID3v2.Enabled = ID3v2.ID3v2.DoesTagExist(m_Filename);
            ucID3v2.LoadFile(m_Filename);
        }

        private void btnRemoveID3v1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_Filename))
            {
                MessageBox.Show("No file loaded", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show(string.Format("Remove ID3v1 tag from '{0}'?", Path.GetFileName(m_Filename)), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Boolean success = ID3v1.ID3v1.RemoveTag(m_Filename);
                    if (success)
                        MessageBox.Show("ID3v1 tag successfully removed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("ID3v1 tag not found", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            btnRemoveID3v1.Enabled = ID3v1.ID3v1.DoesTagExist(m_Filename);
            ucID3v1.LoadFile(m_Filename);
        }

        private void StartRecursiveScan(string basePath)
        {
            m_IsScanning = true;
            m_CancelScanning = false;
            lblDirectory.Visible = false;
            txtDirectory.Visible = false;
            btnChooseDirectory.Visible = false;
            btnScan.Text = "&Cancel";
            btnScan.Enabled = false;
            prgScanFiles.Value = 0;
            prgScanFiles.Visible = true;
            ThreadPool.QueueUserWorkItem(ScanDirectory, basePath);
        }

        private void ScanDirectory(object basePathObject)
        {
            int totalFiles = 0;
            BindingList<Track> trackList = new BindingList<Track>();
            try
            {
                string basePath = (string)basePathObject;

                DirectoryInfo di = new DirectoryInfo(basePath);
                FileInfo[] fileList = di.GetFiles("*.mp3", SearchOption.AllDirectories);

                EnableCancelButton();

                totalFiles = fileList.Length;

                for (int i = 0; i < totalFiles; i++)
                {
                    if (m_CancelScanning)
                    {
                        totalFiles = i;
                        break;
                    }

                    IID3v2 id3 = new ID3v2.ID3v2(fileList[i].FullName);

                    trackList.Add(new Track(id3.Artist, id3.Title, id3.Album, id3.Year, id3.Genre, fileList[i].Name));

                    if ((i % 100) == 0)
                    {
                        UpdateProgress(i * 100 / totalFiles);
                    }
                }
            }
            finally
            {
                EndRecursiveScanning(totalFiles, trackList);
            }
        }

        private void EnableCancelButton()
        {
            if (!InvokeRequired)
                btnScan.Enabled = true;
            else
                Invoke(new MethodInvoker(EnableCancelButton));
        }

        private delegate void UpdateProgressDelegate(int progressValue);
        private void UpdateProgress(int progressValue)
        {
            if (!InvokeRequired)
                prgScanFiles.Value = progressValue;
            else
                Invoke(new UpdateProgressDelegate(UpdateProgress), progressValue);
        }

        private delegate void EndRecursiveScanningDelegate(int totalFiles, BindingList<Track> trackList);
        private void EndRecursiveScanning(int totalFiles, BindingList<Track> trackList)
        {
            if (!InvokeRequired)
            {
                m_IsScanning = false;
                prgScanFiles.Visible = false;
                lblDirectory.Visible = true;
                txtDirectory.Visible = true;
                btnChooseDirectory.Visible = true;
                btnScan.Text = "&Scan";
                btnScan.Enabled = true;

                dataGridView1.DataSource = trackList;
            }
            else
            {
                Invoke(new EndRecursiveScanningDelegate(EndRecursiveScanning), totalFiles, trackList);
            }
        }

        private void LoadFile(string path)
        {
            m_Filename = path;

            ucID3v2.LoadFile(m_Filename);
            ucID3v1.LoadFile(m_Filename);

            btnSave.Enabled = true;
            btnRemoveID3v2.Enabled = ID3v2.ID3v2.DoesTagExist(m_Filename);
            btnRemoveID3v1.Enabled = ID3v1.ID3v1.DoesTagExist(m_Filename);
        }

        private void SaveFile(string path)
        {
            ucID3v2.SaveFile(path);
            ucID3v1.SaveFile(path);
            LoadFile(path);
        }
    }
}
