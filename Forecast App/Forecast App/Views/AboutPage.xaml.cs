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
        public AboutPage()
        {
            InitializeComponent();
            GetCurrentWeather(true);
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

                    var hourlyCast = currentWeather.hourly;

                    Degree.Text = $"{Math.Round(currentWeather.current.temp)}°";
                    main.Text = currentWeather.current.weather[0].main;
                    description.Text = currentWeather.current.weather[0].description;
                    string iconId = currentWeather.current.weather[0].icon;
                    var imageSource = new UriImageSource { Uri = new Uri($"https://openweathermap.org/img/wn/{iconId}@4x.png") };
                    imageSource.CachingEnabled = false;
                    imageSource.CacheValidity = TimeSpan.FromHours(1);
                    weatherIcon.Source = imageSource;
                    hourlyContainer.Children.Clear();
                    int hourlyCastCount = hourlyCast.Length;
                    int i = 0;
                    if (hourlyContainer.Children.Count == 0)
                    {
                        while (i < hourlyCastCount)
                        {
                            string elementIconId = currentWeather.hourly[i].weather[0].icon;
                            var elementImageSource = new UriImageSource { Uri = new Uri($"https://openweathermap.org/img/wn/{elementIconId}@4x.png") };
                            StackLayout stack = new StackLayout
                            {
                                Padding = 10,
                                Children =
                                {
                                    new Image
                                    {
                                        Source = elementImageSource,
                                    },
                                    new Label
                                    {
                                        ClassId = "temp",
                                        Text = $"{Math.Round(hourlyCast[i].temp)}°",
                                        HorizontalTextAlignment = TextAlignment.Center,
                                    },
                                    new Label
                                    {
                                        ClassId = "desc",
                                         Text = hourlyCast[i].weather[0].description,
                                           HorizontalTextAlignment = TextAlignment.Center,
                                    }
                                }
                            };
                            hourlyContainer.Children.Add(stack);
                            i++;
                        }
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
            if (CastRefreshView.IsRefreshing)
            {
                CastRefreshView.IsRefreshing = false;
            }
        }

        async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await GetCurrentWeather(false);
        }
    }
}