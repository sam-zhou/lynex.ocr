using Lynex.BillMaster.Mobile.View;
using Lynex.BillMaster.Mobile.ViewModel;
using Xamarin.Forms;

namespace Lynex.BillMaster.Mobile.Portable.App
{
    public class App : Application
    {
        public App()
        {


            // The root page of your application
            MainPage = new HomePage
            {
                //Content = new StackLayout
                //{
                //    VerticalOptions = LayoutOptions.Center,
                //    Children = {
                //        new Label {
                //            XAlign = TextAlignment.Center,
                //            Text = "Welcome to Lynex BillMaster"
                //        },
                //        new Label {
                //            XAlign = TextAlignment.Center,
                //            Text = "Author: Sam Zhou"
                //        }
                //    }
                //}
            };

            var loginViewModel = new LoginViewModel
            {
                IsLoginButtonEnabled = true,
            };

            loginViewModel.LoginClicked = new Command(LoginClicked);
            MainPage.BindingContext = loginViewModel;
        }

        private void LoginClicked(object obj)
        {
            Xamarin.Forms.MessagingCenter.Send(this, "Hahahah");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
