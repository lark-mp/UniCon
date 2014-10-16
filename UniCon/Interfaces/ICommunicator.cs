using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCon.Interfaces
{
	abstract class ICommunicator
	{
		public delegate void ReceiveLineEventHandler(ReceiveLineEventArgs e);
		public event ReceiveLineEventHandler ReceiveLineEvent;
		protected virtual void OnLineReceived(ReceiveLineEventArgs e)
		{
			ReceiveLineEventHandler eventHandler = ReceiveLineEvent;

			if (eventHandler != null)
			{
				eventHandler(e);
			}
		}
		public abstract void SendLine(string line);
		public abstract void Disconnect();
	}
	public class ReceiveLineEventArgs : EventArgs
	{
		public string line;
	}

}
