using System;
using System.Windows;
using System.Windows.Input;
using HotelReservationSystem.Helpers;
using HotelReservationSystem.Views;
using HotelReservationSystem.Models;

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
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
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

                if (korisnik.GetTipKorisnika() == KorisnikTip.Administrator)
                {
                    var adminWindow = new AdminWindow();
                    adminWindow.Show();
                    Application.Current.Windows[0]?.Close();
                }
                else
                {
                    MessageBox.Show($"Uspešna prijava! Dobrodošli, {korisnik.GetIme()} {korisnik.GetPrezime()}");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Greška: {ex.Message}";
            }
        }

        private void Register(object? parameter)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}
