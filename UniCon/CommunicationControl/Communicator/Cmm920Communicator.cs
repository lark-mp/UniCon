using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCon.Interfaces;
using System.IO.Ports;

namespace UniCon.CommunicationControl.Communicator
{
    class Cmm920Communicator : ICommunicator
    {
        private SerialPort port;

        private const int TX_BUFFERSIZE = 768;
        private byte[] txBuffer;

        public Cmm920Communicator(ref SerialPort port, string portName)
        {
            this.port = port;
            port.PortName = portName;
            port.Open();
            port.RtsEnable = true;

			port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.portDataReceived);

            txBuffer = new byte[TX_BUFFERSIZE];
            lineBuffer = "";
            rawBuffer = new byte[RX_BUFFERSIZE];
            messageBuffer = new byte[RX_BUFFERSIZE];

            requestReset();
            System.Threading.Thread.Sleep(100);
            setConfigMode();
            System.Threading.Thread.Sleep(100);
            setRfParameter();
            System.Threading.Thread.Sleep(100);
            setSendMode();
            System.Threading.Thread.Sleep(100);
		}

        public override void SendLine(string line)
        {
            byte[] byteLine = System.Text.Encoding.ASCII.GetBytes(line+"\n");

            sendMessage(byteLine,byteLine.Length);
        }

        public override void Disconnect()
        {
			lock (port)
			{
				port.Close();
			}
        }

        /// <summary>
        /// TX Section
        /// </summary>



        private void setConfigMode()
        {
            byte[] sendData = new byte[2];
            sendData[0] = 0x02;
            sendData[1] = 0x01;
            sendCommand((short)0x0001, sendData, 2);
        }

        private void setSendMode()
        {
            byte[] sendData = new byte[2];
            sendData[0] = 0x02;
            sendData[1] = 0x02;
            sendCommand((short)0x0001, sendData, 2);
        }

        private void setRfParameter()
        {
            byte[] sendData = new byte[12];
            sendData[0] = 0x02;
            sendData[1] = 0x02;  //50kbps
            sendData[2] = 0x60;  //ch60
            sendData[3] = 0x03;  //TxPower
            sendData[4] = (byte)0x85;  //Tx Carrier SenseLevel
            sendData[5] = (byte)0x95;  //Rx Carrier SenseLevel
            sendData[6] = 0x00;  //Carrier Sense timer
            sendData[7] = (byte)0x82;
            sendData[8] = 0x06;  //Carrier Sense count
            sendData[9] = 0x03;  //RETRY COUNT
            sendData[10] = 0x00;  //RETRY DELAY
            sendData[11] = (byte)0x64;


            sendCommand((short)0x0012, sendData, 12);
        }

        private void readRfParameter()
        {
            byte[] sendData = new byte[1];
            sendData[0] = 0x01;
            sendCommand((short)0x0012, sendData, 1);
        }

        private void setAddress(short panId, long longAddress, short shortAddress)
        {
            byte[] sendData = new byte[13];
            sendData[0] = 0x02;
            sendData[1] = (byte)(panId >> 8);
            sendData[2] = (byte)(panId & 0xFF);
            sendData[3] = (byte)(longAddress >> 56 & 0xFF);
            sendData[4] = (byte)(longAddress >> 48 & 0xFF);
            sendData[5] = (byte)(longAddress >> 40 & 0xFF);
            sendData[6] = (byte)(longAddress >> 32 & 0xFF);
            sendData[7] = (byte)(longAddress >> 24 & 0xFF);
            sendData[8] = (byte)(longAddress >> 16 & 0xFF);
            sendData[9] = (byte)(longAddress >> 8 & 0xFF);
            sendData[10] = (byte)(longAddress & 0xFF);
            sendData[11] = (byte)(shortAddress >> 8);
            sendData[12] = (byte)(shortAddress & 0xFF);

            sendCommand((short)0x0011, sendData, 13);
        }

        private void readAddress()
        {
            byte[] sendData = new byte[1];
            sendData[0] = 0x01;
            sendCommand((short)0x0011, sendData, 1);
        }

        private void sendMessage(byte[] data, int length)
        {
            byte[] sendData = new byte[3 + length];
            sendData[0] = 0x00;
            sendData[1] = (byte)(0x00 | ((length + 4) >> 8));
            sendData[2] = (byte)(length + 4); //PHR
            for (int i = 0; i < length; i++)
            {
                sendData[i + 3] = data[i];
            }

            sendCommand((short)0x0101, sendData, length + 3);
        }

        private void requestReset()
        {
            byte[] sendData = new byte[1];
            sendData[0] = 0x00;
            sendCommand((short)0x00F2, sendData, 1);
        }

        private void sendCommand(short cmd, byte[] data, int dataLength){
        constructTxData(cmd, data, dataLength);

        lock (this)
        {
            port.Write(txBuffer, 0, 11 + dataLength);
        }
    }
        private void constructTxData(short cmd, byte[] data, int dataLength)
        {
            int length = dataLength + 5;

            txBuffer[0] = 0x10;  //DLE
            txBuffer[1] = 0x02;  //STX
            txBuffer[2] = (byte)(length >> 8);    //LENGTH
            txBuffer[3] = (byte)(length & 0xFF);
            txBuffer[4] = (byte)(cmd >> 8);  //COMMAND
            txBuffer[5] = (byte)(cmd & 0xFF);
            txBuffer[6] = 0x00;  //RESULT
            txBuffer[7] = 0x00;  //RESULT_DETAIL

            for (int i = 0; i < dataLength; i++)
            {
                txBuffer[8 + i] = data[i];
            }
            byte sum = calcChecksum(dataLength);
            txBuffer[8 + dataLength] = sum;
            txBuffer[9 + dataLength] = 0x10;
            txBuffer[10 + dataLength] = 0x03;
        }
        private byte calcChecksum(int datalength)
        {
            byte sum = 0;
            for (int i = 2; i < 8 + datalength; i++)
            {
                sum += txBuffer[i];
            }

            return (byte)-sum;
        }

