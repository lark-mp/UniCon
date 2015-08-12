using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCon.Interfaces;
using System.IO.Ports;

namespace UniCon.CommunicationControl.Communicator
{
	class CommunicatorFactory
	{
		public CommunicatorFactory()
		{
		}

		public COMCommunicator CreateCommunicator(ref SerialPort port, string portName)
		{
			return new COMCommunicator(ref port, portName);
		}

		public TCPCommunicator CreateCommunicator(string hostIP, int sendPort, int receivePort)
		{
			return new TCPCommunicator(hostIP, sendPort, receivePort);
		}
        public Cmm920Communicator CreateCommunicator(ref SerialPort port, string portName,int dummy)
        {
            return new Cmm920Communicator(ref port,portName);
        }
	}
}
