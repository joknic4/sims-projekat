using System.Collections.Generic;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetAll();
        Korisnik? GetByJmbg(string jmbg);
        Korisnik? GetByEmail(string email);
        void Add(Korisnik korisnik);
        void Save();
    }
}
