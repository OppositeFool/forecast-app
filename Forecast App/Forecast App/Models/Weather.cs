using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Forecast_App.Models
{
    public class Weather
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }

        public current current { get; set; }

        public hourly[] hourly { get; set; }
        public daily[] daily { get; set; }
        public alerts[] alerts { get; set; }
    }

    public class alerts
    {
        public string @event { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public string description { get; set; }
    }
    public class City
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class current
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double wind_speed { get; set; }
        public double humidity { get; set; }
        public double visibility { get; set; }
        public weather[] weather { get; set; }
    }

    public class hourly
    {
        public double temp { get; set; }
        public weather[] weather { get; set; }
        public long dt { get; set; }
    }
    public class daily
    {

        public weather[] weather { get; set; }
        public temp temp { get; set; }
        public long dt { get; set; }
    }
    public class weather
    {
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
    public class temp
    {
        public double min { get; set; }
        public double max { get; set; }
    }
}
