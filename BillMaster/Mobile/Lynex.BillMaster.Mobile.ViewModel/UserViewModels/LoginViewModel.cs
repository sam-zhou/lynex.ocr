using Xamarin.Forms;

namespace Lynex.BillMaster.Mobile.ViewModel.UserViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsBusy => !IsLoginButtonEnabled;

        public bool IsLoginButtonEnabled { get; set; }

        public Command LoginClicked { get; set; }
    }
}