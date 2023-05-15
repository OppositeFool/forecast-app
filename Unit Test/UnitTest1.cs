using Forecast_App.ViewModels;
using Forecast_App.Views;
using NUnit.Framework;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Unit_Test
{
    public class Tests
    {
        AboutViewModel viewModel = new AboutViewModel();
        [SetUp]
        public void Setup()
        {
            Task.Run(() => viewModel.GetCurrentWeather(true));
        }

        [Test]
        public void TestUnitIsNull()
        {

            Assert.NotNull(viewModel.Unit);
        }
        [Test]
        public void TestUnitBeExactly()
        {
            Assert.IsTrue(viewModel.Unit == "metric" || viewModel.Unit == "imperial");
        }
        [Test]
        public void TestInitCityName()
        {
            Assert.IsTrue(viewModel.CityName == "New York");
        }
    }
}