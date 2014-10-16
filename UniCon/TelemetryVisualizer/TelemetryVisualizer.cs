using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using MyCsMath;

namespace UniCon.TelemetryVisualizer
{

    class TelemetryVisualizer
	{
        private readonly string GPAIO = "$GPAIO";
		private readonly string GIQAT = "$GIQAT";

        private readonly string rollFront = "UniCon.TelemetryVisualizer.AttitudeImage.RollFront.png";
        private readonly string rollBack = "UniCon.TelemetryVisualizer.AttitudeImage.RollBack.png";
        private readonly string pitchFront = "UniCon.TelemetryVisualizer.AttitudeImage.PitchFront.png";
        private readonly string pitchBack = "UniCon.TelemetryVisualizer.AttitudeImage.PitchBack.png";
        private readonly string headingFront = "UniCon.TelemetryVisualizer.AttitudeImage.HeadingFront.png";
        private readonly string headingBack = "UniCon.TelemetryVisualizer.AttitudeImage.HeadingBack.png";

        AttitudeImage rollImage;
        AttitudeImage pitchImage;
        AttitudeImage headingImage;

        double roll = 0.0;
        double pitch = 0.0;
        double heading = 0.0;
        
        double latitude;
        double longitude;
        double accuracy;
        WebBrowser browser;

        public TelemetryVisualizer(PictureBox rollPicBox, PictureBox pitchPicBox, PictureBox headingPicBox, WebBrowser browser)
        {
            
            rollImage = new AttitudeImage(rollPicBox, rollFront, rollBack);
            pitchImage = new AttitudeImage(pitchPicBox, pitchFront, pitchBack);
            headingImage = new AttitudeImage(headingPicBox, headingFront, headingBack);
            this.browser = browser;
        }

        public void PaintRoll(Graphics g)
        {
            rollImage.PaintAttitude(g, roll);
        }

        public void PaintPitch(Graphics g)
        {
            pitchImage.PaintAttitude(g, pitch);
        }

        public void PaintHeading(Graphics g)
        {
            headingImage.PaintAttitude(g, heading);
        }

		public void DecodeLine(string line)
		{
			string[] words = line.Split(',');
			if (line.Contains(GPAIO))
            {
                latitude = double.Parse(words[1].Substring(0, 2)) + double.Parse(words[1].Substring(2)) / 60;
                longitude = double.Parse(words[3].Substring(0, 3)) + double.Parse(words[3].Substring(3)) / 60;
                double.TryParse(words[6], out accuracy);
                accuracy *= 5.0;

                double.TryParse(words[7], out pitch);
				double.TryParse(words[8], out roll);
                double.TryParse(words[9], out heading);
            }
			else if (line.Contains(GIQAT))
			{
				Quaternion rawAttitude = new Quaternion(Double.Parse(words[1]), Double.Parse(words[2]), Double.Parse(words[3]), Double.Parse(words[4]));

				pitch = rawAttitude.calcRadPitch() * 180 / Math.PI;
				roll = rawAttitude.calcRadRoll() * 180 / Math.PI;
				heading = rawAttitude.calcRadHeading() * 180 / Math.PI;
			}

		}

        private void UpdatePos()
        {
            object[] args = { latitude.ToString(), longitude.ToString(), accuracy.ToString() };
            browser.Document.InvokeScript("updatePosition", args);
        }
	}
}
