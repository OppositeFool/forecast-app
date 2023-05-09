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
            var t = Task.Run(() => GetCurrentWeather(true));
            t.Wait();
        }

       /* private async Task CheckIfUpdated()
        {
            await Task.Delay(10000);
            isUpdated = false;
        } */

        private async Task GetCurrentWeather(bool isTest)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromMinutes(1)));

                double testLong = -73.9;
                double testLat = 40.7;

                double realLat;
                double realLong;

                if(!isTest)
                {
                    realLat = location.Latitude;
                    realLong = location.Longitude;
                } else
                {
                    realLat = testLat;
                    realLong = testLong;
                }
              //  realLat = Math.Round(realLat, 2);
               // realLong = Math.Round(realLong, 2);

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(realLat, realLong);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        if(placemark.AdminArea != null)
                        CityName.Text = placemark.AdminArea;
                        else
                           CityName.Text = placemark.Locality;
                        CountryName.Text = placemark.CountryName;
                    }
                }
                
                using (var client = new HttpClient())
                {
                    var uri = $"https://api.openweathermap.org/data/3.0/onecall?lat={realLat}&lon={realLong}&units=metric&lang=hu&appid={SecretKeys.API_KEY}";
                    var result = await client.GetStringAsync(uri);

                    var currentWeather = JsonConvert.DeserializeObject<Weather>(result);


                    Degree.Text = $"{Math.Round(currentWeather.current.temp)}°";
                    main.Text = currentWeather.current.weather[0].main;
                    description.Text = currentWeather.current.weather[0].description;

                    if(CastRefreshView.IsRefreshing)
                    {
                        CastRefreshView.IsRefreshing = false;
                    }
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
            await GetCurrentWeather(false);
        }
        async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await GetCurrentWeather(false);
        }
    }
}