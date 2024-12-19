using System;
using System.Net.Sockets;
using System.Net;

namespace FamilyAppointmentsMobile.Discovery
{
    public class MulticastUdpClient : IDisposable
    {
        private bool disposing = false;
        private bool disposed = false;

        private UdpClient udpClient;
        private int port;
        private IPAddress multicastIPaddress;
        private IPAddress localIPaddress;
        private IPEndPoint localEndPoint;
        private IPEndPoint remoteEndPoint;

        public MulticastUdpClient(IPAddress multicastIPaddress, int port, IPAddress localIPaddress = null)
        {
            // Store params
            this.multicastIPaddress = multicastIPaddress;
            this.port = port;
            this.localIPaddress = localIPaddress;
            if (localIPaddress == null)
                this.localIPaddress = IPAddress.Any;

            // Create endpoints
            remoteEndPoint = new IPEndPoint(this.multicastIPaddress, port);
            localEndPoint = new IPEndPoint(this.localIPaddress, port);

            // Create and configure UdpClient
            udpClient = new UdpClient();
            // Bind, Join
            udpClient.Client.Bind(localEndPoint);
            udpClient.JoinMulticastGroup(this.multicastIPaddress, this.localIPaddress);

            // Start listening for incoming data
            udpClient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

        /// <summary>
        /// Send the buffer by UDP to multicast address
        /// </summary>
        /// <param name="bufferToSend"></param>
        public void SendMulticast(byte[] bufferToSend)
        {
            udpClient.Send(bufferToSend, bufferToSend.Length, remoteEndPoint);
        }

        /// <summary>
        /// Callback which is called when UDP packet is received
        /// </summary>
        /// <param name="ar"></param>
        private void ReceivedCallback(IAsyncResult ar)
        {
            try
            {
                // Get received data
                Byte[] receivedBytes = udpClient.EndReceive(ar, ref remoteEndPoint);

                // fire event if defined
                if (UdpMessageReceived != null)
                    UdpMessageReceived(this, receivedBytes);

                // Restart listening for udp data packages
                udpClient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException)
            {
                //log.Info("Udp client disposed further youvi responses will not be registered");
            }
            catch (Exception e)
            {
                //log.Warn("Exception occured while receiving data");
                //log.Warn(e);
            }
        }

        /// <summary>
        /// Event handler which will be invoked when UDP message is received
        /// </summary>
        public event EventHandler<byte[]> UdpMessageReceived;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            //log.Info("Dispose executed.");
            if (disposed)
            {
                return;
            }

            disposing = isDisposing;
            if (isDisposing)
            {
                if (udpClient != null)
                {
                    udpClient.Close();
                    udpClient = null;
                }
            }

            disposed = true;
        }
    }
}
