namespace RobotController
{
    partial class SettingsForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cameraIpAddress = new System.Windows.Forms.TextBox();
            this.snapshotDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.browseSnapshotDirectory = new System.Windows.Forms.Button();
            this.browseVideoDirectory = new System.Windows.Forms.Button();
            this.videoDirectory = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.carIpAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.carTCPPort = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.snapshotDirectoryBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.videoDirectoryBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.cameraEnableCheckbox = new System.Windows.Forms.CheckBox();
            this.cameraSettingsPanel = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.cameraSettingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Camera IP address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Car IP address";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cameraSettingsPanel);
            this.groupBox1.Controls.Add(this.cameraEnableCheckbox);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 118);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera Settings";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Snapshots Directory";
            // 
            // cameraIpAddress
            // 
            this.cameraIpAddress.Location = new System.Drawing.Point(132, 4);
            this.cameraIpAddress.Name = "cameraIpAddress";
            this.cameraIpAddress.Size = new System.Drawing.Size(90, 20);
            this.cameraIpAddress.TabIndex = 9;
            // 
            // snapshotDirectory
            // 
            this.snapshotDirectory.BackColor = System.Drawing.SystemColors.Control;
            this.snapshotDirectory.Location = new System.Drawing.Point(132, 55);
            this.snapshotDirectory.Name = "snapshotDirectory";
            this.snapshotDirectory.ReadOnly = true;
            this.snapshotDirectory.Size = new System.Drawing.Size(180, 20);
            this.snapshotDirectory.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Saved Videos Directory";
            // 
            // browseSnapshotDirectory
            // 
            this.browseSnapshotDirectory.Location = new System.Drawing.Point(320, 53);
            this.browseSnapshotDirectory.Name = "browseSnapshotDirectory";
            this.browseSnapshotDirectory.Size = new System.Drawing.Size(50, 23);
            this.browseSnapshotDirectory.TabIndex = 8;
            this.browseSnapshotDirectory.Text = "Browse";
            this.browseSnapshotDirectory.UseVisualStyleBackColor = true;
            this.browseSnapshotDirectory.Click += new System.EventHandler(this.setSnapshotDirectory_Click);
            // 
            // browseVideoDirectory
            // 
            this.browseVideoDirectory.Location = new System.Drawing.Point(320, 27);
            this.browseVideoDirectory.Name = "browseVideoDirectory";
            this.browseVideoDirectory.Size = new System.Drawing.Size(50, 23);
            this.browseVideoDirectory.TabIndex = 6;
            this.browseVideoDirectory.Text = "Browse";
            this.browseVideoDirectory.UseVisualStyleBackColor = true;
            this.browseVideoDirectory.Click += new System.EventHandler(this.setVideoRecordPath);
            // 
            // videoDirectory
            // 
            this.videoDirectory.Location = new System.Drawing.Point(132, 29);
            this.videoDirectory.Name = "videoDirectory";
            this.videoDirectory.ReadOnly = true;
            this.videoDirectory.Size = new System.Drawing.Size(180, 20);
            this.videoDirectory.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.carIpAddress);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.carTCPPort);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(5, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(389, 75);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Car Settings";
            // 
            // carIpAddress
            // 
            this.carIpAddress.Location = new System.Drawing.Point(136, 11);
            this.carIpAddress.Name = "carIpAddress";
            this.carIpAddress.Size = new System.Drawing.Size(90, 20);
            this.carIpAddress.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Car TCP Port";
            // 
            // carTCPPort
            // 
            this.carTCPPort.Location = new System.Drawing.Point(136, 41);
            this.carTCPPort.Name = "carTCPPort";
            this.carTCPPort.Size = new System.Drawing.Size(42, 20);
            this.carTCPPort.TabIndex = 4;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(156, 217);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cameraEnableCheckbox
            // 
            this.cameraEnableCheckbox.AutoSize = true;
            this.cameraEnableCheckbox.Checked = true;
            this.cameraEnableCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cameraEnableCheckbox.Location = new System.Drawing.Point(11, 19);
            this.cameraEnableCheckbox.Name = "cameraEnableCheckbox";
            this.cameraEnableCheckbox.Size = new System.Drawing.Size(59, 17);
            this.cameraEnableCheckbox.TabIndex = 12;
            this.cameraEnableCheckbox.Text = "Enable";
            this.cameraEnableCheckbox.UseVisualStyleBackColor = true;
            this.cameraEnableCheckbox.CheckedChanged += new System.EventHandler(this.cameraEnableCheckbox_CheckedChanged);
            // 
            // cameraSettingsPanel
            // 
            this.cameraSettingsPanel.Controls.Add(this.videoDirectory);
            this.cameraSettingsPanel.Controls.Add(this.browseVideoDirectory);
            this.cameraSettingsPanel.Controls.Add(this.label5);
            this.cameraSettingsPanel.Controls.Add(this.label1);
            this.cameraSettingsPanel.Controls.Add(this.cameraIpAddress);
            this.cameraSettingsPanel.Controls.Add(this.browseSnapshotDirectory);
            this.cameraSettingsPanel.Controls.Add(this.snapshotDirectory);
            this.cameraSettingsPanel.Controls.Add(this.label3);
            this.cameraSettingsPanel.Location = new System.Drawing.Point(6, 35);
            this.cameraSettingsPanel.Name = "cameraSettingsPanel";
            this.cameraSettingsPanel.Size = new System.Drawing.Size(373, 79);
            this.cameraSettingsPanel.TabIndex = 13;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 243);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.cameraSettingsPanel.ResumeLayout(false);
            this.cameraSettingsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox carTCPPort;
        private System.Windows.Forms.TextBox snapshotDirectory;
        private System.Windows.Forms.Button browseSnapshotDirectory;
        private System.Windows.Forms.Button browseVideoDirectory;
        private System.Windows.Forms.TextBox videoDirectory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox cameraIpAddress;
        private System.Windows.Forms.TextBox carIpAddress;
        private System.Windows.Forms.FolderBrowserDialog snapshotDirectoryBrowser;
        private System.Windows.Forms.FolderBrowserDialog videoDirectoryBrowser;
        private System.Windows.Forms.CheckBox cameraEnableCheckbox;
        private System.Windows.Forms.Panel cameraSettingsPanel;
    }
}