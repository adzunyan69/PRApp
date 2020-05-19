using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PRApp_Server
{
    public class UDPListener
    {
        private const int listenPort = 8004;
        public static UdpClient listener = new UdpClient(listenPort);
        public static IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
        public static Socket UDPServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public static void sendToUser(string messageToUser, IPAddress userIP)
        {
            byte[] msg = Encoding.ASCII.GetBytes(messageToUser);
            UDPServerSocket.SendTo(msg, new IPEndPoint(groupEP.Address, listenPort)); //groupEP);
        }
            public static void StartListener()
            {
            byte[] msg = Encoding.ASCII.GetBytes("Hi") ;
                try
                {
                    while (true)
                    {
                        byte[] bytes = listener.Receive(ref groupEP);
                        if (bytes[0] == msg[0])
                        {
                            Thread.Sleep(2000);
                            sendToUser("Hi", groupEP.Address);
                            //UDPServerSocket.SendTo(msg, new IPEndPoint(groupEP.Address, listenPort)); //groupEP);
                            Console.WriteLine($"User {groupEP} connected");
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

            public static void Main()
            {
                StartListener();
            }
    }
}
