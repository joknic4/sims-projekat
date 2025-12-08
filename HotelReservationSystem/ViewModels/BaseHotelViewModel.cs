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
        protected string searchText = string.Empty;
        protected string searchCriteria = "Ime";
        protected string sortOrder = "Rastuće";
        protected string statusMessage = string.Empty;

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

        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        public string SearchCriteria
        {
            get => searchCriteria;
            set => SetProperty(ref searchCriteria, value);
        }

        public string SortOrder
        {
            get => sortOrder;
            set => SetProperty(ref sortOrder, value);
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
                var apartmanService = ServiceLocator.Instance.ApartmanService;
                
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
                    "Apartmani" => SearchByApartments(),
                    _ => hotelService.GetApprovedHotels()
                };
                
                SviHoteli.Clear();
                foreach (var hotel in results)
                {
                    SviHoteli.Add(hotel);
                }
                
                StatusMessage = $"Pronađeno {SviHoteli.Count} hotela";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
        }

        private System.Collections.Generic.List<Hotel> SearchByApartments()
        {
            var hotelService = ServiceLocator.Instance.HotelService;
            
            // Format može biti: "2" (samo sobe), "3" (samo gostiju), "2 & 3" (AND), "2 | 3" (OR)
            var searchParts = SearchText.Split(new[] { '&', '|' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (searchParts.Length == 0)
                return hotelService.GetApprovedHotels();
            
            int? brojSoba = null;
            int? brojGostiju = null;
            string logickiOperator = "";
            
            if (searchParts.Length == 1)
            {
                // Samo jedan broj - pretpostavljamo da je broj soba
                if (int.TryParse(searchParts[0].Trim(), out int broj))
                {
                    brojSoba = broj;
                }
            }
            else if (searchParts.Length == 2)
            {
                // Dva broja sa operatorom
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
                
                StatusMessage = $"Sortirano {SviHoteli.Count} hotela po broju zvezdica ({SortOrder.ToLower()})";
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
            }
            catch (Exception ex)
            {
                StatusMessage = $"Greška: {ex.Message}";
            }
        }
    }
}
