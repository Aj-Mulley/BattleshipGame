using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameTest;
class Server
{
    private TcpListener listener;
    private bool running;

    public event Action<string> OnMessageReceived;

    
    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
        running = true;
        
        listener.Start();

        // fire-and-forget accept loop
        _ = AcceptLoop();
    }

    public void Stop()
    {
        running = false;
        listener.Stop();
    }

    private async Task AcceptLoop()
    {
        while (running)
        {
            try
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClient(client); // run client independently
            }
            catch
            {
                //stops server
                break;
            }
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        using NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[1024];

        while (running && client.Connected)
        {
            int bytesRead;

            try
            {
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            }
            catch
            {
                break;
            }

            if (bytesRead == 0)
                break; // client disconnected

            string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            OnMessageReceived?.Invoke(msg);
        }

        client.Close();
    }
}
