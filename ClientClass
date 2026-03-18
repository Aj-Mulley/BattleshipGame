using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        string serverIp = "192.168.0.207";
        int port = 5000;

        TcpClient client = new TcpClient(serverIp, port);
        NetworkStream stream = client.GetStream();

        Console.WriteLine("Connected to server. Type messages:");

        while (true)
        {
            string message = Console.ReadLine();
            if (string.IsNullOrEmpty(message)) break;

            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        stream.Close();
        client.Close();
    }
}
