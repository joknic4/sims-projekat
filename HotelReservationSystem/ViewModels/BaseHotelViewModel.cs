using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HotelReservationSystem.Helpers;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels
{
    public abstract class BaseHotelViewModel : ViewModelBase
    {
        protected ObservableCollection<Hotel> sviHoteli = new();
        protected ObservableCollection<Apartman> apartmaniZaPrikaz = new();
        protected Hotel? selectedHotelZaPrikaz;
        protected Apartman? selectedApartmanZaPrikaz;
        protected string searchText = string.Empty;
        protected string searchCriteria = "Ime";
        protected string sortOrder = "Rastuće";
        protected string statusMessage = string.Empty;
        
        // Liste opcija za ComboBox
        public ObservableCollection<string> SearchCriteriaOptions { get; } = new()
        {
            "Ime", "Godina", "Zvezdice", "Broj soba", "Broj gostiju", "Sobe + Gosti"
        };
        
        public ObservableCollection<string> SortOrderOptions { get; } = new()
        {
            "Rastuće", "Opadajuće"
        };

        public ObservableCollection<Hotel> SviHoteli
        {
            get => sviHoteli;
            set => SetProperty(ref sviHoteli, value);
        }

        public ObservableCollection<Apartman> ApartmaniZaPrikaz
        {
            get => apartmaniZaPrikaz;
            set => SetProperty(ref apartmaniZaPrikaz, value);
        }

        public Hotel? SelectedHotelZaPrikaz
        {
            get => selectedHotelZaPrikaz;
            set
            {
                if (SetProperty(ref selectedHotelZaPrikaz, value) && value != null)
                {
                    LoadApartmaniZaPrikaz(value.GetSifra());
                }
            }
        }

        public Apartman? SelectedApartmanZaPrikaz
        {
            get => selectedApartmanZaPrikaz;
            set => SetProperty(ref selectedApartmanZaPrikaz, value);
        }

        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        public string SearchCriteria
        {
            get => searchCriteria;
            set
            {
                if (SetProperty(ref searchCriteria, value))
                {
                    StatusMessage = $"Kriterijum: {value}";
                }
            }
        }

        public string SortOrder
        {
            get => sortOrder;
            set
            {
                if (SetProperty(ref sortOrder, value))
                {
                    StatusMessage = $"Redosled: {value}";
                }
            }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public ICommand LoadSviHoteliCommand { get; }
        public ICommand SearchHoteliCommand { get; }
        public ICommand SortHoteliCommand { get; }

        public BaseHotelViewModel()
        {
            LoadSviHoteliCommand = new RelayCommand(_ => LoadSviHoteli());
            SearchHoteliCommand = new RelayCommand(_ => SearchHoteli());
            SortHoteliCommand = new RelayCommand(_ => SortHoteli());
        }

        protected virtual void LoadSviHoteli()
        {
            try
            {
                var hotelService = ServiceLocator.Instance.HotelService;
                var allHotels = hotelService.GetApprovedHotels();
                
                SviHoteli.Clear();
                foreach (var hotel in allHotels)
                {
                    SviHoteli.Add(hotel);
                }
                
                StatusMessage = $"Učitano {SviHoteli.Count} hotela";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
        }

        protected virtual void SearchHoteli()
        {
            try
            {
                var hotelService = ServiceLocator.Instance.HotelService;
                
                if (string.IsNullOrWhiteSpace(SearchText) && SearchCriteria != "Apartmani")
                {
                    LoadSviHoteli();
                    return;
                }

                var results = SearchCriteria switch
                {
                    "Ime" => hotelService.SearchHotelsByName(SearchText),
                    "Godina" => int.TryParse(SearchText, out int godina) 
                        ? hotelService.SearchHotelsByYear(godina) 
                        : new System.Collections.Generic.List<Hotel>(),
                    "Zvezdice" => int.TryParse(SearchText, out int zvezdice) 
                        ? hotelService.SearchHotelsByStars(zvezdice) 
                        : new System.Collections.Generic.List<Hotel>(),
                    "Broj soba" => SearchByRooms(),
                    "Broj gostiju" => SearchByGuests(),
                    "Sobe + Gosti" => SearchByRoomsAndGuests(),
                    _ => hotelService.GetApprovedHotels()
                };
                
                SviHoteli.Clear();
                foreach (var hotel in results)
                {
                    SviHoteli.Add(hotel);
                }
                
                StatusMessage = $"Pronađeno {SviHoteli.Count} hotela (kriterijum: {SearchCriteria})";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
        }

        private System.Collections.Generic.List<Hotel> SearchByRoomsAndGuests()
        {
            var hotelService = ServiceLocator.Instance.HotelService;
            
            var searchParts = SearchText.Split(new[] { '&', '|' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (searchParts.Length == 0)
                return hotelService.GetApprovedHotels();
            
            int? brojSoba = null;
            int? brojGostiju = null;
            string logickiOperator = "";
            
            if (searchParts.Length == 1)
            {
                if (int.TryParse(searchParts[0].Trim(), out int broj))
                {
                    brojSoba = broj;
                }
            }
            else if (searchParts.Length == 2)
            {
                if (int.TryParse(searchParts[0].Trim(), out int broj1) &&
                    int.TryParse(searchParts[1].Trim(), out int broj2))
                {
                    brojSoba = broj1;
                    brojGostiju = broj2;
                    logickiOperator = SearchText.Contains("&") ? "&" : "|";
                }
            }
            
            return hotelService.SearchHotelsByApartments(brojSoba, brojGostiju, logickiOperator);
        }

        private System.Collections.Generic.List<Hotel> SearchByRooms()
        {
            var hotelService = ServiceLocator.Instance.HotelService;
            
            if (int.TryParse(SearchText.Trim(), out int brojSoba))
            {
                return hotelService.SearchHotelsByApartments(brojSoba, null, "");
            }
            
            return hotelService.GetApprovedHotels();
        }

        private System.Collections.Generic.List<Hotel> SearchByGuests()
        {
            var hotelService = ServiceLocator.Instance.HotelService;
            
            if (int.TryParse(SearchText.Trim(), out int brojGostiju))
            {
                return hotelService.SearchHotelsByApartments(null, brojGostiju, "");
            }
            
            return hotelService.GetApprovedHotels();
        }

        protected virtual void SortHoteli()
        {
            try
            {
                var hotelService = ServiceLocator.Instance.HotelService;
                var currentList = SviHoteli.ToList();
                
                bool ascending = SortOrder == "Rastuće";
                var sorted = hotelService.SortHotelsByStars(currentList, ascending);
                
                SviHoteli.Clear();
                foreach (var hotel in sorted)
                {
                    SviHoteli.Add(hotel);
                }
                
                StatusMessage = $"Sortirano {SviHoteli.Count} hotela ({SortOrder})";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
        }

        protected void LoadApartmaniZaPrikaz(string sifraHotela)
        {
            try
            {
                var apartmanService = ServiceLocator.Instance.ApartmanService;
                var allApartmani = apartmanService.GetApartmaniByHotel(sifraHotela);
                
                ApartmaniZaPrikaz.Clear();
                foreach (var apartman in allApartmani)
                {
                    ApartmaniZaPrikaz.Add(apartman);
                }
                
                StatusMessage = $"Učitano {ApartmaniZaPrikaz.Count} apartmana";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
        }
    }
}
