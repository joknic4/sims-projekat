using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HotelReservationSystem.Helpers;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels
{
    public class VlasnikViewModel : ViewModelBase
    {
        private ObservableCollection<Hotel> mojiHoteli = new();
        private ObservableCollection<Apartman> apartmani = new();
        private ObservableCollection<Rezervacija> rezervacije = new();
        private Hotel? selectedHotel;
        private Rezervacija? selectedRezervacija;
        
        private string apartmanIme = string.Empty;
        private string apartmanOpis = string.Empty;
        private int apartmanBrojSoba = 1;
        private int apartmanMaxGostiju = 2;
        
        private string statusMessage = string.Empty;

        public ObservableCollection<Hotel> MojiHoteli
        {
            get => mojiHoteli;
            set => SetProperty(ref mojiHoteli, value);
        }

        public ObservableCollection<Apartman> Apartmani
        {
            get => apartmani;
            set => SetProperty(ref apartmani, value);
        }

        public ObservableCollection<Rezervacija> Rezervacije
        {
            get => rezervacije;
            set => SetProperty(ref rezervacije, value);
        }

        public Hotel? SelectedHotel
        {
            get => selectedHotel;
            set
            {
                SetProperty(ref selectedHotel, value);
            }
        }

        public Rezervacija? SelectedRezervacija
        {
            get => selectedRezervacija;
            set => SetProperty(ref selectedRezervacija, value);
        }

        public string ApartmanIme
        {
            get => apartmanIme;
            set => SetProperty(ref apartmanIme, value);
        }

        public string ApartmanOpis
        {
            get => apartmanOpis;
            set => SetProperty(ref apartmanOpis, value);
        }

        public int ApartmanBrojSoba
        {
            get => apartmanBrojSoba;
            set => SetProperty(ref apartmanBrojSoba, value);
        }

        public int ApartmanMaxGostiju
        {
            get => apartmanMaxGostiju;
            set => SetProperty(ref apartmanMaxGostiju, value);
        }

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public ICommand LoadHoteliCommand { get; }
        public ICommand ApproveHotelCommand { get; }
        public ICommand RejectHotelCommand { get; }
        public ICommand CreateApartmanCommand { get; }
        public ICommand LoadRezervacijeCommand { get; }
        public ICommand ApproveRezervacijaCommand { get; }
        public ICommand RejectRezervacijaCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand LoadApartmaniCommand { get; }

        public VlasnikViewModel()
        {
            LoadHoteliCommand = new RelayCommand(_ => LoadHoteli());
            ApproveHotelCommand = new RelayCommand(_ => ApproveHotel());
            RejectHotelCommand = new RelayCommand(_ => RejectHotel());
            CreateApartmanCommand = new RelayCommand(_ => CreateApartman());
            LoadRezervacijeCommand = new RelayCommand(_ => LoadRezervacije());
            ApproveRezervacijaCommand = new RelayCommand(ApproveRezervacija);
            RejectRezervacijaCommand = new RelayCommand(RejectRezervacija);
            LogoutCommand = new RelayCommand(_ => Logout());
            LoadApartmaniCommand = new RelayCommand(_ => LoadApartmaniManual());

            LoadHoteli();
            LoadRezervacije();
        }

        private void LoadHoteli()
        {
            try
            {
                var currentUser = ServiceLocator.Instance.AuthService.GetCurrentUser();
                if (currentUser == null)
                    return;

                var hotelService = ServiceLocator.Instance.HotelService;
                var hotels = hotelService.GetHotelsByVlasnik(currentUser.GetJmbg());
                
                MojiHoteli.Clear();
                int counter = 0;
                foreach (var hotel in hotels)
                {
                    MojiHoteli.Add(hotel);
                    counter++;
                }
                
                StatusMessage = "Učitano hotela: " + counter.ToString();
            }
            catch (Exception ex)
            {
                StatusMessage = "Greška: " + ex.Message;
            }
        }

        private void ApproveHotel()
        {
            try
            {
                if (SelectedHotel == null)
                {
                    MessageBox.Show("Izaberite hotel", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var hotelService = ServiceLocator.Instance.HotelService;
                bool rezultat = hotelService.ApproveHotel(SelectedHotel.GetSifra());
                
                if (rezultat == true)
                {
                    MessageBox.Show("Hotel je odobren i sada je vidljiv u sistemu", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadHoteli();
                }
                else
                {
                    MessageBox.Show("Nije moguće odobriti hotel", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RejectHotel()
        {
            try
            {
                if (SelectedHotel == null)
                {
                    MessageBox.Show("Izaberite hotel", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                var result = MessageBox.Show("Da li ste sigurni da želite da odbijete ovaj hotel?", 
                                            "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    var hotelService = ServiceLocator.Instance.HotelService;
                    bool success = hotelService.RejectHotel(SelectedHotel.GetSifra());
                    
                    if (success)
                    {
                        MessageBox.Show("Hotel je odbijen", "Uspeh", 
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadHoteli();
                    }
                    else
                    {
                        MessageBox.Show("Nije moguće odbiti hotel", "Greška", 
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Greška: " + ex.Message;
            }
        }

        private void LoadApartmaniManual()
        {
            try
            {
                if (SelectedHotel == null)
                {
                    MessageBox.Show("Prvo izaberite hotel", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                var apartmanService = ServiceLocator.Instance.ApartmanService;
                var allApartmani = apartmanService.GetApartmaniByHotel(SelectedHotel.GetSifra());
                
                Apartmani.Clear();
                int count = 0;
                foreach (var apartman in allApartmani)
                {
                    Apartmani.Add(apartman);
                    count = count + 1;
                }
                
                StatusMessage = "Učitano apartmana: " + count;
            }
            catch (Exception ex)
            {
                StatusMessage = "Greška: " + ex.Message;
            }
        }

        private void CreateApartman()
        {
            try
            {
                if (SelectedHotel == null)
                {
                    MessageBox.Show("Prvo izaberite hotel", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(ApartmanIme))
                {
                    MessageBox.Show("Unesite ime apartmana", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (ApartmanBrojSoba <= 0)
                {
                    MessageBox.Show("Broj soba mora biti veći od 0", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var apartmanService = ServiceLocator.Instance.ApartmanService;
                
                string apartmanId = apartmanService.GenerateApartmanId();
                var noviApartman = new Apartman(
                    apartmanId,
                    ApartmanIme,
                    ApartmanOpis,
                    ApartmanBrojSoba,
                    ApartmanMaxGostiju,
                    SelectedHotel.GetSifra()
                );

                bool success = apartmanService.AddApartman(noviApartman);
                
                if (success == true)
                {
                    MessageBox.Show("Apartman je uspešno kreiran", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    LoadApartmaniManual();
                }
                else
                {
                    MessageBox.Show("Kreiranje apartmana nije uspelo", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadRezervacije()
        {
            try
            {
                var currentUser = ServiceLocator.Instance.AuthService.GetCurrentUser();
                if (currentUser == null)
                    return;

                var rezervacijaService = ServiceLocator.Instance.RezervacijaService;
                var allRezervacije = rezervacijaService.GetRezervacijeForVlasnik(currentUser.GetJmbg());

                Rezervacije.Clear();
                int total = 0;
                foreach (var rez in allRezervacije)
                {
                    Rezervacije.Add(rez);
                    total++;
                }
                
                StatusMessage = "Učitano rezervacija: " + total;
            }
            catch (Exception ex)
            {
                StatusMessage = "Greška: " + ex.Message;
            }
        }

        private void ApproveRezervacija(object? parameter)
        {
            try
            {
                if (SelectedRezervacija == null)
                {
                    MessageBox.Show("Izaberite rezervaciju", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var rezervacijaService = ServiceLocator.Instance.RezervacijaService;
                bool result = rezervacijaService.ApproveRezervacija(SelectedRezervacija.GetId());
                
                if (result)
                {
                    MessageBox.Show("Rezervacija je potvrđena", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadRezervacije();
                }
                else
                {
                    MessageBox.Show("Nije moguće potvrditi rezervaciju. Možda je apartman već zauzet.", 
                                  "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Greška: " + ex.Message;
            }
        }

        private void RejectRezervacija(object? parameter)
        {
            try
            {
                if (SelectedRezervacija == null)
                {
                    MessageBox.Show("Izaberite rezervaciju", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                var razlog = Microsoft.VisualBasic.Interaction.InputBox(
                    "Unesite razlog odbijanja rezervacije:",
                    "Razlog odbijanja",
                    "",
                    -1, -1);

                if (string.IsNullOrWhiteSpace(razlog))
                {
                    MessageBox.Show("Morate uneti razlog odbijanja", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var rezervacijaService = ServiceLocator.Instance.RezervacijaService;
                bool success = rezervacijaService.RejectRezervacija(SelectedRezervacija.GetId(), razlog);
                
                if (success == true)
                {
                    MessageBox.Show("Rezervacija je odbijena", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadRezervacije();
                }
                else
                {
                    MessageBox.Show("Nije moguće odbiti rezervaciju", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Greška: " + ex.Message;
            }
        }

        private void Logout()
        {
            ServiceLocator.Instance.AuthService.Logout();
            var loginWindow = new Views.LoginWindow();
            loginWindow.Show();
            
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Views.VlasnikWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
