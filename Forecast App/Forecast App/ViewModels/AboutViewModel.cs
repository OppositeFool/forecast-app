using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using Forecast_App.Models;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Forecast_App.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        public AboutViewModel()
        {
            //GetCurrentWeather();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void GetCurrentWeather()
        {
            using (var client = new HttpClient())
            {
                var uri = $"https://api.openweathermap.org/data/3.0/onecall?lat=47.6&lon=17.6&appid={SecretKeys.API_KEY}";
                var result = await client.GetStringAsync(uri);

                var currentWeather = JsonConvert.DeserializeObject<Weather>(result);

                Degree = 20.6;
            }
        }
        double _degree;
        public double Degree
        {
            get { return _degree; }
            set { _degree = value; OnPropertyChanged(nameof(Degree)); }
        }
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}