        /// <summary>
        /// RX Section
        /// </summary>


        enum DECODE_STATE
        {
            DLE1,
            STX,
            DATALEN1,
            DATALEN2,
            CMD1,
            CMD2,
            RESULT,
            RESULT_DETAIL,
            DATA,
            DLE2,
            ETX,
        };

        private const int RX_BUFFERSIZE = 1024;
        byte[] rawBuffer;
        byte[] messageBuffer;
        DECODE_STATE state;
        int dataLength;
        int dataCount;
        byte cmd1;
        byte cmd2;
        byte result;
        byte resultDetail;
        byte sum;
        string lineBuffer;
        int electricFieldStrength;

        private void portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // ポートのバッファにあるだけ読む
            int length = port.BytesToRead;
            port.Read(rawBuffer, 0, length);

            for (int i = 0; i < length; i++)
            {
                decodeBuf(rawBuffer[i]);
            }

            return;
        }

        private void decodeBuf(byte buf)
        {
            if (state == DECODE_STATE.DLE1)
            {
                if (buf == 0x10)
                {
                    state = DECODE_STATE.STX;
                }
                else
                {
                    state = DECODE_STATE.DLE1;
                }

                //System.out.println("dle1");
            }
            else if (state == DECODE_STATE.STX)
            {
                if (buf == 0x02)
                {
                    state = DECODE_STATE.DATALEN1;
                }
                else
                {
                    state = DECODE_STATE.DLE1;
                }
                sum = 0;
                //System.out.println("stx");
            }
            else if (state == DECODE_STATE.DATALEN1)
            {
                state = DECODE_STATE.DATALEN2;
                dataLength = (buf << 8) & 0xFF00;
                sum += buf;

                //System.out.println("datalen 1");
            }
            else if (state == DECODE_STATE.DATALEN2)
            {
                state = DECODE_STATE.CMD1;
                dataLength = dataLength | (buf & 0xFF);
                sum += buf;

                //System.out.println("datalen2:" + dataLength);
            }
            else if (state == DECODE_STATE.CMD1)
            {
                cmd1 = buf;
                sum += buf;
                state = DECODE_STATE.CMD2;

                //System.out.println("cmd1");
            }
            else if (state == DECODE_STATE.CMD2)
            {
                cmd2 = buf;
                sum += buf;
                state = DECODE_STATE.RESULT;

                //System.out.println("cmd2");
            }
            else if (state == DECODE_STATE.RESULT)
            {
                result = buf;
                sum += buf;
                state = DECODE_STATE.RESULT_DETAIL;

                //System.out.println("result1");
            }
            else if (state == DECODE_STATE.RESULT_DETAIL)
            {
                resultDetail = buf;
                sum += buf;
                state = DECODE_STATE.DATA;
                dataCount = 0;

                //System.out.println("result2");
            }
            else if (state == DECODE_STATE.DATA)
            {
                if (dataCount + 5 < dataLength)
                {
                    messageBuffer[dataCount] = buf;
                    sum += buf;
                    dataCount++;

                    //System.out.println("data"+dataCount);
                }
                else
                {//SUM
                    sum += buf;

                    if (sum == 0)
                    {//checksum error;
                        state = DECODE_STATE.DLE2;
                    }
                    else
                    {
                        state = DECODE_STATE.DLE1;
                    }

                    //System.out.println("sum:"+sum);
                }
            }
            else if (state == DECODE_STATE.DLE2)
            {
                if (buf == 0x10)
                {
                    state = DECODE_STATE.ETX;
                }
                else
                {
                    state = DECODE_STATE.DLE1;
                }

                //System.out.println("dle2");
            }
            else if (state == DECODE_STATE.ETX)
            {
                if (buf == 0x03 && sum == 0)
                {
                    decodeData(messageBuffer, dataCount);
                }
                state = DECODE_STATE.DLE1;

                //System.out.println("etx");
            }
        }

        private void decodeData(byte[] buffer, int length)
        {
            if (cmd1 == (byte)0x81 && cmd2 == (byte)0x01)
            {
                decodeMessage(buffer, length);
            }
        }

        private void decodeMessage(byte[] buffer,int length){
            if(result == 0x00 && resultDetail == 0x00 && buffer[0] == 0x01){
                string newLine = System.Text.Encoding.ASCII.GetString(buffer);
                lineBuffer += newLine.Substring(5, length - 11);

                int tmpElectricFieldStrength = bcdToInt(buffer[2]);
                if (tmpElectricFieldStrength <= 10)
                {
                    electricFieldStrength = tmpElectricFieldStrength + 100;
                }
                else
                {
                    electricFieldStrength = tmpElectricFieldStrength;
                }

                if (!lineBuffer.Contains("\n"))
                {
                    return;
                }
                while (lineBuffer.Contains("\n"))
                {
                    // line に1行分取り出す
                    string line = lineBuffer.Split('\n')[0];
                    // 1行分を rawresult から取り除く
                    lineBuffer = lineBuffer.Substring(lineBuffer.Split('\n')[0].Length + 1);

                    ReceiveLineEventArgs le = new ReceiveLineEventArgs();
                    //le.line = line + ",-" + electricFieldStrength + "[dBm]";
                    le.line = line;
                    OnLineReceived(le);
                }
            }
        }
        private int bcdToInt(byte bcd)
        {
            int upper = (0xFF & bcd) / 0x10;
            int lower = (0xFF & bcd) % 0x10;

            return upper * 10 + lower;
        }
    }
}
