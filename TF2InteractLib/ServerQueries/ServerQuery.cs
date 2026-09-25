using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TF2InteractLib.ServerQueries;

public class ServerQuery
{
    private UdpClient _client = new();

    public async Task QueryIp(string ipAddress, int port)
    {
        try
        {
            Console.WriteLine("starting");
            IPEndPoint endPoint = new(IPAddress.Parse(ipAddress), port);
            Console.WriteLine("parsed");
            List<byte> packet = [0xFF, 0xFF, 0xFF, 0xFF, 0x55]; // header
            packet.AddRange(BitConverter.GetBytes(0xFFFFFFFF));
            Console.WriteLine("wrote packet");
            await _client.SendAsync(packet.ToArray(), packet.Count, endPoint);
            Console.WriteLine("sent");
            UdpReceiveResult response = await _client.ReceiveAsync();
            Console.WriteLine(Encoding.UTF8.GetString(response.Buffer));
            Console.WriteLine("got response.");
            _client.Close();
            _client.Dispose();
            _client = new UdpClient();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}