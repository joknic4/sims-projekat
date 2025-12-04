using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string statusFilter = "Sve";

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
                if (SetProperty(ref selectedHotel, value) && value != null)
                {
                    LoadApartmani(value.GetSifra());
                }
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

        public string StatusFilter
        {
            get => statusFilter;
            set
            {
                if (SetProperty(ref statusFilter, value))
                {
                    LoadRezervacije();
                }
            }
        }

        public ICommand LoadHoteliCommand { get; }
        public ICommand ApproveHotelCommand { get; }
        public ICommand RejectHotelCommand { get; }
        public ICommand CreateApartmanCommand { get; }
        public ICommand LoadRezervacijeCommand { get; }
        public ICommand ApproveRezervacijaCommand { get; }
        public ICommand RejectRezervacijaCommand { get; }
        public ICommand LogoutCommand { get; }

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
                foreach (var hotel in hotels)
                {
                    MojiHoteli.Add(hotel);
                }
                
                StatusMessage = $"Učitano {MojiHoteli.Count} hotela";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
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

                if (SelectedHotel.GetStatus() != StatusHotela.NaCekanju)
                {
                    MessageBox.Show("Možete odobriti samo hotele koji su na čekanju", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var hotelService = ServiceLocator.Instance.HotelService;
                if (hotelService.ApproveHotel(SelectedHotel.GetSifra()))
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
                StatusMessage = $"Greška: {ex.Message}";
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

                if (SelectedHotel.GetStatus() != StatusHotela.NaCekanju)
                {
                    MessageBox.Show("Možete odbiti samo hotele koji su na čekanju", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show("Da li ste sigurni da želite da odbijete ovaj hotel?", 
                                            "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    var hotelService = ServiceLocator.Instance.HotelService;
                    if (hotelService.RejectHotel(SelectedHotel.GetSifra()))
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
                StatusMessage = $"Greška: {ex.Message}";
            }
        }

        private void LoadApartmani(string sifraHotela)
        {
            try
            {
                var apartmanService = ServiceLocator.Instance.ApartmanService;
                var allApartmani = apartmanService.GetApartmaniByHotel(sifraHotela);
                
                Apartmani.Clear();
                foreach (var apartman in allApartmani)
                {
                    Apartmani.Add(apartman);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
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

                if (SelectedHotel.GetStatus() != StatusHotela.Odobren)
                {
                    MessageBox.Show("Možete dodavati apartmane samo u odobrene hotele", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ApartmanIme))
                {
                    MessageBox.Show("Unesite ime apartmana", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var apartmanService = ServiceLocator.Instance.ApartmanService;
                
                var noviApartman = new Apartman(
                    apartmanService.GenerateApartmanId(),
                    ApartmanIme,
                    ApartmanOpis,
                    ApartmanBrojSoba,
                    ApartmanMaxGostiju,
                    SelectedHotel.GetSifra()
                );

                if (apartmanService.AddApartman(noviApartman))
                {
                    MessageBox.Show("Apartman je uspešno kreiran", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearApartmanForm();
                    LoadApartmani(SelectedHotel.GetSifra());
                }
                else
                {
                    MessageBox.Show("Kreiranje apartmana nije uspelo", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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

                var filtered = StatusFilter switch
                {
                    "Na čekanju" => allRezervacije.Where(r => r.GetStatus() == StatusRezervacije.NaCekanju).ToList(),
                    "Potvrđeno" => allRezervacije.Where(r => r.GetStatus() == StatusRezervacije.Potvrdjeno).ToList(),
                    _ => allRezervacije
                };

                Rezervacije.Clear();
                foreach (var rez in filtered)
                {
                    Rezervacije.Add(rez);
                }
                
                StatusMessage = $"Učitano {Rezervacije.Count} rezervacija";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
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
                if (rezervacijaService.ApproveRezervacija(SelectedRezervacija.GetId()))
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
                StatusMessage = $"Greška: {ex.Message}";
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
                if (rezervacijaService.RejectRezervacija(SelectedRezervacija.GetId(), razlog))
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
                StatusMessage = $"Greška: {ex.Message}";
            }
        }

        private void ClearApartmanForm()
        {
            ApartmanIme = string.Empty;
            ApartmanOpis = string.Empty;
            ApartmanBrojSoba = 1;
            ApartmanMaxGostiju = 2;
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
