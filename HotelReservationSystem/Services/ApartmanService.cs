using System.Collections.Generic;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class ApartmanService
    {
        private readonly IApartmanRepository apartmanRepository;
        
        public ApartmanService(IApartmanRepository repository)
        {
            apartmanRepository = repository;
        }
        
        public void DodajApartman(string ime, string opis, int brojSoba, int maxGostiju, string sifraHotela)
        {
            var apartman = new Apartman(ime, opis, brojSoba, maxGostiju, sifraHotela);
            apartmanRepository.Add(apartman);
            apartmanRepository.Save();
        }
        
        public List<Apartman> GetApartmaniHotela(string sifraHotela)
        {
            return apartmanRepository.GetByHotel(sifraHotela);
        }
    }
}
