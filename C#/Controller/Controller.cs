using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;
namespace RobotController
{
    public class Controller
    {
        private const byte FORWARD = 17;
        private const byte BACKWARD = 18;
        private const byte LEFT = 20;
        private const byte RIGHT = 24;
        private const byte RAISE_CAMERA = 33;
        private const byte LOWER_CAMERA = 34;
        private const byte POWER_ON_CAMERA = 36;
        private const byte POWER_OFF_CAMERA = 40;

        private const byte STATUS_CHECK = 64;
        private const byte RESPONSE_OK = 65;
        private const byte RESPONSE_ERROR = 66;

        private const byte RESOLUTION_160X120 = 2;
        private const byte RESOLUTION_320X240 = 8;
        private const byte RESOLUTION_640X480 = 32;

        private const byte NO_COMMAND = 0;

        private const int UP = 16;

        private const int DOWN = 32;

        private bool isConnected, isRecording;

        private int steering, movement, lastCameraDirection;

        private HtmlWindow browser;

        private TcpClient carConnection;

        private NetworkStream stream;

        private Timer commandTimer;

        private FileSystemWatcher videoWatcher;

        private String host, port, lastVideoFile, videoDirectory;

        private byte command;

        public Controller()
        {
            host = Config.Default.CarIpAddress;
            port = Config.Default.CarTcpPort;
            if (RobotController.Config.Default.CarIpAddress == "")
            {
                host = "192.168.1.4";    
            }
            if (Config.Default.CarTcpPort == "")
            {
                port = "23";
            }

            this.isConnected = false;
            command = NO_COMMAND;
            isRecording = false;
        }

        public void setBrowser(HtmlWindow browser)
        {
            this.browser = browser;
        }

        public bool connect()
        {
            if (isConnected)
            {
                return true;
            }

            connectToCar();
            
            isConnected = true;
            this.commandTimer = new Timer();
            this.commandTimer.Enabled = true;
            this.commandTimer.Interval = 200;
            this.commandTimer.Tick +=new EventHandler(commandTimer_Tick);
            return carConnection.Connected;
        }

        private bool connectToCar()
        {
            try
            {
                carConnection = new TcpClient(host, Int16.Parse(port));
                stream = carConnection.GetStream();
                return true;
            }
            catch (SocketException e)
            {
                Debug.WriteLine("Error: " + e.Message);
                return false;
            }
        }

        private void connectToCamera()
        {

        }

        public bool disconnect()
        {
            if (!isConnected)
            {
                return false;
            }
            stream.Close();
            carConnection.Close();
            isConnected = false;
            commandTimer.Enabled = false;
            return true;
        }

        public void turnRight()
        {
            steering = RIGHT;
            command |= RIGHT;
        }

        public void turnLeft()
        {
            steering = LEFT;
            command |= LEFT;
        }

        public void moveForward()
        {
            movement = FORWARD;
            command |= FORWARD;
        }

        public void moveBackward()
        {
            movement = BACKWARD;
            command |= BACKWARD;
        }

        public void raiseCamera()
        {
            command |= RAISE_CAMERA;
        }

        public void lowerCamera()
        {
            command |= LOWER_CAMERA;
        }

        public void tiltCameraUp()
        {
            if (browser != null)
            {
                browser.Document.InvokeScript("up_onmousedown");
                lastCameraDirection = UP;
            }
        }

        public void tiltCameraDown()
        {
            if (browser != null)
            {
                browser.Document.InvokeScript("down_onmousedown");
                lastCameraDirection = DOWN;
            }
        }

        public void turnCameraLeft()
        {
            if (browser != null)
            {
                browser.Document.InvokeScript("left_onmousedown");
                lastCameraDirection = LEFT;
            }
        }

        public void turnCameraRight()
        {
            if (browser != null)
            {
                browser.Document.InvokeScript("right_onmousedown");
                lastCameraDirection = RIGHT;
            }
        }

        public void horizontalPatrol()
        {
            if (browser != null)
            {
               browser.Document.InvokeScript("hpatrol_onclick");
            }
        }

        public void verticalPatrol()
        {
            if (browser != null)
            {
                browser.Document.InvokeScript("vpatrol_onclick");
            }
        }

        public void stopHorizontalPatrol()
        {
            if (browser != null)
            {
                browser.Document.InvokeScript("hpatrolstop_onclick");
            }
        }

        public void stopVerticalPatrol()
        {
            if (browser != null)
            {
                browser.Document.InvokeScript("vpatrolstop_onclick");
            }
        }

        public void stopCameraMovement()
        {
            String functionName = "";
            switch (lastCameraDirection)
            {
                case UP:
                    functionName = "up";
                    break;
                case DOWN:
                    functionName = "down";
                    break;
                case LEFT:
                    functionName = "left";
                    break;
                case RIGHT:
                    functionName = "right";
                    break;
                default:
                    functionName = "";
                    break;
            }
            if (functionName != "")
            {
                functionName = functionName + "_onmouseup";
                browser.Document.InvokeScript(functionName);
            }
            lastCameraDirection ^= lastCameraDirection;
        }

        public void snapshot()
        {
            String directory = RobotController.Config.Default.SnapshotDirectory;
            string filepath = directory + System.IO.Path.DirectorySeparatorChar + DateTime.Now.Ticks + ".jpeg";
            System.Net.WebClient webClient = new System.Net.WebClient();
            String ipAddress = RobotController.Config.Default.CameraIpAddress;
            webClient.DownloadFile("http://"+ipAddress+"/snapshot.cgi?user=admin&pwd=", filepath);
        }

