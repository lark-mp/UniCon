using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCon.Interfaces;
using System.IO.Ports;

namespace UniCon.CommunicationControl.Communicator
{
    class COMCommunicator : ICommunicator
	{
        private SerialPort port;
		private string rawresult;

        public COMCommunicator(ref SerialPort port, string portName)
        {
            this.port = port;
            port.PortName = portName;
            port.Open();

			port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.portDataReceived);
		}

		private void portDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			string line = "";
			// ポートのバッファにあるだけ読む
			rawresult += port.ReadExisting();
			// rawresult に1行分なければ何もしないで終わる
			if (!rawresult.Contains("\n"))
			{
				return;
			}
			while (rawresult.Contains("\n"))
			{
				// line に1行分取り出す
				line = rawresult.Split('\n')[0];
				// 1行分を rawresult から取り除く
				rawresult = rawresult.Substring(rawresult.Split('\n')[0].Length + 1);

                ReceiveLineEventArgs le = new ReceiveLineEventArgs();
                le.line = line;
                OnLineReceived(le);
			}
			
			return;
		}

        public override void SendLine(string line)
        {
            lock (this)
            {
                port.WriteLine(line);
            }
        }

        public override void Disconnect()
        {
			lock (port)
			{
				port.Close();
			}
        }
    }
}
