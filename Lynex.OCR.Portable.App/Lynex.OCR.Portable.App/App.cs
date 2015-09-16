using Lynex.BillMaster.Mobile.View;
using Lynex.BillMaster.Mobile.View.UserViews;
using Lynex.BillMaster.Mobile.ViewModel;
using Lynex.BillMaster.Mobile.ViewModel.UserViewModels;
using Xamarin.Forms;

namespace Lynex.BillMaster.Mobile.Portable.App
{
    public class App : Application
    {
        public App()
        {


            // The root page of your application
            MainPage = new RegistrationView();

            var loginViewModel = new RegistrationViewModel
            {
                IsRegisterButtonEnabled = true,
            };

            loginViewModel.RegisterClicked = new Command(LoginClicked);
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
