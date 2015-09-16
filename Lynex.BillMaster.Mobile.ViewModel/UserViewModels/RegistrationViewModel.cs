using Xamarin.Forms;

namespace Lynex.BillMaster.Mobile.ViewModel.UserViewModels
{
    public class RegistrationViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Country { get; set; }

        public string Mobile { get; set; }

        public bool AcceptTermsAndConditions { get; set; }

        public bool IsRegisterButtonEnabled { get; set; }

        public Command RegisterClicked { get; set; }
    }
}