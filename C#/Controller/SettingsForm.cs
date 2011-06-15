using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace RobotController
{
    public partial class SettingsForm : Form
    {
        private Controller robotController;

        public SettingsForm()
        {
            InitializeComponent();
        }
   
        public void setRobotController(Controller controller)
        {
            this.robotController = controller;
        }
        
        private void setVideoRecordPath(object sender, EventArgs e)
        {
            videoDirectoryBrowser.ShowDialog();
            videoDirectory.Text = videoDirectoryBrowser.SelectedPath;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {   
            Config config = new RobotController.Config();
            carIpAddress.Text = config.CarIpAddress;
            carTCPPort.Text = config.CarTcpPort;
            cameraIpAddress.Text = config.CameraIpAddress;
            videoDirectory.Text = config.VideoDirectory;
            snapshotDirectory.Text = config.SnapshotDirectory;
            cameraEnableCheckbox.Checked = config.CameraEnabled;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Config config = new Config();
            
            config.VideoDirectory = videoDirectory.Text;
            config.SnapshotDirectory = snapshotDirectory.Text;
            config.CarIpAddress = carIpAddress.Text;
            config.CameraIpAddress = cameraIpAddress.Text;
            config.CarTcpPort = carTCPPort.Text;
            config.CameraEnabled = cameraEnableCheckbox.Checked;
            config.Save();
            this.Close();
        }

        private void setSnapshotDirectory_Click(object sender, EventArgs e)
        {
            snapshotDirectoryBrowser.ShowDialog();
            snapshotDirectory.Text = snapshotDirectoryBrowser.SelectedPath;
        }

        private void cameraEnableCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                RobotController.Config.Default.CameraEnabled = true;
                cameraSettingsPanel.Enabled = true;
            }
            else
            {
                RobotController.Config.Default.CameraEnabled = false;
                cameraSettingsPanel.Enabled = false;
            }
        }
    }
}