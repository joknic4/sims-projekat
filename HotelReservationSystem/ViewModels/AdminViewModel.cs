using System;
using System.Windows;
using System.Windows.Input;
using HotelReservationSystem.Helpers;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        private string vlasnikJmbg = string.Empty;
        private string vlasnikEmail = string.Empty;
        private string vlasnikIme = string.Empty;
        private string vlasnikPrezime = string.Empty;
        private string vlasnikTelefon = string.Empty;
        
        private string hotelSifra = string.Empty;
        private string hotelIme = string.Empty;
        private int hotelGodina = DateTime.Now.Year;
        private int hotelZvezdice = 3;
        private string hotelVlasnikJmbg = string.Empty;

        public string VlasnikJmbg
        {
            get => vlasnikJmbg;
            set => SetProperty(ref vlasnikJmbg, value);
        }

        public string VlasnikEmail
        {
            get => vlasnikEmail;
            set => SetProperty(ref vlasnikEmail, value);
        }

        public string VlasnikIme
        {
            get => vlasnikIme;
            set => SetProperty(ref vlasnikIme, value);
        }

        public string VlasnikPrezime
        {
            get => vlasnikPrezime;
            set => SetProperty(ref vlasnikPrezime, value);
        }

        public string VlasnikTelefon
        {
            get => vlasnikTelefon;
            set => SetProperty(ref vlasnikTelefon, value);
        }

        public string HotelSifra
        {
            get => hotelSifra;
            set => SetProperty(ref hotelSifra, value);
        }

        public string HotelIme
        {
            get => hotelIme;
            set => SetProperty(ref hotelIme, value);
        }

        public int HotelGodina
        {
            get => hotelGodina;
            set => SetProperty(ref hotelGodina, value);
        }

        public int HotelZvezdice
        {
            get => hotelZvezdice;
            set => SetProperty(ref hotelZvezdice, value);
        }

        public string HotelVlasnikJmbg
        {
            get => hotelVlasnikJmbg;
            set => SetProperty(ref hotelVlasnikJmbg, value);
        }

        public ICommand RegisterVlasnikCommand { get; }
        public ICommand CreateHotelCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminViewModel()
        {
            RegisterVlasnikCommand = new RelayCommand(RegisterVlasnik);
            CreateHotelCommand = new RelayCommand(_ => CreateHotel());
            LogoutCommand = new RelayCommand(_ => Logout());

            GenerateHotelCode();
        }

        private void RegisterVlasnik(object? parameter)
        {
            try
            {
                var passwordBox = parameter as System.Windows.Controls.PasswordBox;
                if (passwordBox == null || string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    MessageBox.Show("Unesite lozinku", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var korisnikService = ServiceLocator.Instance.KorisnikService;
                
                var noviVlasnik = new Korisnik(
                    VlasnikJmbg,
                    VlasnikEmail,
                    passwordBox.Password,
                    VlasnikIme,
                    VlasnikPrezime,
                    VlasnikTelefon,
                    KorisnikTip.Vlasnik
                );

                if (korisnikService.AddKorisnik(noviVlasnik))
                {
                    MessageBox.Show("Vlasnik je uspešno registrovan", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearVlasnikForm();
                    passwordBox.Clear();
                }
                else
                {
                    MessageBox.Show("Registracija nije uspela.", 
                                  "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateHotel()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(HotelSifra) || string.IsNullOrWhiteSpace(HotelIme))
                {
                    MessageBox.Show("Popunite sva polja", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var hotelService = ServiceLocator.Instance.HotelService;
                
                var noviHotel = new Hotel(
                    HotelSifra,
                    HotelIme,
                    HotelGodina,
                    HotelZvezdice,
                    HotelVlasnikJmbg,
                    StatusHotela.NaCekanju
                );

                if (hotelService.AddHotel(noviHotel))
                {
                    MessageBox.Show("Hotel je kreiran", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearHotelForm();
                    GenerateHotelCode();
                }
                else
                {
                    MessageBox.Show("Kreiranje hotela nije uspelo", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateHotelCode()
        {
            var hotelService = ServiceLocator.Instance.HotelService;
            HotelSifra = hotelService.GenerateHotelCode();
        }

        private void ClearVlasnikForm()
        {
            VlasnikJmbg = string.Empty;
            VlasnikEmail = string.Empty;
            VlasnikIme = string.Empty;
            VlasnikPrezime = string.Empty;
            VlasnikTelefon = string.Empty;
        }

        private void ClearHotelForm()
        {
            HotelIme = string.Empty;
            HotelGodina = DateTime.Now.Year;
            HotelZvezdice = 3;
            HotelVlasnikJmbg = string.Empty;
        }

        private void Logout()
        {
            ServiceLocator.Instance.AuthService.Logout();
            var loginWindow = new Views.LoginWindow();
            loginWindow.Show();
            Application.Current.Windows[0]?.Close();
        }
    }
}
