using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace RobotController
{
    public partial class ControllerForm : Form
    {
        private int FORWARD = 1;

        private int BACKWARD = 2;

        private int LEFT = 4;

        private int RIGHT = 8;

        private Controller robotController;

        private bool isConnected, animateConnectionOnce, isRecording;

        private int carMovement, carDirection, cameraMovement;

        private Timer carCommandRepeater, signalAnimationTimer, recordAnimationTimer, snapshotAnimationTimer;

        private Keys lastCameraDirection;

        private byte connectionAnimationTick;

        public ControllerForm()
        {
            InitializeComponent();
            this.robotController = new Controller();
            carCommandRepeater = new Timer();
            carCommandRepeater.Enabled = false;
            carCommandRepeater.Interval = 250;
            carCommandRepeater.Tick +=new EventHandler(carCommandRepeater_Tick);
            isConnected = false;
            initializeControlPanel();
            animateConnectionOnce = false;
            isRecording = false;
            initializeAnimationTimers();
        }

        private void initializeControlPanel()
        {
            this.cameraTiltUpIcon.MouseDown +=new MouseEventHandler(this.onCameraIconMouseDown);
            this.cameraTiltDownIcon.MouseDown += new MouseEventHandler(this.onCameraIconMouseDown);
            this.cameraPanLeftIcon.MouseDown += new MouseEventHandler(this.onCameraIconMouseDown);
            this.cameraPanRightIcon.MouseDown += new MouseEventHandler(this.onCameraIconMouseDown);
            this.cameraTiltUpIcon.MouseUp += new MouseEventHandler(this.onCameraIconMouseUp);
            this.cameraTiltDownIcon.MouseUp += new MouseEventHandler(this.onCameraIconMouseUp);
            this.cameraPanLeftIcon.MouseUp += new MouseEventHandler(this.onCameraIconMouseUp);
            this.cameraPanRightIcon.MouseUp += new MouseEventHandler(this.onCameraIconMouseUp);
            this.cameraRaiseIcon.MouseDown += new MouseEventHandler(this.onCameraIconMouseDown);
            this.cameraRaiseIcon.MouseUp += new MouseEventHandler(this.onCameraIconMouseUp);
            this.cameraLowerIcon.MouseDown += new MouseEventHandler(this.onCameraIconMouseDown);
            this.cameraLowerIcon.MouseUp += new MouseEventHandler(this.onCameraIconMouseUp);

            this.carBackwardIcon.MouseDown += new MouseEventHandler(this.onCarIconMouseDown);
            this.carForwardIcon.MouseDown += new MouseEventHandler(this.onCarIconMouseDown);
            this.carLeftIcon.MouseDown += new MouseEventHandler(this.onCarIconMouseDown);
            this.carRightIcon.MouseDown += new MouseEventHandler(this.onCarIconMouseDown);
            this.carBackwardIcon.MouseUp += new MouseEventHandler(this.onCarIconMouseUp);
            this.carForwardIcon.MouseUp += new MouseEventHandler(this.onCarIconMouseUp);
            this.carLeftIcon.MouseUp += new MouseEventHandler(this.onCarIconMouseUp);
            this.carRightIcon.MouseUp += new MouseEventHandler(this.onCarIconMouseUp);
            
            this.brightnessIncreaseIcon.Click += new EventHandler(brightnessIncreaseIcon_Click);
            this.brightnessDecreaseIcon.Click += new EventHandler(brightnessDecreaseIcon_Click);
            this.contrastIncreaseIcon.Click += new EventHandler(contrastIncreaseIcon_Click);
            this.contrastDecreaseIcon.Click += new EventHandler(contrastDecreaseIcon_Click);
            this.resolutionIncreaseIcon.Click += new EventHandler(resolutionIncreaseIcon_Click);
            this.resolutionDecreaseIcon.Click += new EventHandler(resolutionDecreaseIcon_Click);

            //this.frameRate.Items.AddRange(robotController.getSupportedFrameRates());
            //this.frameRate.GotFocus += new EventHandler(frameRate_GotFocus);
            this.recordVideo.Click += new EventHandler(recordVideo_Click);
            this.snapshot.Click += new EventHandler(snapshot_Click);
        }

        void frameRate_GotFocus(object sender, EventArgs e)
        {
            controlPanel.Focus();
        }

        private void initializeAnimationTimers()
        {
            connectionAnimationTick = 0;
            signalAnimationTimer = new Timer();
            signalAnimationTimer.Tick += new EventHandler(signalAnimationTimer_Tick);
            signalAnimationTimer.Interval = 300;
            signalAnimationTimer.Enabled = true;
            signalAnimationTimer.Stop();

            recordAnimationTimer = new Timer();
            recordAnimationTimer.Tick += new EventHandler(recordAnimationTimer_Tick);
            recordAnimationTimer.Interval = 500;
            recordAnimationTimer.Enabled = false;

            snapshotAnimationTimer = new Timer();
            snapshotAnimationTimer.Tick += new EventHandler(snapshotAnimationTimer_Tick);
            snapshotAnimationTimer.Interval = 100;
            snapshotAnimationTimer.Enabled = false;
            
        }

        private void connectionIcon_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                bool success = robotController.connect();
                //bool success = true;
                if (success)
                {
                    controlPanel.Focus();
                    isConnected = true;
                    controlPanel.Visible = true;
                    controlPanel.Enabled = true;
                    if (RobotController.Config.Default.CameraEnabled)
                    {
                        connectToCamera();
                    }
                    else
                    {
                        robotController.powerOffCamera();
                    }
                }
                else
                {
                    Debug.WriteLine("Connection failed");
                }
            }
            else
            {
                bool success = robotController.disconnect();
                if (success)
                {
                    controlPanel.Enabled = false;
                    isConnected = false;
                    disconnectCamera();
                    carCommandRepeater.Enabled = false;
                    connectionIcon.Image = RobotController.Properties.Resources.irkick;
                }
            }
        }

        void connectToCamera()
        {
            String host = RobotController.Config.Default.CameraIpAddress;
            webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.activeXModeSelection);
            webBrowser.Navigate("http://192.168.1.8/main.htm");
        }

        void disconnectCamera()
        {
            webBrowser.Navigate("about:blank");
            webBrowser.Visible = false;
        }

        void startConnection(object sender, PaintEventArgs e)
        {
            robotController.connect();
        }

        private void onCarIconMouseDown(object sender, EventArgs e)
        {
            PictureBox icon = (PictureBox)sender;
            bool unknownCommand = false;
            switch (icon.Name)
            {
                case "carForwardIcon":
                    robotController.moveForward();
                    carMovement = FORWARD;
                    break;
                case "carBackwardIcon":
                    robotController.moveBackward();
                    carMovement = BACKWARD;
                    break;
                case "carLeftIcon":
                    robotController.turnLeft();
                    carDirection = LEFT;
                    break;
                case "carRightIcon":
                    robotController.turnRight();
                    carDirection = RIGHT;
                    break;
                default:
                    unknownCommand = true;
                    break;
            }
            if (!unknownCommand)
            {
                animateConnectionIcon();
            }
            carCommandRepeater.Enabled = true;
            //controlPanel.Focus();
        }

        private void onCarIconMouseUp(object sender, EventArgs e)
        {
            carCommandRepeater.Enabled = false;
            stopConnectionAnimation();
        }

        private void onCameraIconMouseDown(object sender, EventArgs e)
        {
            PictureBox icon = (PictureBox)sender;
            bool unknownCommand = false;
            switch(icon.Name)
            {
                case "cameraTiltUpIcon":
                    robotController.tiltCameraUp();
                    break;
                case "cameraTiltDownIcon":
                    robotController.tiltCameraDown();
                    break;
                case "cameraPanLeftIcon":
                    robotController.turnCameraLeft();
                    break;
                case "cameraPanRightIcon":
                    robotController.turnCameraRight();
                    break;
                case "cameraRaiseIcon":
                    robotController.raiseCamera();
                    carCommandRepeater.Enabled = true;
                    break;
                case "cameraLowerIcon":
                    robotController.lowerCamera();
                    carCommandRepeater.Enabled = true;
                    break;
                default:
                    unknownCommand = true;
                    break;
            }
            if (!unknownCommand)
            {
                animateConnectionIcon();
            }
            this.controlPanel.Focus();
        }

        private void onCameraIconMouseUp(object sender, EventArgs e)
        {
            PictureBox icon = (PictureBox)sender;
            if (icon.Name != "cameraLowerIcon" && icon.Name != "cameraRaiseIcon")
            {
                robotController.stopCameraMovement();
            }
            else
            {
                carCommandRepeater.Enabled = false;
            }
            stopConnectionAnimation();
        }

        private void keyIsPreview(object sender, PreviewKeyDownEventArgs e)
        {
            this.Focus();
        }

        private void keyIsPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            Keys key = (Keys)e.KeyCode;
            controlCameraByKey(key);
            controlCarByKey(key);
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (!isConnected)
            {
                return;
            }
            Keys key = (Keys)e.KeyCode;
            bool unknownCommand = false;
            switch (key)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    carCommandRepeater.Enabled = false;    
                    break;
                case Keys.W:
                case Keys.S:
                case Keys.A:
                case Keys.D:
                    robotController.stopCameraMovement();
                    lastCameraDirection ^= lastCameraDirection;
                    break;
                case Keys.Z:
                case Keys.X:
                    break;
                default:
                    unknownCommand = true;
                    break;
            }
            if (!unknownCommand)
            {
                stopConnectionAnimation();
            }
        }

        private void controlCameraByKey(Keys key)
        {
            //for the tilt and turn, refrain from invoking in quick succession the same camera command since the
            //browser implementation apparently stops the camera everytime a command is issued
            if (lastCameraDirection == key)
            {
                return;
            }
            bool unknownCommand = false;
            switch(key)
            {
                case Keys.W:
                    robotController.tiltCameraUp();
                    lastCameraDirection = Keys.W;
                    break;
                case Keys.S:
                    robotController.tiltCameraDown();
                    lastCameraDirection = Keys.S;
                    break;
                case Keys.A:
                    robotController.turnCameraLeft();
                    lastCameraDirection = Keys.A;
                    break;
                case Keys.D:
                    robotController.turnCameraRight();
                    lastCameraDirection = Keys.D;
                    break;
                case Keys.Z:
                    robotController.lowerCamera();
                    break;
                case Keys.X:
                    robotController.raiseCamera();
                    break;
                default:
                    unknownCommand = true;
                    break;
            }
            if (!unknownCommand)
            {
                animateConnectionIcon();
            }
        }

        private void controlCarByKey(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    robotController.moveForward();
                    break;
                case Keys.Down:
                    robotController.moveBackward();
                    break;
                case Keys.Left:
                    robotController.turnLeft();
                    break;
                case Keys.Right:
                    robotController.turnRight();
                    break;
                default:
                    break;
            }
        }

        private void carCommandRepeater_Tick(object sender, EventArgs e)
        {
            if (carDirection == LEFT)
            {
                robotController.turnLeft();
            }
            else if (carDirection == RIGHT)
            {
                robotController.turnRight();
            }

            if (carMovement == FORWARD)
            {
                robotController.moveForward();
            }
            else if (carMovement == BACKWARD)
            {
                robotController.moveBackward();
            }
        }

        private void cameraConnected(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser.DocumentCompleted -= this.cameraConnected;
            displayCameraViewOnly((WebBrowser)sender);

            HtmlWindow frame = webBrowser.Document.Window.Frames[0];
            robotController.setBrowser(frame);
            webBrowser.Visible = true;
            controlPanel.Visible = true;
            controlPanel.Focus();
        }

        /**
        * Strip the HTML UI to the bare camera view by hiding the top and side control panels
        */
        private void displayCameraViewOnly(WebBrowser browser)
        {
            HtmlWindow outerFrame = browser.Document.Window.Frames[0];
            HtmlElement containerTable = outerFrame.Document.Body.Children[7];
            HtmlWindow innerFrame = outerFrame.Document.Window.Frames[0];
            HtmlElement frameTable = containerTable.FirstChild.Children[2].FirstChild.FirstChild;

            outerFrame.Document.Body.AppendChild(frameTable);
            frameTable.Style = "width: 100%";
            containerTable.Style = "display: none;";
            innerFrame.Size = new System.Drawing.Size(645, 485);
            
            frameTable.FirstChild.FirstChild.FirstChild.Style = "display:none;";
            innerFrame.Document.Body.FirstChild.FirstChild.Children[1].FirstChild.Style = "border-style:none;";
            animateConnectionIconOnce();
        }
        
        /**
         * Automatically select the Internet Explorer/ActiveX option from the list by clicking on that link
         */
        private void activeXModeSelection(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlWindow iframe;
            if (webBrowser.Url.PathAndQuery == "/main.htm")
            {
                webBrowser.DocumentCompleted -= this.activeXModeSelection;
                iframe = webBrowser.Document.Window.Frames[0];
                webBrowser.DocumentCompleted += this.cameraConnected;
                iframe.Document.InvokeScript("monitor");
            }
            else
            {
                connectToCamera();
            }
        }

        private void settingsIcon_Click(object sender, EventArgs e)
        {
            SettingsForm form = new SettingsForm();
            bool isCameraEnabledPreviously = RobotController.Config.Default.CameraEnabled;
            form.ShowDialog();
            bool isCameraEnabledNow = RobotController.Config.Default.CameraEnabled;
            if (isCameraEnabledPreviously == true && isCameraEnabledNow == false)
            {
                disconnectCamera();
                robotController.powerOffCamera();
                cameraPanel.Enabled = false;
            }
            else if (isCameraEnabledPreviously == false && isCameraEnabledNow == true)
            {
                robotController.powerOnCamera();
                System.Threading.Thread.Sleep(10000);
                connectToCamera();
                cameraPanel.Enabled = true;
            }
        }

        private void recordVideo_Click(object sender, EventArgs e)
        {
            
            if (!isRecording)
            {
                isRecording = true;
                recordVideo.BackgroundImage = RobotController.Properties.Resources.videoOn;
                robotController.startRecording();
                recordAnimationTimer.Enabled = true;
            }
            else
            {
                robotController.stopRecording();
                recordVideo.BackgroundImage = RobotController.Properties.Resources.video;
                isRecording = false;
                recordAnimationTimer.Enabled = false;
            }
        }

        private void snapshot_Click(object sender, EventArgs e)
        {
            robotController.snapshot();
            snapshotAnimationTimer.Enabled = true;
        }

        private void brightnessIncreaseIcon_Click(object sender, EventArgs e)
        {
            robotController.increaseBrightness();
        }

        private void brightnessDecreaseIcon_Click(object sender, EventArgs e)
        {
            robotController.decreaseBrightness();
        }

        private void contrastIncreaseIcon_Click(object sender, EventArgs e)
        {
            robotController.increaseContrast();
        }

        private void contrastDecreaseIcon_Click(object sender, EventArgs e)
        {
            robotController.decreaseContrast();
        }

        private void resolutionIncreaseIcon_Click(object sender, EventArgs e)
        {
            robotController.increaseResolution();
        }

        private void resolutionDecreaseIcon_Click(object sender, EventArgs e)
        {
            robotController.decreaseResolution();
        }

        private void frameRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox list = (ComboBox)sender;
            robotController.setFrameRate((String)list.SelectedItem);
        }

        private void animateConnectionIcon()
        {
            signalAnimationTimer.Start();
            animateConnectionOnce = false;
        }

        private void animateConnectionIconOnce()
        {
            signalAnimationTimer.Start();
            animateConnectionOnce = true;
        }

        private void stopConnectionAnimation()
        {
            if (connectionAnimationTick != 2)
            {
                animateConnectionOnce = true;
            }
            else
            {
                connectionIcon.Image = RobotController.Properties.Resources.irkickflash;
                signalAnimationTimer.Stop();
            }
        }

        private void signalAnimationTimer_Tick(object sender, EventArgs e)
        {
            if (connectionAnimationTick == 0)
            {
                connectionIcon.Image = RobotController.Properties.Resources.irkickflash0;
                connectionAnimationTick++;   
            }
            else if (connectionAnimationTick == 1)
            {
                connectionIcon.Image = RobotController.Properties.Resources.irkickflash1;
                connectionAnimationTick++;
            }
            else if (connectionAnimationTick == 2)
            {
                connectionIcon.Image = RobotController.Properties.Resources.irkickflash;
                connectionAnimationTick = 0;
                if (animateConnectionOnce)
                {
                    signalAnimationTimer.Stop();
                    animateConnectionOnce = false;
                }
            }
        }

        void recordAnimationTimer_Tick(object sender, EventArgs e)
        {
            if ((string)recordVideo.Tag == "RecordOff")
            {
                recordVideo.BackgroundImage = RobotController.Properties.Resources.videoOn;
                recordVideo.Tag = "RecordOn";
            }
            else
            {
                recordVideo.BackgroundImage = RobotController.Properties.Resources.video;
                recordVideo.Tag = "RecordOff";
            }
        }


        void snapshotAnimationTimer_Tick(object sender, EventArgs e)
        {
            if ((string)snapshot.Tag == "SnapshotOff")
            {
                snapshot.BackgroundImage = RobotController.Properties.Resources.snapshotOn;
                snapshot.Tag = "SnapshotOn";
            }
            else
            {
                snapshot.BackgroundImage = RobotController.Properties.Resources.snapshot;
                snapshot.Tag = "SnapshotOff";
                snapshotAnimationTimer.Enabled = false;
            }
        }
    }
}
