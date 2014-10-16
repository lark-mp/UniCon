using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;
using UniCon.Interfaces;

namespace UniCon.CommunicationControl.Communicator
{
	class TCPCommunicator : ICommunicator
	{
		TcpSender sender;
		TcpReceiver receiver;

		public TCPCommunicator(string hostIP, int sendPort, int receivePort)
		{
			receiver = new TcpReceiver();
			sender = new TcpSender();
			receiver.connect(hostIP, receivePort);
			receiver.ReceiveLineEvent += new TcpReceiver.ReceiveLineEventHandler(this.OnLineReceived);
			sender.connect(hostIP, sendPort);
		}

		public override void Disconnect()
		{
			sender.disconnect();
			receiver.disconnect();
		}

		public override void SendLine(string line)
		{
			sender.send(line);
		}

	}

	class TcpSender
	{
		System.Net.Sockets.TcpClient client;
		System.Net.Sockets.NetworkStream ns;
		System.IO.MemoryStream ms;
		System.Text.Encoding enc;

		public TcpSender()
		{
			enc = System.Text.Encoding.UTF8;
		}
		~TcpSender()
		{
			disconnect();
		}
		public void connect(string host, int port)
		{
			client = new System.Net.Sockets.TcpClient();
			client.Connect(host, port);
			client.SendTimeout = 1000;
			client.ReceiveTimeout = 1000;
			ns = client.GetStream();
			ms = new System.IO.MemoryStream();
		}
		public void disconnect()
		{
			if (ms != null)
			{
				ms.Close();
			}
			if (client != null)
			{
				client.Close();
			}
			if (ns != null)
			{
				ns.Close();
			}
		}


		public void send(string message)
		{
			if (client != null && client.Connected)
			{
				try
				{
					byte[] sendBytes = enc.GetBytes(message);
					ns.Write(sendBytes, 0, sendBytes.Length);
				}
				catch (System.IO.IOException)
				{
				}
			}
		}
	}

	class TcpReceiver
	{
		System.Net.Sockets.TcpListener listener;
		System.Net.Sockets.TcpClient client;
		System.Net.Sockets.NetworkStream ns;
		System.Text.Encoding enc;
		Thread receiverThread;

		private readonly string myIP = "192.168.77.7";

		public delegate void ReceiveLineEventHandler(ReceiveLineEventArgs e);
		public event ReceiveLineEventHandler ReceiveLineEvent;

		public TcpReceiver()
		{
		}
		~TcpReceiver()
		{
			disconnect();
		}
		public void connect(string host, int port)
		{
			Console.WriteLine("waiting for connection in port" + port);

			System.Net.IPAddress address = Dns.GetHostAddresses(myIP)[0];

			Console.WriteLine(address);

			listener = new System.Net.Sockets.TcpListener(address, port);
			listener.Start();
			client = listener.AcceptTcpClient();
			ns = client.GetStream();

			Console.WriteLine("connected to address" + ((IPEndPoint)client.Client.RemoteEndPoint).Address
				+ ", port" + ((IPEndPoint)client.Client.RemoteEndPoint).Port);

			receiverThread = new Thread(new ThreadStart(this.receiveLine));
			receiverThread.Start();
		}
		public void disconnect()
		{
			if (client != null && client.Connected)
			{
				client.Close();
			}
			if (ns != null)
			{
				ns.Close();
			}
			if (listener != null)
			{
				listener.Stop();
			}

		}

		public void receiveLine()
		{
			StreamReader reader = new StreamReader(ns, System.Text.Encoding.ASCII);

			while (client.Connected == true)
			{
				try
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (ReceiveLineEvent != null)
						{
							ReceiveLineEventArgs e = new ReceiveLineEventArgs();
							e.line = line;
							ReceiveLineEvent(e);
						}

					}
				}
				catch (IOException)
				{
				}
				catch (ObjectDisposedException)
				{
				}
			}


		}

	}

}
