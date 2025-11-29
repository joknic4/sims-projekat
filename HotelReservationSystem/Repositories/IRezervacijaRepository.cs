using System.Collections.Generic;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public interface IRezervacijaRepository
    {
        List<Rezervacija> GetAll();
        List<Rezervacija> GetByGost(string jmbgGosta);
        List<Rezervacija> GetByStatus(StatusRezervacije status);
        Rezervacija? GetById(string id);
        void Add(Rezervacija rezervacija);
        void Save();
    }
}
