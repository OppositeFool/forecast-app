using Forecast_App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forecast_App.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}