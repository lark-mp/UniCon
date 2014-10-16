using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace UniCon.CV
{
	class CvLib
	{

		CvMat eagleEyeMapx = null;
		CvMat eagleEyeMapy = null;
		IplImage reticleImage = null;


		double degPitch, degroll, degHedading;
		int cmd;

		static double w = 640, h = 480;

		public CvLib()
		{
			//Thread thread = new Thread(new ThreadStart(this.show));
			//thread.IsBackground = true;
			//thread.Start();
		}

		public void setCommand(int cmd)
		{
			this.cmd = cmd;
		}
		public double[] getTargetOffset()
		{
			double[] offset = new double[2];
			offset[0] = targetX - w / 2;
			offset[1] = targetY - h / 2;

			return offset;
		}


		public void show()
		{
			try
			{
				Bitmap screenBitmap = new Bitmap(1280, 480);
				Graphics screenGraphic = Graphics.FromImage(screenBitmap);
				//using (var capture = Cv.CreateCameraCapture(CaptureDevice.VFW))
				//using (var capture = Cv.CreateCameraCapture(CaptureDevice.Any))

				IplImage frame;

				Cv.NamedWindow("Capture", WindowMode.AutoSize);


				while (true)
				{
					int c = Cv.WaitKey(2);
					if (c == '\x1b')
						break;

					screenGraphic.CopyFromScreen(new Point(0, 0), new Point(0, 0), screenBitmap.Size);
					IplImage capturedImg = (IplImage)BitmapConverter.ToIplImage(screenBitmap);

					//Cv.ShowImage("Capture", );
					//Cv.ShowImage("Capture", frame);
					//Cv.ShowImage("edit", drawReticle(eagleEye2(frame)));
					//Cv.ShowImage("edit", edgeTrack(frame,c));
					//Cv.ShowImage("edit", corrTrack(frame, c));
					//Cv.ShowImage("edit", drawReticle(edgeTrack2(rotate(frame, degroll), c)));
					//Cv.ShowImage("edit", drawReticle(edgeTrack2(rotate(frame, degroll),0)));
					//Cv.ShowImage("edit", drawReticle(edgeTrack(corrTrack(rotate(frame, degroll), c), c)));
					//Cv.ShowImage("edit", edgeTrack2(corrTrack(rotate(frame,degroll), c),c));
					//Cv.ShowImage("edit", edgeTrack2(rotate(frame,degroll),c));
					//Cv.ShowImage("edit", areaCorrTrack(frame,c));
					//if (prevImage != null)



					Cv.ShowImage("Capture", capturedImg);

					capturedImg.Dispose();

					if (prevImage != null)
					{
						//Cv.ShowImage("area", prevImage);
					}

					//break;
				}

				Cv.DestroyWindow("Capture");
				Cv.DestroyWindow("edit");

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
		public IplImage rotate(IplImage src, double angle)
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
		public IplImage canny(IplImage src)
		{
			IplImage dst = new IplImage(src.Size, BitDepth.U8, 1);
			src.Canny(dst, 50, 200);



			return dst;
		}

		public IplImage zoom(IplImage src)
		{
			IplImage dst = new IplImage(src.Size, BitDepth.U8, 1);

			Bitmap srcBitmap = BitmapConverter.ToBitmap(src);
			Bitmap dstBitmap = new Bitmap(srcBitmap);

			for (int y = 0; y < srcBitmap.Height; y++)
			{
				for (int x = 0; x < srcBitmap.Width; x++)
				{
					dstBitmap.SetPixel(x, y, srcBitmap.GetPixel(x / 4 + 3 * srcBitmap.Width / 8, y / 4 + 3 * srcBitmap.Height / 8));
				}
			}

			dst = BitmapConverter.ToIplImage(dstBitmap);

			return dst;
		}




		TrackState state = TrackState.stop;
		enum TrackState
		{
			stop,
			run,
		}

		int targetX = (int)w / 2;
		int targetY = (int)h / 2;
		const int searchArea = 256;
		const int searchSkip = 1;
		const int caretSize = 64;
		public IplImage edgeTrackOld(IplImage src, int cmd)
		{

			IplImage dst = Cv.CreateImage(src.ROI.Size, src.Depth, src.NChannels);
			Cv.Copy(src, dst);

			//Cv.CvtColor(src, dst, ColorConversion.RgbToGray);

			Bitmap bitmap = BitmapConverter.ToBitmap(dst);

			int maxEvaluation = int.MinValue;
			int maxEvalX = 0;
			int maxEvalY = 0;

			if (cmd == 's')
			{
				state = TrackState.run;
			}
			else if (cmd == 'r')
			{
				state = TrackState.stop;
				targetX = src.Width / 2;
				targetY = src.Height / 2;
			}

			if (state == TrackState.run)
			{
				for (int x = targetX - searchArea / 2; x < targetX + searchArea / 2; x += searchSkip)
				{
					for (int y = targetY - searchArea / 2; y < targetY + searchArea / 2; y += searchSkip)
					{
						int currentEvaluation = evaluate(bitmap, x, y);
						if (currentEvaluation > maxEvaluation)
						{
							maxEvaluation = currentEvaluation;
							maxEvalX = x;
							maxEvalY = y;
						}
					}
				}
				targetX = maxEvalX;
				targetY = maxEvalY;
			}



			int caretTop = limit(targetY - caretSize / 2, src.Height);
			int caretBottom = limit(targetY + caretSize / 2, src.Height);
			int caretLeft = limit(targetX - caretSize / 2, src.Width);
			int caretRight = limit(targetX + caretSize / 2, src.Width);



			Cv.Rectangle(dst, Cv.Point(caretLeft, caretTop), Cv.Point(caretRight, caretBottom), 255);

			return dst;
		}


		public int evaluate(Bitmap bitmap, int offsetX, int offsetY)
		{
			int sum = 0;




			for (int x = offsetX - caretSize / 2; x < offsetX + caretSize / 2; x++)
			{
				if (x < 0 || x >= bitmap.Width)
				{
					return int.MinValue;
				}
				for (int y = offsetY - caretSize / 2; y < offsetY + caretSize / 2; y++)
				{

					if (y < 0 || y >= bitmap.Height)
					{
						return int.MinValue;
					}
					int weight;
					if (x >= offsetX && y >= offsetY)
					{
						weight = -3;
					}
					else
					{
						weight = 1;
					}

					sum += weight * (int)(bitmap.GetPixel(x, y).GetBrightness() * 255);

				}
			}
			return sum;
		}

		public IplImage edgeTrack2(IplImage src, int cmd)
		{
			try
			{
				IplImage monochro = Cv.CreateImage(src.ROI.Size, BitDepth.U8, 1);


				Cv.CvtColor(src, monochro, ColorConversion.RgbToGray);

				//Cv.SetImageROI(monochro, new CvRect(src.Width / 4, src.Height / 4, src.Width / 2, src.Height / 2));

				CvMat integral = new CvMat(monochro.ROI.Height + 1, monochro.ROI.Width + 1, MatrixType.F32C1);

				//IplImage evalMap = Cv.CreateImage(monochro.ROI.Size, src.Depth, src.NChannels);


				Cv.Integral(monochro, integral);

				if (cmd == 's')
				{
					state = TrackState.run;
				}
				else if (cmd == 'r' || targetX < 0 || targetY < 0)
				{
					state = TrackState.stop;
					targetX = monochro.ROI.Width / 2;
					targetY = monochro.ROI.Height / 2;
				}

				double maxEvaluation = double.MinValue;
				int maxEvalX = 0;
				int maxEvalY = 0;

				//Bitmap evalBitmap = evalMap.ToBitmap();

				if (state == TrackState.run)
				{
					for (int x = targetX - searchArea / 2; x < targetX + searchArea / 2; x += searchSkip)
					{
						for (int y = targetY - searchArea / 2; y < targetY + searchArea / 2; y += searchSkip)
						{
							if (x >= caretSize / 2 && y >= caretSize / 2 && x < monochro.ROI.Width - caretSize / 2 && y < monochro.ROI.Height - caretSize / 2)
							{
								double currentEvaluation = evaluate2(integral, x, y);
								if (currentEvaluation > maxEvaluation)
								{
									maxEvaluation = currentEvaluation;
									maxEvalX = x;
									maxEvalY = y;
								}

								//int intEval = (int)((currentEvaluation/32/32/2)+128);
								//evalBitmap.SetPixel(x, y, Color.FromArgb(intEval,intEval,intEval));
							}
						}
					}
					targetX = maxEvalX;
					targetY = maxEvalY;
				}


				targetX = limit(targetX, caretSize / 2, monochro.ROI.Width - caretSize);
				targetY = limit(targetY, caretSize / 2, monochro.ROI.Height - caretSize);

				int caretTop = targetY - caretSize / 2;
				int caretBottom = targetY + caretSize / 2;
				int caretLeft = targetX - caretSize / 2;
				int caretRight = targetX + caretSize / 2;

				Cv.Rectangle(monochro, Cv.Point(caretLeft, caretTop), Cv.Point(caretRight, caretBottom), 255);



				return monochro;
				//return BitmapConverter.ToIplImage(evalBitmap);


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return src;

		}

		private double evaluate2(CvMat integral, int x, int y)
		{
			double whiteAreaVal = getIntegralSum(integral, y - caretSize / 2, y + caretSize / 2, x - caretSize / 2, x + caretSize / 2);
			double blackAreaVal = getIntegralSum(integral, y, y + caretSize / 2, x, x + caretSize / 2);

			return whiteAreaVal - 4 * blackAreaVal;
		}

		private double getIntegralSum(CvMat integral, int top, int bottom, int left, int right)
		{
			double upLeftVal = Cv.mGet(integral, top, left);
			double downLeftVal = Cv.mGet(integral, bottom, left);
			double upRightVal = Cv.mGet(integral, top, right);
			double downRightVal = Cv.mGet(integral, bottom, right + 1);

			return (downRightVal - downLeftVal - upRightVal + upLeftVal);
		}

		private int limit(int value, int ceil)
		{
			if (value < 0)
			{
				value = 0;
			}
			else if (value >= ceil)
			{
				value = ceil - 1;
			}
			return value;
		}
		private int limit(int value, int floor, int ceil)
		{
			if (value < floor)
			{
				value = floor;
			}
			else if (value > ceil)
			{
				value = ceil;
			}
			return value;
		}

		IplImage prevImage = null;
		double lopassCoef = 0.95;
		public IplImage corrTrack(IplImage src, int cmd)
		{
			if (prevImage == null)
			{
				prevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
				Cv.Resize(src, prevImage);
				Cv.SetImageROI(prevImage, new CvRect(src.Width / 4, src.Height / 4, src.Width / 2, src.Height / 2));
			}

			//CvSize matchSize = new CvSize(src.Width-prevImage.Width+1,src.Height-prevImage.Height+1);
			CvSize matchSize = new CvSize(src.Width - prevImage.ROI.Width + 1, src.Height - prevImage.ROI.Height + 1);

			IplImage matchImage = Cv.CreateImage(matchSize, BitDepth.F32, 1);


			Cv.MatchTemplate(src, prevImage, matchImage, MatchTemplateMethod.CCorrNormed);

			CvPoint minLoc = new CvPoint(0, 0);
			CvPoint maxLoc = new CvPoint(0, 0);

			Cv.MinMaxLoc(matchImage, out minLoc, out maxLoc);

			//Cv.Rectangle(dst, maxLoc,new CvPoint(maxLoc.X+prevImage.Width/2,maxLoc.Y+prevImage.Height/2), 3);


			IplImage dst = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
			CvPoint2D32f center;
			center.X = (float)(lopassCoef * (maxLoc.X + prevImage.ROI.Width / 2 - 0.5) + (1 - lopassCoef) * src.Width / 2);
			center.Y = (float)(lopassCoef * (maxLoc.Y + prevImage.ROI.Height / 2 - 0.5) + (1 - lopassCoef) * src.Height / 2);
			Cv.GetRectSubPix(src, dst, center);



			prevImage.Dispose();
			prevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
			Cv.Resize(dst, prevImage);
			Cv.SetImageROI(prevImage, new CvRect(src.Width / 4, src.Height / 4, src.Width / 2, src.Height / 2));

			//Cv.SetImageROI(dst,new CvRect(src.Width/4,src.Height/4,src.Width/2,src.Height/2));

			matchImage.Dispose();

			return dst;
		}

		private IplImage areaCorrTrack(IplImage src, int cmd)
		{
			IplImage dst = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
			Cv.Copy(src, dst);


			if (cmd == 's')
			{
				state = TrackState.run;

				if (prevImage != null)
				{
					prevImage.Dispose();
				}
				prevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
				Cv.Copy(src, prevImage);
				Cv.SetImageROI(prevImage, new CvRect(targetX - caretSize / 2, targetY - caretSize / 2, caretSize, caretSize));
			}
			else if (cmd == 'r')
			{
				state = TrackState.stop;
				targetX = src.Width / 2;
				targetY = src.Height / 2;
			}

			if (state == TrackState.run)
			{
				CvSize matchSize = new CvSize(src.Width - prevImage.ROI.Width + 1, src.Height - prevImage.ROI.Height + 1);
				IplImage matchImage = Cv.CreateImage(matchSize, BitDepth.F32, 1);


				Cv.MatchTemplate(src, prevImage, matchImage, MatchTemplateMethod.CCorrNormed);

				CvPoint minLoc = new CvPoint(0, 0);
				CvPoint maxLoc = new CvPoint(0, 0);

				Cv.MinMaxLoc(matchImage, out minLoc, out maxLoc);

				targetX = maxLoc.X + caretSize / 2;
				targetY = maxLoc.Y + caretSize / 2;

				prevImage.Dispose();
				prevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
				Cv.Copy(src, prevImage);
				Cv.SetImageROI(prevImage, new CvRect(targetX - caretSize / 2, targetY - caretSize / 2, caretSize, caretSize));


				matchImage.Dispose();
			}
			else
			{
				if (prevImage != null)
				{
					prevImage.Dispose();
				}
				prevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
				Cv.Copy(src, prevImage);
				Cv.SetImageROI(prevImage, new CvRect(targetX - caretSize / 2, targetY - caretSize / 2, caretSize, caretSize));

				CvSize matchSize = new CvSize(src.Width - prevImage.ROI.Width + 1, src.Height - prevImage.ROI.Height + 1);
				IplImage matchImage = Cv.CreateImage(matchSize, BitDepth.F32, 1);


				Cv.MatchTemplate(src, prevImage, matchImage, MatchTemplateMethod.CCorrNormed);

				CvPoint minLoc = new CvPoint(0, 0);
				CvPoint maxLoc = new CvPoint(0, 0);

				double minValue, maxValue;


				Cv.MinMaxLoc(matchImage, out minValue, out maxValue, out minLoc, out maxLoc);

				//CvScalar avg = Cv.Avg(matchImage);
				//Console.Write(maxValue+",\t"+avg.Val0+"\n\r");

				IplImage binarized = Cv.CreateImage(matchImage.Size, matchImage.Depth, matchImage.NChannels);

				Cv.Threshold(matchImage, binarized, 0.97, 1.0, ThresholdType.Binary);
				int nonZero = Cv.CountNonZero(binarized);


				if (nonZero > src.Width * src.Height * 0.001)
				{
					Cv.Line(dst, Cv.Point(targetX - caretSize / 2, targetY - caretSize / 2), Cv.Point(targetX + caretSize / 2, targetY + caretSize / 2), new CvScalar(0, 255, 0, 0));
					Cv.Line(dst, Cv.Point(targetX + caretSize / 2, targetY - caretSize / 2), Cv.Point(targetX - caretSize / 2, targetY + caretSize / 2), new CvScalar(0, 255, 0, 0));
				}

				matchImage.Dispose();
				binarized.Dispose();
			}

			targetX = limit(targetX, caretSize / 2, dst.ROI.Width - caretSize);
			targetY = limit(targetY, caretSize / 2, dst.ROI.Height - caretSize);

			int caretTop = targetY - caretSize / 2;
			int caretBottom = targetY + caretSize / 2;
			int caretLeft = targetX - caretSize / 2;
			int caretRight = targetX + caretSize / 2;

			Cv.Rectangle(dst, Cv.Point(caretLeft, caretTop), Cv.Point(caretRight, caretBottom), new CvScalar(0, 255, 0, 0));

			return dst;
		}

		private IplImage areaCorrTrack2(IplImage src, int cmd)
		{
			IplImage dst = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
			Cv.Copy(src, dst);


			if (cmd == 's')
			{
				state = TrackState.run;

				if (prevImage != null)
				{
					prevImage.Dispose();
				}
				prevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
				Cv.Copy(src, prevImage);
				Cv.SetImageROI(prevImage, new CvRect(targetX - caretSize / 2, targetY - caretSize / 2, caretSize, caretSize));
			}
			else if (cmd == 'r')
			{
				state = TrackState.stop;
				targetX = src.Width / 2;
				targetY = src.Height / 2;
			}

			if (state == TrackState.run)
			{
				CvSize matchSize = new CvSize(src.Width - prevImage.ROI.Width + 1, src.Height - prevImage.ROI.Height + 1);
				IplImage matchImage = Cv.CreateImage(matchSize, BitDepth.F32, 1);


				Cv.MatchTemplate(src, prevImage, matchImage, MatchTemplateMethod.CCorrNormed);

				double minValue, maxValue;
				CvPoint minLoc = new CvPoint(0, 0);
				CvPoint maxLoc = new CvPoint(0, 0);

				Cv.MinMaxLoc(matchImage, out minValue, out maxValue, out minLoc, out maxLoc);

				targetX = maxLoc.X + caretSize / 2;
				targetY = maxLoc.Y + caretSize / 2;


				IplImage tmpPrevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
				Cv.Copy(src, tmpPrevImage);
				Cv.SetImageROI(tmpPrevImage, new CvRect(targetX - caretSize / 2, targetY - caretSize / 2, caretSize, caretSize));

				if (maxValue < 0.98)
				{
					Cv.Line(dst, Cv.Point(targetX - caretSize / 2, targetY - caretSize / 2), Cv.Point(targetX + caretSize / 2, targetY + caretSize / 2), new CvScalar(0, 255, 0, 0));
					Cv.Line(dst, Cv.Point(targetX + caretSize / 2, targetY - caretSize / 2), Cv.Point(targetX - caretSize / 2, targetY + caretSize / 2), new CvScalar(0, 255, 0, 0));
				}


				IplImage matchImage2 = Cv.CreateImage(matchSize, BitDepth.F32, 1);


				Cv.MatchTemplate(src, tmpPrevImage, matchImage2, MatchTemplateMethod.CCorrNormed);

				IplImage binarized = Cv.CreateImage(matchImage.Size, matchImage.Depth, matchImage.NChannels);

				Cv.Threshold(matchImage2, binarized, 0.97, 1.0, ThresholdType.Binary);
				int nonZero = Cv.CountNonZero(binarized);

				if (nonZero < src.Width * src.Height * 0.001)
				{
					prevImage.Dispose();
					prevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
					Cv.Copy(src, prevImage);
					Cv.SetImageROI(prevImage, new CvRect(targetX - caretSize / 2, targetY - caretSize / 2, caretSize, caretSize));
					Console.WriteLine("1");
				}
				else
				{
					Console.WriteLine("0");
				}

				//return binarized;

				matchImage.Dispose();
			}
			else
			{
				if (prevImage != null)
				{
					prevImage.Dispose();
				}
				prevImage = Cv.CreateImage(src.Size, src.Depth, src.NChannels);
				Cv.Copy(src, prevImage);
				Cv.SetImageROI(prevImage, new CvRect(targetX - caretSize / 2, targetY - caretSize / 2, caretSize, caretSize));

				CvSize matchSize = new CvSize(src.Width - prevImage.ROI.Width + 1, src.Height - prevImage.ROI.Height + 1);
				IplImage matchImage = Cv.CreateImage(matchSize, BitDepth.F32, 1);


				Cv.MatchTemplate(src, prevImage, matchImage, MatchTemplateMethod.CCorrNormed);

				CvPoint minLoc = new CvPoint(0, 0);
				CvPoint maxLoc = new CvPoint(0, 0);

				double minValue, maxValue;


				Cv.MinMaxLoc(matchImage, out minValue, out maxValue, out minLoc, out maxLoc);

				//CvScalar avg = Cv.Avg(matchImage);
				//Console.Write(maxValue+",\t"+avg.Val0+"\n\r");

				IplImage binarized = Cv.CreateImage(matchImage.Size, matchImage.Depth, matchImage.NChannels);

				Cv.Threshold(matchImage, binarized, 0.97, 1.0, ThresholdType.Binary);
				int nonZero = Cv.CountNonZero(binarized);


				if (nonZero > src.Width * src.Height * 0.001)
				{
					Cv.Line(dst, Cv.Point(targetX - caretSize / 2, targetY - caretSize / 2), Cv.Point(targetX + caretSize / 2, targetY + caretSize / 2), new CvScalar(0, 255, 0, 0));
					Cv.Line(dst, Cv.Point(targetX + caretSize / 2, targetY - caretSize / 2), Cv.Point(targetX - caretSize / 2, targetY + caretSize / 2), new CvScalar(0, 255, 0, 0));
				}

				matchImage.Dispose();
				binarized.Dispose();
			}

			targetX = limit(targetX, caretSize / 2, dst.ROI.Width - caretSize);
			targetY = limit(targetY, caretSize / 2, dst.ROI.Height - caretSize);

			int caretTop = targetY - caretSize / 2;
			int caretBottom = targetY + caretSize / 2;
			int caretLeft = targetX - caretSize / 2;
			int caretRight = targetX + caretSize / 2;

			Cv.Rectangle(dst, Cv.Point(caretLeft, caretTop), Cv.Point(caretRight, caretBottom), new CvScalar(0, 255, 0, 0));

			return dst;
		}

		public IplImage eagleEye(IplImage src)
		{
			const bool DEBUG_GRID = false;

			IplImage dst = Cv.CreateImage(src.Size, src.Depth, src.NChannels);

			if (eagleEyeMapx == null || eagleEyeMapy == null)
			{
				initEagleEyeMap(src);
			}

			Cv.Remap(src, dst, eagleEyeMapx, eagleEyeMapy, Interpolation.Cubic);


			return dst;
		}

		private void initEagleEyeMap(IplImage src)
		{
			const double zoomRate = 4.0;
			const double pixcelZoomRadius = 150;
			const double pixcelTransientRadius = 300;

			int width = src.Width;
			int height = src.Height;

			eagleEyeMapx = Cv.CreateMat(height, width, MatrixType.F32C1);
			eagleEyeMapy = Cv.CreateMat(height, width, MatrixType.F32C1);


			for (int y = -height / 2; y < height / 2; y++)
			{
				for (int x = -width / 2; x < width / 2; x++)
				{
					double srcX = 0;
					double srcY = 0;

					double direction;
					double length;
					double multiplyer;

					length = Math.Sqrt(x * x + y * y);
					direction = Math.Atan2(y, x);


					if (length < pixcelZoomRadius)
					{
						srcX = (int)(x / zoomRate);
						srcY = (int)(y / zoomRate);
					}
					else if (length < pixcelTransientRadius)
					{
						multiplyer = (1 + 1 / zoomRate) / 2 - (1 - 1 / zoomRate) / 2 * Math.Cos(Math.PI * (length - pixcelZoomRadius) / (pixcelTransientRadius - pixcelZoomRadius));

						srcX = (int)(multiplyer * x);
						srcY = (int)(multiplyer * y);
					}
					else
					{
						srcX = (int)x;
						srcY = (int)y;
						//continue;
					}

					if (-width / 2 <= srcX && srcX < width / 2 && -height / 2 <= srcY && srcY < height / 2)
					{
						eagleEyeMapx[y + height / 2, x + width / 2] = srcX + width / 2;
						eagleEyeMapy[y + height / 2, x + width / 2] = srcY + height / 2;
					}
				}
			}
		}
		public IplImage drawReticle(IplImage src)
		{
			IplImage dst = Cv.CreateImage(src.ROI.Size, src.Depth, src.NChannels);

			if (reticleImage == null)
			{
				initReticle(src);
			}
			Cv.AddWeighted(src, 1.0, reticleImage, 0.5, 0.0, dst);

			return dst;
		}
		private void initReticle(IplImage src)
		{
			int width = src.ROI.Width;
			int height = src.ROI.Height;

			Bitmap reticle = new Bitmap(width, height);

			for (int x = 0; x < width; x++)
			{
				reticle.SetPixel(x, height / 2, Color.Green);
				if (x < width * 3 / 8 || x >= width * 5 / 8)
				{
					reticle.SetPixel(x, height / 2 - 1, Color.Green);
					reticle.SetPixel(x, height / 2 + 1, Color.Green);
				}
			}

			for (int y = 0; y < height; y++)
			{
				reticle.SetPixel(width / 2, y, Color.LightGreen);
				if (y < height * 3 / 8 || y >= height * 5 / 8)
				{
					reticle.SetPixel(width / 2 + 1, y, Color.LightGreen);
					reticle.SetPixel(width / 2 - 1, y, Color.LightGreen);
				}
			}


			reticleImage = Cv.CreateImage(src.ROI.Size, src.Depth, src.NChannels);
			BitmapConverter.ToIplImage(reticle, reticleImage);

		}

		public IplImage eagleEye2(IplImage src)
		{
			const bool DEBUG_GRID = false;
			const bool DRAW_RETICLE = true;

			const double zoomRate = 4.0;
			const double pixcelZoomRadius = 150;
			const double pixcelTransientRadius = 400;

			IplImage dst = new IplImage(src.Size, BitDepth.U8, 1);

			Bitmap srcBitmap = BitmapConverter.ToBitmap(src);
			Bitmap dstBitmap = new Bitmap(srcBitmap);




			int width = srcBitmap.Width;
			int height = srcBitmap.Height;


			if (DEBUG_GRID)
			{
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						if (x % 16 == 0 || y % 16 == 0)
						{
							srcBitmap.SetPixel(x, y, Color.Black);
						}
						else
						{
							srcBitmap.SetPixel(x, y, Color.White);
						}
					}
				}
			}


			for (int y = -height / 2; y < height / 2; y++)
			{
				for (int x = -width / 2; x < width / 2; x++)
				{
					int srcX = 0;
					int srcY = 0;

					double direction;
					double length;
					double multiplyer;

					length = Math.Sqrt(x * x + y * y);
					direction = Math.Atan2(y, x);

					if (length < pixcelZoomRadius)
					{
						srcX = (int)(x / zoomRate);
						srcY = (int)(y / zoomRate);
					}
					else if (length < pixcelTransientRadius)
					{
						multiplyer = (1 + 1 / zoomRate) / 2 - (1 - 1 / zoomRate) / 2 * Math.Cos(Math.PI * (length - pixcelZoomRadius) / (pixcelTransientRadius - pixcelZoomRadius));

						srcX = (int)(multiplyer * x);
						srcY = (int)(multiplyer * y);
					}
					else
					{
						continue;
					}

					if (-width / 2 <= srcX && srcX < width / 2 && -height / 2 <= srcY && srcY < height / 2)
					{
						dstBitmap.SetPixel(x + width / 2, y + height / 2, srcBitmap.GetPixel(srcX + width / 2, srcY + height / 2));
					}
				}
			}

			dst = BitmapConverter.ToIplImage(dstBitmap);

			if (DRAW_RETICLE)
			{
				Bitmap reticle = new Bitmap(width, height);

				for (int x = 0; x < width; x++)
				{
					reticle.SetPixel(x, height / 2, Color.Green);
					if (x < width * 3 / 8 || x >= width * 5 / 8)
					{
						reticle.SetPixel(x, height / 2 - 1, Color.Green);
						reticle.SetPixel(x, height / 2 + 1, Color.Green);
					}
				}

				for (int y = 0; y < height; y++)
				{
					reticle.SetPixel(width / 2, y, Color.LightGreen);
					if (y < height * 3 / 8 || y >= height * 5 / 8)
					{
						reticle.SetPixel(width / 2 + 1, y, Color.LightGreen);
						reticle.SetPixel(width / 2 - 1, y, Color.LightGreen);
					}
				}

				IplImage reticleImage = BitmapConverter.ToIplImage(reticle);

				Cv.AddWeighted(dst, 1.0, reticleImage, 0.5, 0.0, dst);
			}

			return dst;
		}

		public void decodeLine(String line)
		{
			string[] lines = line.Split(',');
			if (lines.Length == 3)
			{
				try
				{
					degroll = double.Parse(lines[2]);
				}
				catch (Exception) { }
			}

		}

	}
}
