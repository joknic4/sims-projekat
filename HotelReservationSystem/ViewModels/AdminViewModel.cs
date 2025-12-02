using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HotelReservationSystem.Helpers;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        private ObservableCollection<Korisnik> vlasnike = new();
        private ObservableCollection<Hotel> hoteli = new();
        
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
        
        private string statusMessage = string.Empty;

        public ObservableCollection<Korisnik> Vlasnike
        {
            get => vlasnike;
            set => SetProperty(ref vlasnike, value);
        }

        public ObservableCollection<Hotel> Hoteli
        {
            get => hoteli;
            set => SetProperty(ref hoteli, value);
        }

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

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public ICommand RegisterVlasnikCommand { get; }
        public ICommand CreateHotelCommand { get; }
        public ICommand GenerateHotelCodeCommand { get; }
        public ICommand LoadVlasnikeCommand { get; }
        public ICommand LoadHoteliCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminViewModel()
        {
            RegisterVlasnikCommand = new RelayCommand(RegisterVlasnik);
            CreateHotelCommand = new RelayCommand(_ => CreateHotel());
            GenerateHotelCodeCommand = new RelayCommand(_ => GenerateHotelCode());
            LoadVlasnikeCommand = new RelayCommand(_ => LoadVlasnike());
            LoadHoteliCommand = new RelayCommand(_ => LoadHoteli());
            LogoutCommand = new RelayCommand(_ => Logout());

            LoadVlasnike();
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
                    LoadVlasnike();
                }
                else
                {
                    MessageBox.Show("Registracija nije uspela. Možda već postoji vlasnik sa ovim JMBG-om ili email-om.", 
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
                    MessageBox.Show("Hotel je kreiran i čeka odobrenje vlasnika", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearHotelForm();
                    LoadHoteli();
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

        private void LoadVlasnike()
        {
            try
            {
                var korisnikService = ServiceLocator.Instance.KorisnikService;
                var allVlasnike = korisnikService.GetVlasnike();
                
                Vlasnike.Clear();
                foreach (var vlasnik in allVlasnike)
                {
                    Vlasnike.Add(vlasnik);
                }
                
                StatusMessage = $"Učitano {Vlasnike.Count} vlasnika";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
        }

        private void LoadHoteli()
        {
            try
            {
                var hotelService = ServiceLocator.Instance.HotelService;
                var allHotels = hotelService.GetAllHotels();
                
                Hoteli.Clear();
                foreach (var hotel in allHotels)
                {
                    Hoteli.Add(hotel);
                }
                
                StatusMessage = $"Učitano {Hoteli.Count} hotela";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
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
