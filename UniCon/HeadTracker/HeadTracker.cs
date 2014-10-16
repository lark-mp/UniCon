using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCsMath;

namespace UniCon.HeadTracker
{
	class HeadTracker
	{
		public double radHeading { get; protected set; }
		public double radPitch { get; protected set; }
		public double radRoll { get; protected set; }

		public HeadTracker()
		{
			radHeading = 0.0;
			radPitch = 0.0;
			radRoll = 0.0;
		}
		public void decodeLine(String line)
		{
			string[] elements = line.Split(',');
			if (elements.Length != 5 || elements[0] != "$GIQAT")
			{
				Console.WriteLine(line);
				Console.WriteLine("not matched");
				return;
			}

			Quaternion rawAttitude = new Quaternion(Double.Parse(elements[1]), Double.Parse(elements[2]), Double.Parse(elements[3]), Double.Parse(elements[4]));
			Quaternion rotation1 = new Quaternion(Math.Cos(Math.PI / 4), 0, Math.Sin(Math.PI / 4), 0);
			Quaternion rotation2 = new Quaternion(Math.Cos(Math.PI / 2), 0, 0, Math.Sin(Math.PI / 2));

			Quaternion attitude = rawAttitude.mul(rotation1.con());
			attitude = attitude.mul(rotation2.con());

			radHeading = attitude.calcRadHeading();
			radRoll = attitude.calcRadRoll();
			radPitch = attitude.calcRadPitch();

			//debug print
			//Console.WriteLine("pitch:"+radPitch*180/Math.PI+", roll:"+radRoll*180/Math.PI+" ,heading:"+radHeading*180/Math.PI);
		}

	}
}
