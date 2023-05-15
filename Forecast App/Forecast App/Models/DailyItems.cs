using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Forecast_App.Models
{
    public class DailyItems : INotifyPropertyChanged
    {
        public string day { get; set; }
        public string temp { get; set; }
        public ImageSource imgSource { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
