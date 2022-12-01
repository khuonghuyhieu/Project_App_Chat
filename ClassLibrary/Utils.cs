using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace ClassLibrary
{
    public class Utils
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static void SendCommon(object res, Socket socket)
        {
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(res);
            socket.Send(jsonUtf8Bytes, jsonUtf8Bytes.Length, SocketFlags.None);
        }
        public static bool ConfirmPassword(string password, string confirmPassword)
        {
            if (!password.Equals(confirmPassword))
                return false;

            return true;
        }
        public static void KillThread(Thread thread)
        {
            try
            {
                if (thread != null)
                    thread.Interrupt();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string ClearJson(string json)
        {
            //return json.Replace("\\u0022", "\"").Replace("\0", "");
            //return json.Replace("\\u0022", "\"");
            return json.Replace("\0", "");
        }
    }
}
