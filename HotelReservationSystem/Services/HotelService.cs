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
            return hotelRepository.GetAll()
                .Where(h => h.GetIme().ToLower().Contains(ime.ToLower()))
                .ToList();
        }
        
        public List<Hotel> PretraziPoZvezdicama(int zvezdice)
        {
            return hotelRepository.GetAll()
                .Where(h => h.GetBrojZvezdica() == zvezdice)
                .ToList();
        }
    }
}
