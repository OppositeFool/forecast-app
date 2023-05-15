using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Forecast_App.Models
{
    public class HourlyItems : INotifyPropertyChanged
    {
        public string hour { get; set; }
        public string temp { get; set; }
        public string main { get; set; }
        public ImageSource imgSource { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
