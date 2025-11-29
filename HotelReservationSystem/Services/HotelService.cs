using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class HotelService
    {
        private readonly IHotelRepository hotelRepository;
        
        public HotelService(IHotelRepository repository)
        {
            hotelRepository = repository;
        }
        
        public List<Hotel> GetSviHoteli()
        {
            return hotelRepository.GetAll();
        }
        
        public void DodajHotel(string sifra, string ime, int godina, int zvezdice, string jmbgVlasnika)
        {
            var hotel = new Hotel(sifra, ime, godina, zvezdice, jmbgVlasnika);
            hotelRepository.Add(hotel);
            hotelRepository.Save();
        }
        
        public List<Hotel> PretraziPoImenu(string ime)
        {
            var sviHoteli = hotelRepository.GetAll().ToList();
            var rezultat = new List<Hotel>();
            
            foreach (var hotel in sviHoteli)
            {
                if (hotel.GetIme().ToLower().Contains(ime.ToLower()))
                {
                    rezultat.Add(hotel);
                }
            }
            
            return rezultat.ToList();
        }
        
        public List<Hotel> PretraziPoZvezdicama(int zvezdice)
        {
            var rezultat = hotelRepository.GetAll()
                .Where(h => h.GetBrojZvezdica() == zvezdice)
                .ToList();
            
            rezultat.Sort((a, b) => string.Compare(a.GetIme(), b.GetIme(), StringComparison.Ordinal));
            
            return rezultat;
        }
        
        public void OdobriHotel(string sifra)
        {
            var sviHoteli = hotelRepository.GetAll();
            
            foreach (var h in sviHoteli)
            {
                if (h.GetSifra() == sifra)
                {
                    h.SetStatus(StatusHotela.Odobren);
                    hotelRepository.Save();
                    break;
                }
            }
        }
        
        public List<Hotel> GetHotelsByVlasnik(string jmbgVlasnika)
        {
            var sviHoteli = hotelRepository.GetAll();
            var rezultat = new List<Hotel>();
            
            for (int i = 0; i < sviHoteli.Count; i++)
            {
                var hotel = sviHoteli[i];
                if (hotel.GetJmbgVlasnika() == jmbgVlasnika)
                {
                    rezultat.Add(hotel);
                }
            }
            
            return rezultat.ToList();
        }
    }
}
