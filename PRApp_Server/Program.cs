﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using PRApp_Server;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocketTcpServer
{
    class Program
    {
        static int port = 8005; // порт для приема входящих запросов
        static NeuralNetworkClassifiyer nnc = new NeuralNetworkClassifiyer();

        static void Main(string[] args)
        {
            // nnc.Init();
            nnc.LoadTrainedNN(); 

            Task task = new Task(UDPListener.StartListener);
            task.Start();
            //UDPListener.StartListener();

            // testNeural();
            listenerCycle();
        }

        static void testNeural()
        {
            string testPathToImage = @"E:\PRApp\PRApp_Server\bin\Debug\netcoreapp2.1\picBy192.168.56.1_650.jpg";
            //nnc.Classify(testPathToImage);

        }

        static void listenerCycle()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ipAddr = "";
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddr = ip.ToString();
                    break;
                }
            }

            var listener = new TcpListener(IPAddress.Parse(ipAddr), port);
            Console.WriteLine("local ip address:" + ipAddr);
            listener.Start();
            while (true)
            {
                string lastFilePath;
                IPAddress userIP;
                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                using (var output = File.Create("picBy" + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + "_" + DateTime.Now.Millisecond.ToString() + ".jpg"))
                {
                    userIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                    Console.Write("Receiving the file...");

                    var buffer = new byte[1024];
                    int bytesRead, cx=0;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) >0)//== buffer.Length) // это был не баг, так и должно быть
                    {
                        output.Write(buffer, 0, bytesRead);
                        if (cx < 40)
                            cx++;
                        else
                        {
                            Console.Write(".");
                            cx = 0;
                        } 
                    }
                    Console.WriteLine();
                    Console.WriteLine("File " + output.Name + " successfully saved");
                    lastFilePath = output.Name;

                }


                Console.WriteLine("Trying to classify object at the image...");
                nnc.Classify(lastFilePath, userIP);
            }
        }
        
            
            /*
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    // отправляем ответ
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            */
        
    }
}