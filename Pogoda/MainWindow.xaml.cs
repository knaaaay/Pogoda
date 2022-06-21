using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Pogoda
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start(); //INICJOWANIE INFORMACJI STATYCZNYCH MIAST PRZY WLACZANIU APLIKACJI
        }

        //PRZESUWANIE OKIENKA APLIKACJI
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //ZAMYKWANIE APLIKACJI
        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //MINIMALIZOWANIE APLIKACJI
        private void minimalizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // KLUCZ API
        string APIKey = "0f8e30624fbc2f7da78da0e88a1eaa71";

        //PRZYCISKI
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            getWeather();
        }

        //ODSWIEZANIE INFORMACJI STATYCZNYCH MIAST
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        //ZAPISYWANIE INFORMACJI DO PLIKI - PRZYCISK
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        //ZAPISYWANIE INFORMACJI DO PLIKU W FOLDERZE PROJEKTU
        private void Save()
        {
            try
            {

                if (labCity.Content != null) //BEZ PODANIA MIASTA I KLIKNIECA WYSZUKAJ NIE ZAPISZEMY DANYCH, KTORYCH NIE MA
                {
                    StreamWriter strWriter = new StreamWriter("Pogoda " + labCity.Content + DateTime.Now.ToString(" MM/dd/yyyy HH/mm") + ".txt");
                    strWriter.WriteLine("Miasto: " + labCity.Content);
                    strWriter.WriteLine("Temperatura: " + labTemp.Content);
                    strWriter.WriteLine("Temperatura odczuwalna: " + labTempFeelsLike.Content);
                    strWriter.WriteLine("Prędkość wiatru: " + labWind.Content);
                    strWriter.WriteLine("Wilgotność: " + labHumidity.Content);
                    strWriter.WriteLine("Zachmurzenie: " + labCondition.Content);
                    strWriter.Close();
                    MessageBox.Show("Zapisano pomyślnie");
                }
                else {
                    MessageBox.Show("Brak danych");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ZBIERANIE DANYCH Z API RÓZNYCH MIAST
        private void getWeather()
        {
            using (WebClient web = new WebClient())
            {
                try
                {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric", TBCity.Text, APIKey);
                    var json = web.DownloadString(url);
                    weatherInformation.root Info = JsonConvert.DeserializeObject<weatherInformation.root>(json);


                    labCondition.Content = Info.weather[0].description.ToString();
                    labTemp.Content = Info.main.temp.ToString() + " °C";
                    labTempFeelsLike.Content = Info.main.feels_like.ToString() + " °C";
                    labHumidity.Content = Info.main.humidity.ToString() + " %";
                    labPressure.Content = Info.main.pressure.ToString() + " hPa";
                    labWind.Content = Info.wind.speed.ToString() + " m/s";
                    labTime.Content = DateTime.Now.ToString("HH:mm");
                    labCity.Content = TBCity.Text.ToUpper();

                    double Temp = Convert.ToDouble(Info.main.temp);
                    double TempFeelsLike = Convert.ToDouble(Info.main.temp);

                    if (Temp > 0)
                    {
                        if (Temp > 20)
                        {
                            labTemp.Foreground = Brushes.Red;
                        }
                        else
                        {
                            labTemp.Foreground = Brushes.Yellow;
                        }
                    }
                    else
                    {
                        labTemp.Foreground = Brushes.Blue;
                    }

                    if (TempFeelsLike > 0)
                    {
                        if (TempFeelsLike > 20)
                        {
                            labTempFeelsLike.Foreground = Brushes.Red;
                        }
                        else
                        {
                            labTempFeelsLike.Foreground = Brushes.Yellow;
                        }
                    }
                    else
                    {
                        labTempFeelsLike.Foreground = Brushes.Blue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //ZBIERANIE DANYCH Z API STATYCZNYCH MIAST
        private void Start()
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q=Warszawa&appid={0}&units=metric", APIKey);
                var json = web.DownloadString(url);
                weatherInformation.root Info = JsonConvert.DeserializeObject<weatherInformation.root>(json);
                labWarszawaTemp.Content = Info.main.temp.ToString() + " °C";

            }
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q=Wroclaw&appid={0}&units=metric", APIKey);
                var json = web.DownloadString(url);
                weatherInformation.root Info = JsonConvert.DeserializeObject<weatherInformation.root>(json);
                labWroclawTemp.Content = Info.main.temp.ToString() + " °C";

            }
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q=Gdansk&appid={0}&units=metric", APIKey);
                var json = web.DownloadString(url);
                weatherInformation.root Info = JsonConvert.DeserializeObject<weatherInformation.root>(json);
                labGdanskTemp.Content = Info.main.temp.ToString() + " °C";

            }
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q=Krakow&appid={0}&units=metric", APIKey);
                var json = web.DownloadString(url);
                weatherInformation.root Info = JsonConvert.DeserializeObject<weatherInformation.root>(json);
                labKrakowTemp.Content = Info.main.temp.ToString() + " °C";

            }
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q=Poznan&appid={0}&units=metric", APIKey);
                var json = web.DownloadString(url);
                weatherInformation.root Info = JsonConvert.DeserializeObject<weatherInformation.root>(json);
                labPoznanTemp.Content = Info.main.temp.ToString() + " °C";

            }
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q=Rzeszow&appid={0}&units=metric", APIKey);
                var json = web.DownloadString(url);
                weatherInformation.root Info = JsonConvert.DeserializeObject<weatherInformation.root>(json);
                labRzeszowTemp.Content = Info.main.temp.ToString() + " °C";

            }
        }
    }
}
