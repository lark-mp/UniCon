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

        private readonly string[] CONTROL_PHASE = {
            "LaunchStandby",
            "BoostPhase",
            "GlidePhase",
            "ManualControl",
            "Emergency",
            "PitchHold",
        };



        AttitudeImage rollImage;
        AttitudeImage pitchImage;
        AttitudeImage headingImage;

        public double degRoll { get; set; }
        public double degPitch {get; set; }
        public double degHeading {get; set;}
        public double degCannonHeadingRelativeToBody {get;set;}
        
        double latitude;
        double longitude;
        double accuracy;
        int gpsValid;
        int nextWaypointId;

        WebBrowser browser;
        UniConForm form;

        public TelemetryVisualizer(PictureBox rollPicBox, PictureBox pitchPicBox, PictureBox headingPicBox, WebBrowser browser, UniConForm form)
        {
            latitude = 35.707256;
            longitude = 139.760640;
            rollImage = new AttitudeImage(rollPicBox, rollFront, rollBack);
            pitchImage = new AttitudeImage(pitchPicBox, pitchFront, pitchBack);
            headingImage = new AttitudeImage(headingPicBox, headingFront, headingBack);
            this.browser = browser;
            this.form = form;
        }

        public void PaintRoll(Graphics g)
        {
            rollImage.PaintAttitude(g, degRoll);
        }

        public void PaintPitch(Graphics g)
        {
            pitchImage.PaintAttitude(g, degPitch);
        }

        public void PaintHeading(Graphics g)
        {
            headingImage.PaintAttitude(g, degHeading);
        }

		public void DecodeLine(string line)
		{
			string[] words = line.Split(',');
			if (line.Contains(GPAIO))
            {
                if(words.Length != 17){
                    Console.WriteLine("invalid line:");
                    Console.WriteLine(line);
                    return;
                }
                //$GPAIO,Latitude,N/S,Longitude,E/W,height,HDOP,pitch,roll,yaw,SpeedX,SpeedY,SpeedZ,GpsValid,nextWaypoint,checksum
                double tmpDegPitch;
                double tmpDegRoll;
                double tmpDegHeading;
                double height;

                latitude = double.Parse(words[1].Substring(0, 2)) + double.Parse(words[1].Substring(2)) / 60;
                longitude = double.Parse(words[3].Substring(0, 3)) + double.Parse(words[3].Substring(3)) / 60;
                double.TryParse(words[5], out height);
                double.TryParse(words[6], out accuracy);
                accuracy *= 5.0;

                double.TryParse(words[7], out tmpDegPitch);
				double.TryParse(words[8], out tmpDegRoll);
                double.TryParse(words[9], out tmpDegHeading);

                degPitch = tmpDegPitch;
                degRoll = tmpDegRoll;
                degHeading = tmpDegHeading;

                double tmpMpsXSpeed;
                double tmpMpsYSpeed;
                double tmpMpsZSpeed;

                double.TryParse(words[10], out tmpMpsXSpeed);
                double.TryParse(words[11], out tmpMpsYSpeed);
                double.TryParse(words[12], out tmpMpsZSpeed);

                int tmpGpsValid;
                Int32.TryParse(words[13],out tmpGpsValid);
                gpsValid = tmpGpsValid;

                int tmpNextWaypointId;
                Int32.TryParse(words[14], out tmpNextWaypointId);
                nextWaypointId = tmpNextWaypointId;
                UpdatePos();

                Int32 controlPhaseId;
                Int32.TryParse(words[15], out controlPhaseId);
                string controlPhaseText = CONTROL_PHASE[controlPhaseId];

                form.setGpsStatusLabel(latitude,longitude, 1, height);
                form.setSpeedStatusLabel(tmpMpsXSpeed,tmpMpsYSpeed,tmpMpsZSpeed);
                form.setAttitudeStatusLabel(0,degRoll,degPitch,degHeading);
                form.setControlPhaseLabel("Phase:"+controlPhaseText);
            }
			else if (line.Contains(GIQAT))
			{
				Quaternion rawAttitude = new Quaternion(Double.Parse(words[1]), Double.Parse(words[2]), Double.Parse(words[3]), Double.Parse(words[4]));

				degPitch = rawAttitude.calcRadPitch() * 180 / Math.PI;
				degRoll = rawAttitude.calcRadRoll() * 180 / Math.PI;
				degHeading = rawAttitude.calcRadHeading() * 180 / Math.PI;

                UpdatePos();
            }
            else if (words[0] == "$GITNK")
            {
                degCannonHeadingRelativeToBody = double.Parse(words[1]);
            }
            else if(words[0] == "$WPAFM"){
                //$WPAFM,WaypointCount,Lattitude0,Longitude0,Lattitude1,Longitude1,...
                if (browser != null)
                {
                    browser.Invoke(new deleteAllAffirmedWaypointsDelegate(delegateDeleteAllAffirmedWaypoints), null);


                    int waypointCount;
                    int.TryParse(words[1], out waypointCount);
                    if (words.Length == 2 + (waypointCount * 2))
                    {
                        for (int i = 0; i < waypointCount; i++)
                        {
                            double lattitude, longitude;
                            double.TryParse(words[i * 2 + 2], out lattitude);
                            double.TryParse(words[i * 2 + 3], out longitude);

                            browser.Invoke(new setAffirmedWaypointDelegate(delegateSetAffirmedWaypoint), lattitude, longitude);

                        }
                    }

                }
            }

		}

        private void UpdatePos()
        {
            if (browser != null)
            {
                browser.Invoke(new UpdatePosDelegate(delegateUpdatePos), null);
            }
        }

        private delegate void UpdatePosDelegate();
        private void delegateUpdatePos()
        {
            object[] args = { latitude.ToString(), longitude.ToString(), accuracy.ToString(), degHeading.ToString() ,gpsValid.ToString(),nextWaypointId.ToString()};
            this.browser.Document.InvokeScript("updatePosition", args);
        }

        private delegate void setAffirmedWaypointDelegate(double lattitude,double longitude);
        private void delegateSetAffirmedWaypoint(double lattitude,double longitude)
        {
            object[] args = { lattitude,longitude };
            this.browser.Document.InvokeScript("addAffirmedWaypoint", args);
        }

        private delegate void deleteAllAffirmedWaypointsDelegate();
        private void delegateDeleteAllAffirmedWaypoints()
        {
            object[] args = { };
            this.browser.Document.InvokeScript("deleteAllAffirmedWaypoints",args);
        }
	}
}
