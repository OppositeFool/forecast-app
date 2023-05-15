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
using System.Collections.Generic;
using System.IO;
using System.Collections;
using Plugin.LocalNotification;

namespace Forecast_App.Views
{
    public partial class AboutPage : ContentPage
    {
        AboutViewModel viewModel = new AboutViewModel();
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
            Section1.IsVisible = true;
            Section2.IsVisible = true;
           // Section3.IsVisible = true;

          
            //Section4.IsVisible = true;
          //  BindableLayout.SetItemsSource(hourlyContainer, viewModel.HourlyContainer);
        }
        async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await viewModel.GetCurrentWeather(false);
        }
        async void Entry_Completed(object sender, EventArgs e)
        {
            var text = ((Entry)sender).Text; //cast sender to access the properties of the Entry

            await viewModel.GetCurrentWeather(false, text);
        }
        async void ChangeUnit(object sender, EventArgs args)
        {
            if (viewModel.Unit == "metric")
            {
                viewModel.Unit = "imperial";
            }
            else
            {
                viewModel.Unit = "metric";
            }
            File.WriteAllText(viewModel.filename, viewModel.Unit);
            await viewModel.GetCurrentWeather(false, viewModel.CityName);
        }
    }
}