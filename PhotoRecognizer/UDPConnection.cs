using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoRecognizer
{
    class UDPConnection
    {
        private const int UDPPort = 8004;
        public static void StartListener()
        {
            UdpClient listener = new UdpClient(UDPPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, UDPPort);
            Socket UDPServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte[] msg = Encoding.ASCII.GetBytes("Hi");
            try
            {
                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    if (bytes[0] == msg[0])
                    {
                        //UDPServerSocket.SendTo(msg, new IPEndPoint(groupEP.Address,UDPPort));
                        Sockets.serverAddress = groupEP.Address;
                        Console.WriteLine($"connected to server {groupEP} ");
                    }
                    else
                    {
                        MainActivity.txtOut(Encoding.Default.GetString(bytes));
                    }
                    //Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
                Console.WriteLine("Listener stopped");
            }
        }
        public static void SendBroadcast()
        {
            Socket UDPsocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            UDPsocket.EnableBroadcast = true;
            IPAddress broadcast = IPAddress.Parse("255.255.255.255");//IPAddress.Parse("192.168.1.255");
            string[] msg = new[] { "Hi" };
            byte[] sendbuf = Encoding.ASCII.GetBytes(msg[0]);
            IPEndPoint ep = new IPEndPoint(broadcast, UDPPort);

            UDPsocket.SendTo(sendbuf, ep);
            Task task = new Task(StartListener);
            task.Start();
            //StartListener();
        }
    }
}