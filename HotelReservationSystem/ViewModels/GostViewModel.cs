using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HotelReservationSystem.Helpers;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels
{
    public class GostViewModel : ViewModelBase
    {
        private ObservableCollection<Hotel> hoteli = new();
        private ObservableCollection<Apartman> apartmani = new();
        private ObservableCollection<Rezervacija> rezervacije = new();
        private Hotel? selectedHotel;
        private Apartman? selectedApartman;
        private Rezervacija? selectedRezervacija;
        private DateTime datumOd = DateTime.Today;
        private DateTime datumDo = DateTime.Today.AddDays(1);
        private string searchText = string.Empty;
        private string statusMessage = string.Empty;
        private string statusFilter = "Sve";

        public ObservableCollection<Hotel> Hoteli
        {
            get => hoteli;
            set => SetProperty(ref hoteli, value);
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

        public Apartman? SelectedApartman
        {
            get => selectedApartman;
            set => SetProperty(ref selectedApartman, value);
        }

        public Rezervacija? SelectedRezervacija
        {
            get => selectedRezervacija;
            set => SetProperty(ref selectedRezervacija, value);
        }

        public DateTime DatumOd
        {
            get => datumOd;
            set => SetProperty(ref datumOd, value);
        }

        public DateTime DatumDo
        {
            get => datumDo;
            set => SetProperty(ref datumDo, value);
        }

        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
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
        public ICommand SearchCommand { get; }
        public ICommand CreateRezervacijaCommand { get; }
        public ICommand LoadRezervacijeCommand { get; }
        public ICommand CancelRezervacijaCommand { get; }
        public ICommand LogoutCommand { get; }

        public GostViewModel()
        {
            LoadHoteliCommand = new RelayCommand(_ => LoadHoteli());
            SearchCommand = new RelayCommand(_ => SearchHoteli());
            CreateRezervacijaCommand = new RelayCommand(_ => CreateRezervacija());
            LoadRezervacijeCommand = new RelayCommand(_ => LoadRezervacije());
            CancelRezervacijaCommand = new RelayCommand(_ => CancelRezervacija());
            LogoutCommand = new RelayCommand(_ => Logout());

            LoadHoteli();
        }

        private void LoadHoteli()
        {
            try
            {
                var hotelService = ServiceLocator.Instance.HotelService;
                var allHotels = hotelService.GetApprovedHotels();
                
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

        private void SearchHoteli()
        {
            try
            {
                var hotelService = ServiceLocator.Instance.HotelService;
                var results = hotelService.SearchHotelsByName(SearchText);
                
                Hoteli.Clear();
                foreach (var hotel in results)
                {
                    Hoteli.Add(hotel);
                }
                
                StatusMessage = $"Pronađeno {Hoteli.Count} hotela";
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

        private void CreateRezervacija()
        {
            try
            {
                if (SelectedApartman == null)
                {
                    MessageBox.Show("Izaberite apartman", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (DatumDo < DatumOd)
                {
                    MessageBox.Show("Datum kraja ne može biti pre datuma početka", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var currentUser = ServiceLocator.Instance.AuthService.GetCurrentUser();
                if (currentUser == null)
                    return;

                var rezervacijaService = ServiceLocator.Instance.RezervacijaService;
                
                var novaRezervacija = new Rezervacija(
                    Guid.NewGuid().ToString(),
                    currentUser.GetJmbg(),
                    SelectedApartman.GetId(),
                    DatumOd,
                    DatumDo,
                    StatusRezervacije.NaCekanju,
                    string.Empty
                );

                if (rezervacijaService.CreateRezervacija(novaRezervacija))
                {
                    MessageBox.Show("Rezervacija je uspešno kreirana i čeka odobrenje vlasnika", "Uspeh", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadRezervacije();
                }
                else
                {
                    MessageBox.Show("Nije moguće kreirati rezervaciju", "Greška", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
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
                var allRezervacije = rezervacijaService.GetRezervacijeByGost(currentUser.GetJmbg());

                var filtered = StatusFilter switch
                {
                    "Na čekanju" => allRezervacije.Where(r => r.GetStatus() == StatusRezervacije.NaCekanju).ToList(),
                    "Potvrđeno" => allRezervacije.Where(r => r.GetStatus() == StatusRezervacije.Potvrdjeno).ToList(),
                    "Odbijeno" => allRezervacije.Where(r => r.GetStatus() == StatusRezervacije.Odbijeno).ToList(),
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

        private void CancelRezervacija()
        {
            try
            {
                if (SelectedRezervacija == null)
                {
                    MessageBox.Show("Izaberite rezervaciju", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show("Da li ste sigurni da želite da otkažete rezervaciju?", 
                                            "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    var rezervacijaService = ServiceLocator.Instance.RezervacijaService;
                    if (rezervacijaService.CancelRezervacija(SelectedRezervacija.GetId()))
                    {
                        MessageBox.Show("Rezervacija je otkazana", "Uspeh", 
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadRezervacije();
                    }
                    else
                    {
                        MessageBox.Show("Nije moguće otkazati rezervaciju", "Greška", 
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
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
