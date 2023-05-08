using Forecast_App.Models;
using Forecast_App.ViewModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forecast_App.Views
{
    public partial class AboutPage : ContentPage
    {
        bool isUpdated = true;

        public AboutPage()
        {
            InitializeComponent();
            //BindingContext = new AboutViewModel
            GetCurrentWeather();
        }

       /* private async Task CheckIfUpdated()
        {
            await Task.Delay(10000);
            isUpdated = false;
        } */

        private async Task GetCurrentWeather()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromMinutes(1)));

                double testLong = -73.935242;
                double testLat = 40.730610;

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(testLat, testLong);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        CityName.Text = placemark.AdminArea;
                        CountryName.Text = placemark.CountryName;
                    }
                }
                using (var client = new HttpClient())
                {
                    var uri = $"https://api.openweathermap.org/data/3.0/onecall?lat={testLat}&lon={testLong}&units=metric&&appid=debug";
                    var result = await client.GetStringAsync(uri);

                    var currentWeather = JsonConvert.DeserializeObject<Weather>(result);


                    Degree.Text = $"{Math.Round(currentWeather.current.temp).ToString()}°";
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild Feature", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild Permission", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild Other", ex.Message, "OK");
            }
        }

        private async void ButtonGetCurrentLoc_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromMinutes(1)));

                if (location != null)
                    {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                        var placemark = placemarks?.FirstOrDefault();
                        if (placemark != null)
                        {
                             CityName.Text = placemark.Locality;
                        CountryName.Text = placemark.CountryName;
                        }
                    }
                using (var client = new HttpClient())
                {
                    var uri = $"https://api.openweathermap.org/data/3.0/onecall?lat={location.Latitude}&lon={location.Longitude}&units=metric&&appid=debug";
                    var result = await client.GetStringAsync(uri);

                    var currentWeather = JsonConvert.DeserializeObject<Weather>(result);


                    Degree.Text = $"{Math.Round(currentWeather.current.temp).ToString()}°";
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild Feature", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild Permission", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild Other", ex.Message, "OK");
            }
        }
        async void RefreshView_Refreshing(object sender, EventArgs e)
        {
           /* if (!isUpdated)
            {
              await GetCurrentWeather();
              CastRefreshView.IsRefreshing = false;
              await CheckIfUpdated();
                isUpdated = true;
            } */
        }
    }
}