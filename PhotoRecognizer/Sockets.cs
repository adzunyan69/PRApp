using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PhotoRecognizer
{
    class Sockets
    {
        public static System.Net.IPAddress serverAddress = System.Net.IPAddress.Parse("192.168.2.138");
        public static void sendThis(string path)
        {

            System.Net.IPAddress IP = serverAddress;//System.Net.IPAddress.Parse("192.168.0.103");
            System.Net.IPEndPoint endPoint = new System.Net.IPEndPoint(IP, 8005);

            Socket client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

            client.Connect(endPoint);

            Console.WriteLine("Sending {0} to the host.", path);
            client.SendFile(path);

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}