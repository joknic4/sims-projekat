using System.Collections.Generic;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public interface IRezervacijaRepository
    {
        List<Rezervacija> GetAll();
        Rezervacija? GetById(string id);
        List<Rezervacija> GetByGost(string jmbgGosta);
        List<Rezervacija> GetByApartman(string idApartmana);
        bool Add(Rezervacija rezervacija);
        bool Update(Rezervacija rezervacija);
        bool Delete(string id);
        void SaveToFile();
        void LoadFromFile();
    }
}
