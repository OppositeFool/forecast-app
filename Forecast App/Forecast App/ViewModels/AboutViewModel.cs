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
using System.Linq;
using Plugin.LocalNotification;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.IO;

namespace Forecast_App.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {

        private string unitName;
        public string UnitName
        {
            get { return unitName; }
            set
            {
                if (unitName != value)
                {
                    unitName = value;
                    OnPropertyChanged(nameof(UnitName));
                }
            }
        }
        private string unit;
        public string Unit
        {
            get { return unit; }
            set
            {
                if (unit != value)
                {
                    unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        private string countryName;
        public string CountryName
        {
            get { return countryName; }
                set
                {
                    if (countryName != value)
                    {
                    countryName = value;
                        OnPropertyChanged(nameof(CountryName));
                    }
                }
        }
        public string cityName { get; set; }
        public string CityName
        {
            get { return cityName; }
            set
            {
                if (cityName != value)
                {
                    cityName = value;
                    OnPropertyChanged(nameof(CityName));
                }
            }
        }
        public string tempValue { get; set; }
        public string TempValue
        {
            get { return tempValue; }
            set
            {
                if (tempValue != value)
                {
                    tempValue = $"{value}°";
                    OnPropertyChanged(nameof(TempValue));
                }
            }
        }
        public string mainCondition { get; set; }
        public string MainCondition
        {
            get { return mainCondition; }
            set
            {
                if (mainCondition != value)
                {
                    mainCondition = value;
                    OnPropertyChanged(nameof(MainCondition));
                }
            }
        }
        public string description { get; set; }
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public ImageSource imgSource { get; set; }
        public ImageSource ImgSource
        {
            get { return imgSource; }
            set
            {
                if (imgSource != value)
                {
                    imgSource = value;
                    OnPropertyChanged(nameof(ImgSource));
                }
            }
        }
        private string feelsLike;
        public string FeelsLike
        {
            get { return feelsLike; }
            set
            {
                if (feelsLike != value)
                {
                    feelsLike = value;
                    OnPropertyChanged(nameof(FeelsLike));
                }
            }
        }
        private string windSpeed;
        public string WindSpeed
        {
            get { return windSpeed; }
            set
            {
                if (windSpeed != value)
                {
                    windSpeed = value;
                    OnPropertyChanged(nameof(WindSpeed));
                }
            }
        }
        private string humidity;
        public string Humidity
        {
            get { return humidity; }
            set
            {
                if (humidity != value)
                {
                    humidity = value;
                    OnPropertyChanged(nameof(Humidity));
                }
            }
        }
        private string visibility;
        public string Visibility
        {
            get { return visibility; }
            set
            {
                if (visibility != value)
                {
                    visibility = value;
                    OnPropertyChanged(nameof(Visibility));
                }
            }
        }
        private bool activityIsBusy;
        public bool ActivityIsBusy
        {
            get { return activityIsBusy; }
            set
            {
                if (activityIsBusy != value)
                {
                    activityIsBusy = value;
                    OnPropertyChanged(nameof(ActivityIsBusy));
                }
            }
        }
        private bool refreshIsBusy;
        public bool RefreshIsBusy
        {
            get { return refreshIsBusy; }
            set
            {
                if (refreshIsBusy != value)
                {
                    refreshIsBusy = value;
                    OnPropertyChanged(nameof(RefreshIsBusy));
                }
            }
        }

        private ObservableCollection<DailyItems> dailyItemList;
        public ObservableCollection<DailyItems> DailyItemList
        {
            get { return dailyItemList; }
            set
            {
                if (dailyItemList != value)
                {
                    dailyItemList = value;
                    OnPropertyChanged(nameof(DailyItemList));
                   
                }
            }
        }
        private ObservableCollection<HourlyItems> hourlyItemList;
        public ObservableCollection<HourlyItems> HourlyItemList
        {
            get { return hourlyItemList; }
            set
            {
                if (hourlyItemList != value)
                {
                    hourlyItemList = value;
                    OnPropertyChanged(nameof(HourlyItemList));

                }
            }
        }

        public string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "unit.txt");

        public AboutViewModel()
        {
            if(!File.Exists(filename))
            {
                File.WriteAllText(filename, "metric");
            }
            Unit = File.ReadAllText(filename);

            CityName = "New York";
            Task.Run(() => GetCurrentWeather(true, CityName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string GetHunWeatherMain(string word)
        {
            Dictionary<string, string> weatherDic = new Dictionary<string, string>()
        {
            {"Rain", "Eső"},
            {"Thuderstorm", "Vihar"},
            {"Drizzle", "Szitálás"},
            {"Snow", "Havazás"},
            {"Clear", "Tiszta" },
            {"Mist", "Gyenge köd" },
            {"Smoke", "Szmog" },
            {"Haze", "Gyenge szmog" },
            {"Clouds", "Felhős" },
            {"Dust", "Poros" },
            {"Fog", "Köd" },
            {"Ash", "Vulkáni Hamu" },
            {"Squall", "Zivatar" },
            {"Tornado", "Tornádó" }
        };
            if (weatherDic[word] != null)
                return weatherDic[word];

            return word;
        }

        public async Task GetCurrentWeather(bool isTest, string searchValue = "", bool refresh = true)
        {
            // Section1.IsVisible = false;
            // Section2.IsVisible = false;
            // Section3.IsVisible = false;
            // Section4.IsVisible = false;
            if (Unit == "metric") UnitName = "Celsius"; else UnitName = "Fahrenheit";

                if (searchValue != "" || isTest)
                {
                    ActivityIsBusy = true;
                }
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromMinutes(1)));

                double testLong = -73.9;
                double testLat = 40.7;

                double realLat;
                double realLong;

                if (!isTest)
                {
                    realLat = location.Latitude;
                    realLong = location.Longitude;
                }
                else
                {
                    realLat = testLat;
                    realLong = testLong;
                }
                //  realLat = Math.Round(realLat, 2);
                // realLong = Math.Round(realLong, 2);

                if (location != null && searchValue == "")
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(realLat, realLong);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        if (placemark.AdminArea != null)
                            CityName = placemark.AdminArea;
                        else
                            CityName = placemark.Locality;
                        CountryName = placemark.CountryName;
                    }
                }

                using (var client = new HttpClient())
                {
                    if (searchValue != "" && refresh)
                    {
                        var searchAPI = $"https://api.openweathermap.org/geo/1.0/direct?q={searchValue}&limit=5&appid={SecretKeys.API_KEY}";
                        var res = await client.GetStringAsync(searchAPI);
                        List<City> cityValues = JsonConvert.DeserializeObject<List<City>>(res);

                        realLat = cityValues[0].lat;
                        realLong = cityValues[0].lon;

                        var placemarks = await Geocoding.GetPlacemarksAsync(realLat, realLong);
                        var placemark = placemarks?.FirstOrDefault();
                        if (placemark != null)
                        {
                            if (placemark.AdminArea != null)
                                CityName = placemark.AdminArea;
                            else
                                CityName = placemark.Locality;
                            CountryName = placemark.CountryName;
                        }
                    }

                    var uri = $"https://api.openweathermap.org/data/3.0/onecall?lat={realLat}&lon={realLong}&units={Unit}&lang=hu&appid={SecretKeys.API_KEY}";
                    var result = await client.GetStringAsync(uri);

                    var currentWeather = JsonConvert.DeserializeObject<Weather>(result);

                    var hourlyCast = currentWeather.hourly;
                    var dailyCast = currentWeather.daily;
                    var alerts = currentWeather.alerts;


                    // LocalNotificationCenter.Current.CancelAll();
                    if (alerts != null)
                    {
                        foreach (var alert in alerts)
                        {
                            DateTime alertTime = DateTimeOffset.FromUnixTimeSeconds(alert.start).DateTime;
                            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentWeather.timezone);
                            DateTime localAlertTime = TimeZoneInfo.ConvertTimeFromUtc(alertTime, targetTimeZone);
                            var notification = new NotificationRequest
                            {
                                BadgeNumber = 1,
                                Description = alert.description,
                                Title = alert.@event,
                                Schedule = {
                                NotifyTime = localAlertTime,
                                }
                            };
                            // LocalNotificationCenter.Current.Show(notification);
                        }
                    }
                    TempValue = Math.Round(currentWeather.current.temp).ToString();
                    string mainText = GetHunWeatherMain(currentWeather.current.weather[0].main);
                    MainCondition = mainText;
                    Description = currentWeather.current.weather[0].description;
                    string iconId = currentWeather.current.weather[0].icon;

                    Humidity = $"{currentWeather.current.humidity}%";
                    Visibility = $"{currentWeather.current.visibility.ToString("N0")}m";
                    FeelsLike = $"{currentWeather.current.feels_like}°";
                    WindSpeed = $"{currentWeather.current.wind_speed}m/s";

                    var imageSource = new UriImageSource { Uri = new Uri($"https://openweathermap.org/img/wn/{iconId}@4x.png") };
                    imageSource.CachingEnabled = false;
                    imageSource.CacheValidity = TimeSpan.FromHours(1);
                    ImgSource = imageSource;
                   
                    int hourlyCastCount = hourlyCast.Length;
                    int dailyCastCount = dailyCast.Length;
                    int i = 0;
                    int j = 0;
                    DailyItemList = new ObservableCollection<DailyItems>();
                    HourlyItemList = new ObservableCollection<HourlyItems>();

                    if (HourlyItemList.Count == 0)
                    {
                        while (i < hourlyCastCount)
                        {
                            string elementIconId = hourlyCast[i].weather[0].icon;
                            var elementImageSource = new UriImageSource { Uri = new Uri($"https://openweathermap.org/img/wn/{elementIconId}@4x.png") };
                            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(hourlyCast[i].dt).DateTime;
                            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentWeather.timezone);
                            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, targetTimeZone);
                            string hour;
                            if (i == 0)
                                hour = "Most";
                            else
                                hour = localDateTime.ToString("HH");

                            HourlyItems item = new HourlyItems
                            {
                                hour = hour,
                                main = GetHunWeatherMain(hourlyCast[i].weather[0].main),
                                imgSource = elementImageSource,
                                temp = $"{Math.Round(hourlyCast[i].temp)}°",
                            };

                            HourlyItemList.Add(item);
                            i++;
                        }
                    }
                