        public void startRecording()
        {
            if (isRecording)
            {
                return;
            }
            
            toogleRecording();
            isRecording = true;

            if (null == videoWatcher)
            {
                videoDirectory = getRawVideoDirectory();
                videoWatcher = new FileSystemWatcher();
                videoWatcher.Path = videoDirectory;
                videoWatcher.EnableRaisingEvents = true;
                videoWatcher.Created += new FileSystemEventHandler(lastVideoFile_Watch);
                videoWatcher.Changed += new FileSystemEventHandler(transferVideoFile);
            }
            else
            {
                videoWatcher.EnableRaisingEvents = true;
            }
        }

        public void stopRecording()
        {
            if (isRecording)
            {
                toogleRecording();
                isRecording = false;
            }
        }

        private void toogleRecording()
        {
            HtmlWindow innerFrame = browser.Document.Window.Frames[0];
            object[] args = new object[1];
            args[0] = 4;
            innerFrame.Document.InvokeScript("set_action", args);
        }

        private void lastVideoFile_Watch(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine(e.FullPath);
            Debug.WriteLine(e.ChangeType);
            lastVideoFile = e.Name;
        }

        private void transferVideoFile(object sender, FileSystemEventArgs e)
        {
            Char dirSeparator = System.IO.Path.DirectorySeparatorChar;
            String inputFile = e.FullPath;
            String outputFile = Config.Default.VideoDirectory + dirSeparator + DateTime.Now.Ticks + ".avi";

            System.IO.File.Copy(inputFile, outputFile);
        }

        public void powerOnCamera()
        {
            command |= POWER_ON_CAMERA;
            if (isConnected)
            {
                sendCommands();
            }
        }


        public void powerOffCamera()
        {
            command |= POWER_OFF_CAMERA;
            if (isConnected)
            {
                sendCommands();
            }
        }

        public void increaseBrightness()
        {
            browser.Document.InvokeScript("plus_brightness");
        }

        public void decreaseBrightness()
        {
            browser.Document.InvokeScript("minus_brightness");
        }

        public void increaseContrast()
        {
            browser.Document.InvokeScript("plus_contrast");
        }

        public void decreaseContrast()
        {
            browser.Document.InvokeScript("minus_contrast");
        }

        public void increaseResolution()
        {
            byte currentResolution = getCurrentResolution();
            if (currentResolution == RESOLUTION_160X120)
            {
                setResolution(RESOLUTION_320X240);
            }
            else if (currentResolution == RESOLUTION_320X240)
            {
                setResolution(RESOLUTION_640X480);
            }
        }

        public void decreaseResolution()
        {
            byte currentResolution = getCurrentResolution();
            if (currentResolution == RESOLUTION_640X480)
            {
                setResolution(RESOLUTION_320X240);
            }
            else if (currentResolution == RESOLUTION_320X240)
            {
                setResolution(RESOLUTION_160X120);
            }
        }

        public void setFrameRate(String frameRate)
        {
            object[] args = new object[1];
            args[0] = getFrameRateArg(frameRate);
            browser.Document.InvokeScript("videorate_change", args);
        }

        public string[] getSupportedFrameRates()
        {
            return new string[] {"20", "15", "10", "5", "4", "3", "2", "1"};
        }

        private void setResolution(byte resolution)
        {
            HtmlWindow innerFrame = browser.Document.Window.Frames[0];
            Object[] objectArray = new Object[2];
            objectArray[0] = (Object)"0";

            objectArray[1] = (Object)resolution;
            innerFrame.Document.InvokeScript("set_M", objectArray);
        }

        private byte getFrameRateArg(string frameRate)
        {
            byte[] lookup = { 1, 3, 6, 11, 12, 13, 14, 15};
            byte a;
            string[] supported = getSupportedFrameRates();

            for (a = 0; a < lookup.Length; a++)
            {
                if (frameRate == supported[a])
                {
                    return lookup[a];
                }
            }
            return 0;
        }

        private byte getCurrentResolution()
        {
            String functionName = "getResolution";
            HtmlWindow innerFrame = browser.Document.Window.Frames[0];
            object result = innerFrame.Document.InvokeScript(functionName);
            if (null == result)
            {
                String function = "getResolution = function(){return ipcam[current].Resolution};";
                attachScript(innerFrame.Document.Body, function);
                result = innerFrame.Document.InvokeScript(functionName);
            }
            return Byte.Parse(result.ToString());
        }

        public String getRawVideoDirectory()
        {
            String functionName = "getRecordPath";
            HtmlWindow innerFrame = browser.Document.Window.Frames[0];
            object result = innerFrame.Document.InvokeScript(functionName);
            if (null == result)
            {
                String function = "getRecordPath = function () { return ipcam[current].GetRecordPath()}";
                attachScript(innerFrame.Document.Body, function);
                result = innerFrame.Document.InvokeScript(functionName);
            }
            return result.ToString();
        }

        private void attachScript(HtmlElement parentElement, String scriptText)
        {
            HtmlElement newScript = browser.Document.CreateElement("script");
            mshtml.IHTMLScriptElement script = (mshtml.IHTMLScriptElement)newScript.DomElement;
            script.text = scriptText;
            parentElement.AppendChild(newScript);
        }

        private void commandTimer_Tick(object sender, EventArgs e)
        {
            if (command != NO_COMMAND)
            {
                sendCommands();
            }
            command = NO_COMMAND;
        }

        private bool sendCommands()
        {
            byte[] carCommand, carResponse;
            carCommand = carResponse = new byte[1];

            carCommand[0] = this.command;

            try
            {
                stream.Write(carCommand, 0, 1);
                stream.Flush();
                carConnection.ReceiveTimeout = 250;
                stream.Read(carResponse, 0, 1);
                if (carResponse[0] == RESPONSE_OK)
                {
                    return true;
                }
            }
            catch (System.IO.IOException e)
            {
                Debug.WriteLine("Connection failed");
            }
            return false;
        }

        
    }
}
