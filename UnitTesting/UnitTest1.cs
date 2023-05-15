using Forecast_App.ViewModels;

namespace UnitTesting
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            AboutViewModel viewModel = new AboutViewModel();

            viewModel.CityName = "Debrecen";

            viewModel.GetCurrentWeather(false);

            Assert.Greater(viewModel.DailyItemList.Count, 0);
        }
    }
}