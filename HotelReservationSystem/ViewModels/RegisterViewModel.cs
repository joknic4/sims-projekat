using System;
using System.Windows;
using System.Windows.Input;
using HotelReservationSystem.Helpers;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string jmbg = string.Empty;
        private string email = string.Empty;
        private string ime = string.Empty;
        private string prezime = string.Empty;
        private string mobilniTelefon = string.Empty;
        private string errorMessage = string.Empty;

        public string Jmbg
        {
            get => jmbg;
            set => SetProperty(ref jmbg, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Ime
        {
            get => ime;
            set => SetProperty(ref ime, value);
        }

        public string Prezime
        {
            get => prezime;
            set => SetProperty(ref prezime, value);
        }

        public string MobilniTelefon
        {
            get => mobilniTelefon;
            set => SetProperty(ref mobilniTelefon, value);
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Register(object? parameter)
        {
            try
            {
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Jmbg))
                {
                    ErrorMessage = "Unesite JMBG";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Email))
                {
                    ErrorMessage = "Unesite email";
                    return;
                }

                var passwordBox = parameter as System.Windows.Controls.PasswordBox;
                if (passwordBox == null || string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    ErrorMessage = "Unesite lozinku";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Ime) || string.IsNullOrWhiteSpace(Prezime))
                {
                    ErrorMessage = "Unesite ime i prezime";
                    return;
                }

                var korisnikService = ServiceLocator.Instance.KorisnikService;
                var authService = ServiceLocator.Instance.AuthService;

                var noviKorisnik = new Korisnik(Jmbg, Email, passwordBox.Password, 
                                               Ime, Prezime, MobilniTelefon, KorisnikTip.Gost);

                if (authService.RegisterGost(noviKorisnik))
                {
                    MessageBox.Show("Uspešno ste se registrovali! Možete se prijaviti.", "Registracija", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    Application.Current.Windows[1]?.Close();
                }
                else
                {
                    ErrorMessage = "Registracija nije uspela.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Greška: {ex.Message}";
            }
        }

        private void Cancel(object? parameter)
        {
            Application.Current.Windows[1]?.Close();
        }
    }
}