                if (DailyItemList.Count == 0) {
                        while (j < dailyCastCount)
                        {
                            string elementIconId = dailyCast[j].weather[0].icon;
                            var elementImageSource = new UriImageSource { Uri = new Uri($"https://openweathermap.org/img/wn/{elementIconId}@4x.png") };
                            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(dailyCast[j].dt).DateTime;
                            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentWeather.timezone);
                            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, targetTimeZone);
                            string day;
                            if (j == 0)
                                day = "Ma";
                            else
                                day = localDateTime.ToString("dddd").ToUpper();

                            DailyItems item = new DailyItems
                            {
                                day = day,
                                imgSource = elementImageSource,
                                temp = $"{Math.Round(dailyCast[j].temp.min)}° - {Math.Round(dailyCast[j].temp.max)}°",
                            };

                            DailyItemList.Add(item);
                            j++;
                        }
                        }
                    }
                
           // Section1.IsVisible = true;
           // Section2.IsVisible = true;
           // Section3.IsVisible = true;
           // Section4.IsVisible = true;
           if (ActivityIsBusy || RefreshIsBusy)
            {
                ActivityIsBusy = false;
                RefreshIsBusy = false;
            }
        }
        private void OnMyItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DailyItems.PropertyChanged))
            {
                OnPropertyChanged(nameof(DailyItemList));
            }
        }
    }
}