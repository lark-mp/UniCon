using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace UniCon.TelemetryVisualizer
{
	class AttitudeImage
	{
        PictureBox picBox;
        Bitmap frontBmp;

        public AttitudeImage(PictureBox picBox, string frontImage, string backImage)
        {
            this.picBox = picBox;

            System.Reflection.Assembly myAssembly =
				System.Reflection.Assembly.GetExecutingAssembly();

			picBox.Image = new Bitmap(
				new Bitmap(myAssembly.GetManifestResourceStream(backImage)),
                picBox.Width, picBox.Height);
            frontBmp =
                new Bitmap(myAssembly.GetManifestResourceStream(frontImage));
        }

        public void PaintAttitude(Graphics g, double angle)
        {
            Rectangle pbRect = new Rectangle(new Point(0, 0), picBox.Size);

            g.TranslateTransform(-picBox.Size.Width / 2, -picBox.Size.Height / 2);
            g.RotateTransform((float)angle, System.Drawing.Drawing2D.MatrixOrder.Append);
            g.TranslateTransform(picBox.Size.Width / 2, picBox.Size.Height / 2,
                System.Drawing.Drawing2D.MatrixOrder.Append);
            g.DrawImage(frontBmp, pbRect);
        }
	}
}
