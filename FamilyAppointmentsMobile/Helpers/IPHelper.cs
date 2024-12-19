using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace FamilyAppointmentsMobile.Helpers
{
    public class IPHelper
    {
        /// <summary>
        /// Returns list of IP Addresses for all active network adapters.
        /// This method works only for subnet mask 255.255.255.0
        /// </summary>
        /// <returns>IP addresses</returns>
        public static List<IPAddress> FindLanAddresses()
        {
            List<IPAddress> addresses = new List<IPAddress>();
            try
            {
                foreach (var runningInterface in NetworkInterface.GetAllNetworkInterfaces().Where(ni => ni.OperationalStatus == OperationalStatus.Up))
                {
                    var ipProperties = runningInterface.GetIPProperties();
                    //there are pronlems with ipProperties.GatewayAddresses, if the maui Android application is running we have to take a look on it
                    foreach (var ip in ipProperties.UnicastAddresses)
                    {
                        var test = ip.Address.ToString();
                    }
                    foreach (var address in ipProperties.UnicastAddresses.Select(ua => ua.Address))
                    {
                        if (address.AddressFamily == AddressFamily.InterNetwork && !address.Equals(IPAddress.Loopback))
                        {
                            addresses.Add(address);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log.Error("FindLanAddress exception:{0}", ex);
            }

            return addresses;
        }

        public static async Task<IPAddress> FindRestServiceAddress(int port)
        {
            string baseIP = "192.168.1."; // Change this to your network's base IP
            int start = 1;
            int end = 254;

            for (int i = start; i <= end; i++)
            {
                string ip = baseIP + i;
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(ip, 100);
                if (ip == "192.168.1.150")
                {

                }
                if (reply.Status != IPStatus.Success)
                {
                    try
                    {
                        using (var client = new TcpClient())
                        {
                            if (client.ConnectAsync(ip, port).Wait(2000))
                            {
                                return IPAddress.Parse(ip);
                            }
                        }
                    }
                    catch
                    {
                        // Ignoriere Verbindungsfehler und versuche die nächste Adresse
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns IP Address of the first active network adapter in the list.
        /// </summary>
        /// <returns>IP address</returns>
        public static IPAddress FindLanAddress()
        {
            return FindLanAddresses().FirstOrDefault();
        }
    }
}
