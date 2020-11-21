using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace IntegralLeft
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> serversAdres;
        private List<int> serversPort;

        public MainWindow()
        {
            InitializeComponent();
            serversAdres = new List<string>();
            serversPort = new List<int>();
        }

        private void AddServerListBox_Click(object sender, RoutedEventArgs e)
        {
            string serverAdres = TextServerAddress.Text;
            int serverPort = Convert.ToInt32(TextServerPort.Text);


            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(serverAdres, serverPort);
                if (socket.Connected)
                {
                    ListBoxServer.Items.Add(serverAdres + ":" + serverPort);
                    serversAdres.Add(serverAdres);
                    serversPort.Add(serverPort);

                    TextServerAddress.Clear();
                    TextServerPort.Clear();
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Удаленный сервер не запущен!");
            }
        }

        private void SolutionOne_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            double result = 0;

            stopwatch.Start();
            SolutionIntegral integral = new SolutionIntegral(Integral.Text, Convert.ToInt32(Ni.Text), Convert.ToInt32(Nj.Text), Convert.ToInt32(Nk.Text),
                Convert.ToDouble(S_i1.Text), Convert.ToDouble(E_i1.Text), Convert.ToDouble(S_i2.Text), Convert.ToDouble(E_i2.Text), Convert.ToDouble(S_i3.Text), Convert.ToDouble(E_i3.Text));
            result = integral.Solution();
            stopwatch.Stop();

            ResultTime.Content = Convert.ToString(stopwatch.ElapsedMilliseconds / 1000);
            Solution.Content = result.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string textFunc = Integral.Text;
            int ni = Convert.ToInt32(Ni.Text);
            int nj = Convert.ToInt32(Nj.Text);
            int nk = Convert.ToInt32(Nk.Text);
            double s_i1 = Convert.ToDouble(S_i1.Text);
            double e_i1 = Convert.ToDouble(E_i1.Text);

            double s_i2 = Convert.ToDouble(S_i2.Text);
            double e_i2 = Convert.ToDouble(E_i2.Text);

            double s_i3 = Convert.ToDouble(S_i3.Text);
            double e_i3 = Convert.ToDouble(E_i3.Text);

            List<string> dataSend = new List<string>();

            double hx = (e_i1 - s_i1) / ni;
            double hy = (e_i2 - s_i2) / nj;
            double hz = (e_i3 - s_i3) / nk;

            double result = 0;

            Socket[] sockets = new Socket[serversAdres.Count];

            var calc = new Sprache.Calc.XtensibleCalculator();

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            double temp_hx = s_i1;
            while (temp_hx <= e_i1 - hx)
            {
                double temp_hy = s_i2;
                while (temp_hy <= e_i2 - hy)
                {
                    double temp_hz = s_i3;
                    while (temp_hz <= e_i3 - hz)
                    {

                        double temp = temp_hz;
                        for (int i = 0; i < serversAdres.Count; i++)
                        {
                            if (temp < e_i3)
                            {
                                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(serversAdres[i]), serversPort[i]);
                                sockets[i] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                sockets[i].Connect(iPEndPoint);

                                byte[] sendData = Encoding.Unicode.GetBytes(textFunc + ";" + temp_hx + ";" + temp_hy + ";" + temp + ";");
                                sockets[i].Send(sendData);
                                temp += hz;

                            }
                            else
                            {
                                break;
                            }
                        }
                        temp = temp_hz;
                        for (int i = 0; i < serversAdres.Count; i++)
                        {
                            int bytes = 0;
                            byte[] recvData = new byte[1024];
                            string recvdata = "";
                            if (temp < e_i3)
                            {
                                do
                                {
                                    bytes = sockets[i].Receive(recvData);
                                    if (bytes != 0)
                                    {
                                        recvdata = Encoding.Unicode.GetString(recvData);
                                        result += Convert.ToDouble(recvdata);
                                    }
                                } while (sockets[i].Available > 0);

                                sockets[i].Shutdown(SocketShutdown.Both);
                                sockets[i].Close();
                                temp += hz;
                            }
                        }
                        temp_hz += hz * serversAdres.Count;
                    }

                    result *= hz;
                    temp_hy += hy;
                }
                result *= hy;
                temp_hx += hx;
            }
            result *= hx;
            stopwatch.Stop();

            resultLabel.Dispatcher.Invoke((Action)(() =>
            {
                resultLabel.Content = result.ToString();
            }));

            ResultTimeNet.Dispatcher.Invoke((Action)(() =>
            {
                ResultTimeNet.Content = Convert.ToString(stopwatch.ElapsedMilliseconds / 1000);
            }));
        }
    }
}
