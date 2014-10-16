using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace UniCon.Joypad
{
	class JoypadController
	{
		const int THROTTLE_TIC = 8;
		const int FLAPS_TIC = 4;

		Joystick joypad = null;
		JoystickState state;
		int flaps;
		int throttle;

		bool[] buttonsPrev;
		static DirectInput directInput = new DirectInput();

		Dictionary<string, TrackBar> tBarList;

		public static List<string> getDeviceNames()
		{
			var devNames = new List<string>();
			IList<DeviceInstance> deviceList = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);
            
			foreach (var device in deviceList)
			{
				devNames.Add(device.ProductName);
			}

			return devNames;
		}

		public JoypadController(int deviceIndex, Dictionary<string, TrackBar> tBarList)
		{
			IList<DeviceInstance> deviceList = directInput.GetDevices(DeviceClass.GameControl,  DeviceEnumerationFlags.AttachedOnly);

			DeviceInstance joyInstance = deviceList[deviceIndex];
			
			joypad = new Joystick(directInput, joyInstance.InstanceGuid);
			//joypad.SetCooperativeLevel(null, CooperativeLevel.Background | CooperativeLevel.NonExclusive);

			joypad.Acquire();

			buttonsPrev = new bool[joypad.Capabilities.ButtonCount];

			flaps = 0;
			throttle = 0;

			this.tBarList = tBarList;
			state = new JoystickState();
		}

		public void UpdateJoyView()
		{
			tBarList["pitch"].Value = 255 - getY();
			tBarList["roll"].Value = getX();
			tBarList["yaw"].Value = 255 - getZ();
			tBarList["flaps"].Value = getRz();
			tBarList["throttle"].Value = getThrottle();
		}

		public void UpdateJoyState()
		{
			joypad.Poll();
			joypad.GetCurrentState(ref state);

			UpdateThrottle();
			UpdateFlap();
			
			for (int i = 0; i < buttonsPrev.Length; i++)
			{
				buttonsPrev[i] = state.Buttons[i];
			}
		}

		private void UpdateThrottle()
		{
			if (buttonsPrev[7 - 1] && state.Buttons[7 - 1])
			{
				throttle -= 255 / (THROTTLE_TIC - 1);
			}
			if (buttonsPrev[8 - 1] && state.Buttons[8 - 1])
			{
				throttle += 255 / (THROTTLE_TIC - 1);
			}
			if (buttonsPrev[2 - 1] && state.Buttons[2 - 1])
			{
				throttle = 255;
			}
			if (buttonsPrev[1 - 1] && state.Buttons[1 - 1])
			{
				throttle = 0;
			}
			if (255 < throttle)
			{
				throttle = 255;
			}
			else if (throttle < 0)
			{
				throttle = 0;
			}
		}

		private void UpdateFlap()
		{
			if (buttonsPrev[3 - 1] && state.Buttons[3 - 1])
			{
				flaps -= 255 / (FLAPS_TIC - 1);
			}
			if (buttonsPrev[4 - 1] && state.Buttons[4 - 1])
			{
				flaps += 255 / (FLAPS_TIC - 1);
			}
			if (255 < flaps)
			{
				flaps = 255;
			}
			else if (flaps < 0)
			{
				flaps = 0;
			}
		}

		public int getY()//0 to 255
		{
			return 255 - state.Y / 256;
		}

        public int getX()// 0 to 255
		{
			return state.X / 256;
		}

        public int getZ()//0 to 255
		{
			return 255 - state.Z / 256;
		}
        public int getRz()//0 to 255
		{
			return state.RotationZ / 256;
		}
        public int getThrottle()//0 to 255
		{
			return throttle;
		}
		
		public bool getButton(int i)
		{
			joypad.GetCurrentState(ref state);
            return state.Buttons[i];
		}

	}
}
