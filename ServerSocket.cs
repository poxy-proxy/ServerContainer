using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.IO;

namespace ServerContainer
{
    public class ServerSocket
    {
        static Socket serverSocket;
        static string path = @"D:\\vs projects\\ServerContainer\\App_Data\\Logs.txt";
        public static void StartListening(IPAddress ip, int port)
        {

            serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(ip, port);
            serverSocket.Bind(point);
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("{0}Listen Success", serverSocket.LocalEndPoint.ToString());
               
            }
           // Console.WriteLine("{0}Listen Success", serverSocket.LocalEndPoint.ToString());
            serverSocket.Listen(10);
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
           // Console.ReadLine();


        }
        static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }
        static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {

                try
                {
                    //clientSocket accept
                    byte[] result = new byte[1024];
                    int receiveNumber = myClientSocket.Receive(result);
                   // Console.WriteLine("Receive client{0}news{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine("Receive client{0}news{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));

                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(ex.Message);

                    }
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
    }
}