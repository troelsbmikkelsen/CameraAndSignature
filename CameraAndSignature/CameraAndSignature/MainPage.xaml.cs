using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CameraAndSignature
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            CameraPageButton.Clicked += CameraPageButton_Clicked;
            SignaturePageButton.Clicked += SignaturePageButton_Clicked;
        }

        private async void CameraPageButton_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new Pages.CameraPage());
        }

        private async void SignaturePageButton_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new Pages.SignaturePage());
        }
    }
}
