using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniCon.Joypad;
using UniCon.HeadTracker;
using UniCon.CV;

namespace UniCon.Interpreter
{
	class Interpreter
	{
        private JoypadController joypadController;
        private HeadTracker.HeadTracker headTracker;
        private CvOculus cvOculus;

        private CheckBox cam640Button;
        private CheckBox CamFullHDButton;
        private CheckBox camOffButton;
        private CheckBox camStabilizeCheckBox;

        bool button2Prev;
        double degHeadTrackerZeroPoint = 0.0;

        public Interpreter(HeadTracker.HeadTracker headTracker, CvOculus cvOculus, CheckBox cam640Button, CheckBox CamFullHDButton, CheckBox camOffButton, CheckBox camStabilizeCheckBox)
        {
            // TODO: Complete member initialization
            this.headTracker = headTracker;
            this.cvOculus = cvOculus;
            this.cam640Button = cam640Button;
            this.CamFullHDButton = CamFullHDButton;
            this.camOffButton = camOffButton;
            this.camStabilizeCheckBox = camStabilizeCheckBox;
        }

        internal string DecodeCon()
        {
            int pitch = 255 - joypadController.getY();
            int roll = joypadController.getX();
            int yaw = 255 - joypadController.getZ();
            int throttle = joypadController.getThrottle();
            int rz = joypadController.getRz();


            byte cameraStart = 0;
            if (cam640Button.Checked)
            {
                cameraStart |= 0x01;
            }
            else if (CamFullHDButton.Checked)
            {
                cameraStart |= 0x03;
            }
            else if (camOffButton.Checked)
            {
                cameraStart |= 0x0F;
            }
            cam640Button.Checked = false;
            CamFullHDButton.Checked = false;
            camOffButton.Checked = false;


            if (!button2Prev && joypadController.getButton(2))
            {
                camStabilizeCheckBox.Checked = (camStabilizeCheckBox.Checked == false); //invert
            }
            button2Prev = joypadController.getButton(2);
            if (camStabilizeCheckBox.Checked)
            {
                cameraStart |= 0xC0;
            }

            if (joypadController.getButton(4))
            {
                cameraStart |= 0x20;
            }
            if (joypadController.getButton(5))
            {
                cameraStart |= 0x10;
            }

            byte mode = 0;
            byte reserved = 0;



            double dCameraH = (headTracker.radHeading * 180 / Math.PI - degHeadTrackerZeroPoint);
            if (dCameraH < -180)
            {
                dCameraH += 360;
            }
            else if (dCameraH > 180)
            {
                dCameraH -= 360;
            }


            double dCameraV = headTracker.radPitch * 180 / Math.PI;

            short sCameraH = (short)(dCameraH * 10);
            short sCameraV = (short)(dCameraV * 10);

            byte cameraH_H = (byte)((sCameraH >> 8) & 0xFF);
            byte cameraH_L = (byte)(sCameraH & 0xFF);
            byte cameraV_H = (byte)((sCameraV >> 8) & 0xFF);
            byte cameraV_L = (byte)(sCameraV & 0xFF);


            return "$USCTL," + mode.ToString("x2") + "," + pitch.ToString("x2") + ","
                + roll.ToString("x2") + "," + yaw.ToString("x2") + "," + rz.ToString("x2") + ","
                + throttle.ToString("x2") + ","
                + cameraH_H.ToString("x2") + "," + cameraH_L.ToString("x2") + ","
                + cameraV_H.ToString("x2") + "," + cameraV_L.ToString("x2") + ","
                + reserved.ToString("x2") + ","
                + cameraStart.ToString("x2") + "\n";

        }

        internal void SetHeadTrackerZero()
        {
            degHeadTrackerZeroPoint = headTracker.radHeading * 180 / Math.PI;
        }

        internal void SetJoypad(JoypadController joypadController)
        {
            this.joypadController = joypadController;
        }
    }
}
