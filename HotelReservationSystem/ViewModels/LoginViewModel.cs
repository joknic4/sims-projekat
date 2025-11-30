using System;
using System.Windows;
using System.Windows.Input;
using HotelReservationSystem.Helpers;

namespace HotelReservationSystem.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string email = string.Empty;
        private string errorMessage = string.Empty;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object? parameter)
        {
            try
            {
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Email))
                {
                    ErrorMessage = "Unesite email adresu";
                    return;
                }

                var passwordBox = parameter as System.Windows.Controls.PasswordBox;
                if (passwordBox == null || string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    ErrorMessage = "Unesite lozinku";
                    return;
                }

                var authService = ServiceLocator.Instance.AuthService;
                var korisnik = authService.Login(Email, passwordBox.Password);

                if (korisnik == null)
                {
                    ErrorMessage = "Neispravni podaci za prijavu";
                    return;
                }

                MessageBox.Show($"Uspešna prijava! Dobrodošli, {korisnik.GetIme()} {korisnik.GetPrezime()}");
                // TODO: Otvaranje glavnog prozora prema tipu korisnika
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Greška: {ex.Message}";
            }
        }
    }
}
