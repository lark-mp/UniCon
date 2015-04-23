using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace UniCon.CV
{
	class CvOculus
	{
		static readonly private Size SRC_SIZE = new Size(640, 480);
		static readonly private CvSize DST_SIZE = new CvSize(1280, 800); //resolution of oclus rift. both eye.
		static readonly private Point SRC_OFFSET = new Point(0, 31);
		static private System.Collections.ObjectModel.ReadOnlyCollection<double> DISTORTION_K
			= System.Array.AsReadOnly(new double[] { 1, 0.8, 1.0, 0 });
		static readonly double DECENTERING_X = 0.09;  //-1 to 1
		static readonly int SHIFT_Y = 35;
		static readonly CvColor DEFAULT_COLOR = CvColor.Green;
		static double angle_test = 0;

		private bool cvContinue;
		CvMat oculusWarpMapX = null;
		CvMat oculusWarpMapY = null;
		IplImage overlayStaticContent;
		IplImage overlayDynamicContent;

		public double degTankPitch { get; set; }
		public double degTankRoll { get; set; }
		public double degTankHeading { get; set; }
		public bool cameraStabilize { get; set; }

		CvLib cvLib;

		public CvOculus()
		{
			degTankPitch = degTankRoll = degTankHeading = 0;
			cvLib = new CvLib();

			Thread thread = new Thread(new ThreadStart(this.cvThread));
			thread.IsBackground = true;
			thread.Start();
		}

		private void cvThread()
		{
			cvContinue = true;

			Bitmap srcBitmap = new Bitmap(SRC_SIZE.Width, SRC_SIZE.Height);
			Graphics srcGraphics = Graphics.FromImage(srcBitmap);

			while (cvContinue)
			{
				int c = Cv.WaitKey(1);

				srcGraphics.CopyFromScreen(SRC_OFFSET, new Point(0, 0), SRC_SIZE);
				IplImage srcImage = (IplImage)BitmapConverter.ToIplImage(srcBitmap);

				//////////////////////////////////////////
				//             add effect here.         //
				//////////////////////////////////////////

				if (cameraStabilize)
				{
					srcImage = rotate(srcImage, -degTankRoll);

				}

				//IplImage tmpImage = cvLib.corrTrack(srcImage, 0);

				IplImage overlayedImage = overlay(srcImage);

				IplImage dstImage = oculusWarp(overlayedImage);

				//////////////////////////////////////////
				//            set effected image        //
				//////////////////////////////////////////
				Cv.ShowImage("debug", overlayedImage);
				Cv.ShowImage("rift", dstImage);

				srcImage.Dispose();
				dstImage.Dispose();
				//tmpImage.Dispose();
			}

			Cv.DestroyWindow("rift");
		}

		public void kill()
		{
			cvContinue = false;
		}

		///////////////////////////////////////////
		// definition of effects
		///////////////////////////////////////////
		private IplImage oculusWarp(IplImage src)
		{
			GC.Collect();
			const bool DEBUG_GRID = false;

			IplImage dst = Cv.CreateImage(DST_SIZE, src.Depth, src.NChannels);

			if (oculusWarpMapX == null || oculusWarpMapY == null)
			{
				initOculusWarpMap(src, dst);
			}

			Cv.Remap(src, dst, oculusWarpMapX, oculusWarpMapY, Interpolation.Cubic);


			return dst;
		}

		private IplImage overlay(IplImage src)
		{
			IplImage tmp = Cv.CreateImage(src.ROI.Size, src.Depth, src.NChannels);
			IplImage dst = Cv.CreateImage(src.ROI.Size, src.Depth, src.NChannels);

			if (overlayStaticContent == null)
			{
				initOverlayStaticContent(src);
			}
			if (overlayDynamicContent == null)
			{
				initOverlayDynamicContent(src);
			}
			generateOverlayDynamicContent(src);

			Cv.AddWeighted(src, 1.0, overlayStaticContent, 2.0, 0.0, tmp);
			Cv.AddWeighted(tmp, 1.0, overlayDynamicContent, 2.0, 0.0, dst);

			return dst;
		}

		private IplImage rotate(IplImage src, double angle)
		{
			try
			{
				IplImage dst = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
				Cv.Copy(src, dst);


				CvPoint2D32f center = new CvPoint2D32f(src.ROI.Width / 2.0, src.ROI.Height / 2.0);
				CvMat mat = Cv.GetRotationMatrix2D(center, angle, 1.0);


				Cv.WarpAffine(src, dst, mat);

				return dst;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return null;
		}





		///////////////////////////////////////////
		// helper functions for effects
		///////////////////////////////////////////

		/////////////////////
		//oculus warp
		/////////////////////
		private void initOculusWarpMap(IplImage src, IplImage dst)
		{
			oculusWarpMapX = Cv.CreateMat(DST_SIZE.Height, DST_SIZE.Width, MatrixType.F32C1);
			oculusWarpMapY = Cv.CreateMat(DST_SIZE.Height, DST_SIZE.Width, MatrixType.F32C1);


			for (int i = 0; i < DST_SIZE.Height; i++)
			{
				for (int j = 0; j < DST_SIZE.Width; j++)
				{
					int shiftedY = i + SHIFT_Y;
					if (j < DST_SIZE.Width / 2)
					{
						int decenteredX = (int)(j - dst.Width * DECENTERING_X / 2);
						double rPow2 = calcDistanceFromCenterPow2(decenteredX, shiftedY, dst.Size);
						double p = (DISTORTION_K[0] + rPow2 * DISTORTION_K[1] + Math.Pow(rPow2, 2) * DISTORTION_K[2] + Math.Pow(rPow2, 2) * DISTORTION_K[3]);

						oculusWarpMapX[i, j] = (decenteredX - dst.Width / 4) * p + src.Width / 2;
						oculusWarpMapY[i, j] = (shiftedY - dst.Height / 2) * p + src.Height / 2;
					}
					else
					{
						int decenteredX = (int)(j + dst.Width * DECENTERING_X / 2 - dst.Width / 2);
						double rPow2 = calcDistanceFromCenterPow2(decenteredX, shiftedY, dst.Size);
						double p = (DISTORTION_K[0] + rPow2 * DISTORTION_K[1] + Math.Pow(rPow2, 2) * DISTORTION_K[2] + Math.Pow(rPow2, 2) * DISTORTION_K[3]);

						oculusWarpMapX[i, j] = (decenteredX - dst.Width / 4) * p + src.Width / 2;
						oculusWarpMapY[i, j] = (shiftedY - dst.Height / 2) * p + src.Height / 2;
					}
				}
			}
		}

		private double calcDistanceFromCenterPow2(int x, int y, CvSize dstSize)
		{
			return Math.Pow(((double)x) / (dstSize.Width / 2) - 0.5, 2) + Math.Pow(((double)y) / dstSize.Height - 0.5, 2);
		}

		/////////////////////
		//overlay
		/////////////////////
		private void initOverlayStaticContent(IplImage src)
		{
			overlayStaticContent = Cv.CreateImage(src.ROI.Size, src.Depth, src.NChannels);
			/*
			for (int i = 1; i < 4; i++)
			{
				Cv.Line(overlayStaticContent, Cv.Point(i*src.Width / 4, 0), Cv.Point(i*src.Width / 4, src.Height), DEFAULT_COLOR);
				Cv.Line(overlayStaticContent, Cv.Point(0, i*src.Height / 4), Cv.Point(src.Width, i*src.Height / 4), DEFAULT_COLOR);
			}
			*/
			DrawReticle(overlayStaticContent);
		}

		private void DrawReticle(IplImage src)
		{
			string str = "90TK";
			List<FontFace> font_face = new List<FontFace>((FontFace[])Enum.GetValues(typeof(FontFace)));
			CvFont[] font = new CvFont[font_face.Count * 2];
			double fontsize = 0.5;
			for (int i = 0; i < font.Length; i += 2)
			{
				font[i] = new CvFont(font_face[i / 2], fontsize, fontsize);
				font[i + 1] = new CvFont(font_face[i / 2] | FontFace.Italic, fontsize, fontsize);
			}

			Cv.Circle(src, Cv.Point(src.Width / 2, src.Height / 2), 8, DEFAULT_COLOR, 2);

			Cv.Circle(src, Cv.Point(src.Width / 2, src.Height * 2 / 8), 4, DEFAULT_COLOR, 1);
			Cv.Circle(src, Cv.Point(src.Width / 2, src.Height * 3 / 8), 4, DEFAULT_COLOR, 1);
			Cv.Circle(src, Cv.Point(src.Width / 2, src.Height * 5 / 8), 4, DEFAULT_COLOR, 1);
			Cv.Circle(src, Cv.Point(src.Width / 2, src.Height * 6 / 8), 4, DEFAULT_COLOR, 1);
			Cv.Circle(src, Cv.Point(src.Width * 2 / 8, src.Height / 2), 4, DEFAULT_COLOR, 1);
			Cv.Circle(src, Cv.Point(src.Width * 3 / 8, src.Height / 2), 4, DEFAULT_COLOR, 1);
			Cv.Circle(src, Cv.Point(src.Width * 5 / 8, src.Height / 2), 4, DEFAULT_COLOR, 1);
			Cv.Circle(src, Cv.Point(src.Width * 6 / 8, src.Height / 2), 4, DEFAULT_COLOR, 1);

			Cv.PutText(src, str, Cv.Point(src.Width * 3 / 4, src.Height * 3 / 4), font[3], DEFAULT_COLOR);


		}


		private void initOverlayDynamicContent(IplImage src)
		{
			overlayDynamicContent = Cv.CreateImage(src.ROI.Size, src.Depth, src.NChannels);
		}
		private void generateOverlayDynamicContent(IplImage src)
		{
			angle_test = (angle_test + 0.1) % 360;
			DrawAzimuth(src, angle_test);
			DrawAngleTape(src, angle_test);
		}
        private void DrawAzimuth(IplImage src, double angle = 0)
        {
            double vr = 30;
            CvPoint[] vpoint = new CvPoint[3];
            double drawposition = 11.0f / 16;
            angle = -angle / 180 * Math.PI;
            vpoint[0] = Cv.Point((int)(vr * Math.Cos(angle + Math.PI / 2) + src.Width * drawposition), (int)(-vr * Math.Sin(angle + Math.PI / 2) + src.Height * drawposition));
            vpoint[1] = Cv.Point((int)(vr * Math.Cos(angle - Math.PI * 9 / 16) + src.Width * drawposition), (int)(-vr * Math.Sin(angle - Math.PI * 9 / 16) + src.Height * drawposition));
            vpoint[2] = Cv.Point((int)(vr * Math.Cos(angle - Math.PI * 7 / 16) + src.Width * drawposition), (int)(-vr * Math.Sin(angle - Math.PI * 7 / 16) + src.Height * drawposition));
            Cv.Circle(src, Cv.Point((int)(src.Width * drawposition), (int)(src.Height * drawposition)), 30, DEFAULT_COLOR, 1);
            Cv.FillConvexPoly(src, vpoint, DEFAULT_COLOR);

        }


        private void DrawAngleTape(IplImage src, double degAngle = 0)
        {
            double anglelabel;
            double anglescale = 3000.0f / src.Width;

            for (int i = 0; i < 18; i++)
            {
                anglelabel = i * 20 + degAngle;
                if (anglelabel > 180)
                {
                    anglelabel -= 360;
                }
                if (anglelabel > -80 && anglelabel < 80)
                {

                    Cv.Line(src, Cv.Point((int)(-anglescale * anglelabel + src.Width / 2), src.Height * 4 / 32), Cv.Point((int)(-anglescale * anglelabel + src.Width / 2), src.Height * 5 / 32), DEFAULT_COLOR);

                    int labelString = -i * 20;

                    if (labelString < -180)
                    {
                        labelString += 360;
                    }

                    string CurrentAngle = (labelString).ToString();
                    int labelXShift = 7 - CurrentAngle.Length * 8 + src.Width / 2;
                    Cv.PutText(src, CurrentAngle, Cv.Point((int)(-anglescale * anglelabel + labelXShift), src.Height * 6 / 32 + 2), new CvFont(FontFace.HersheyDuplex, 0.5, 0.5), DEFAULT_COLOR);
                }

            }

        }
	}
}
