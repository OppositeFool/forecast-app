using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Forecast_App.Models
{
    public class Weather
    {
        public double lat { get; set; }

        public current current { get; set; }
    }

    public class current
    {
        public double temp { get; set; }
    }
}
