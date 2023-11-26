using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
#pragma warning disable

namespace Server
{
    public class Server
    {

        static void Main(string[] args)
        {
            string Path = @"C:\Users\Elgun\Desktop\Capture.png";

            var Listener = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
            );





            var IP = IPAddress.Parse("127.0.0.1");   
            var Port = 4000;                         

            var EndPoint = new IPEndPoint(IP, Port); 

            var msg = "";
            var len = 0;
            var buffer = new byte[ushort.MaxValue - 29];

            Listener.Bind(EndPoint);

            EndPoint remote_EndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                len = Listener.ReceiveFrom(buffer, ref remote_EndPoint);

                msg = Encoding.Default.GetString(buffer, 0, len);

                if (msg == "Send Screenshoot")
                {
                    Console.WriteLine($"{remote_EndPoint.ToString()}: {msg}");

                    CaptureMyScreen();
                    
                    byte[] send_buffer = Encoding.Default.GetBytes(Path);
                    
                    Listener.SendTo(send_buffer, remote_EndPoint);

                    Console.WriteLine("Has Been Sent !");

                }
            }

        }

        static private void CaptureMyScreen()
        {
            try
            {
                Bitmap Capture_bitmap = new Bitmap(1024, 768, PixelFormat.Format32bppArgb);

                Rectangle Capture_rectangle = Screen.AllScreens[0].Bounds;

                Graphics Capture_graphics = Graphics.FromImage(Capture_bitmap);
                Capture_graphics.CopyFromScreen(Capture_rectangle.Left, Capture_rectangle.Top, 0, 0, Capture_rectangle.Size);

                Capture_bitmap.Save(@"C:\Users\Elgun\Desktop\Capture.png", ImageFormat.Png);

                Console.WriteLine("Has Been Captured Successfully !");
             
            }
            
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
