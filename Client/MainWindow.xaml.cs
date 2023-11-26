using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        static IPAddress remote_ip = IPAddress.Parse("127.0.0.1");
        static int remote_port = 4000;

        EndPoint remote_endpoint = new IPEndPoint(remote_ip, remote_port);

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {


            string message = "Send Screenshoot";

            byte[] buffer = Encoding.Default.GetBytes(message);

            Client.SendTo(buffer, remote_endpoint);

            SetImageSource();
        }

        private async void SetImageSource()
        {
            try
            {

                await Task.Run(() =>
                {
                    var msg = "";
                    var len = 0;
                    var receive_buffer = new byte[1024];

                    len = Client.ReceiveFrom(receive_buffer, ref remote_endpoint);

                    msg = Encoding.Default.GetString(receive_buffer, 0, len);
                    // Create a BitmapImage and set its UriSource
                    Dispatcher.Invoke(() =>
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(msg);
                        bitmapImage.EndInit();

                        ScreenShoot.Source = bitmapImage;
                    });
                     
                });

                // Set the Image control's source
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., file not found
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }



    }
}