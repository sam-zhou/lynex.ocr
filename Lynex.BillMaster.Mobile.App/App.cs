using System.Net.Http;
using System.Text;
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
            using (var client = new HttpClient())
            {
                var response = client.PostAsync("http://api.mylynex.com.au/account/register",

                        // Pass in an anonymous object that maps to the expected 
                        // RegisterUserBindingModel defined as the method parameter 
                        // for the Register method on the API:
                        new StringContent((new
                        {
                            Email = "samzhou.it@gmail.com",
                            Password = "Jukfrg!1",
                            ConfirmPassword = "Jukfrg!1",
                        }).ToString(), Encoding.UTF8, "application/json")).Result;

                if (!response.IsSuccessStatusCode)
                {
                    // Unwrap the response and throw as an Api Exception:
                    //var ex = ApiException.CreateApiException(response);
                    //throw ex;
                    Xamarin.Forms.MessagingCenter.Send(this, "Failed");
                }

            }
            Xamarin.Forms.MessagingCenter.Send(this, "Successed");
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
