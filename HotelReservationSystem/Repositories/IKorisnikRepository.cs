using System.Collections.Generic;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetAll();
        Korisnik? GetByJmbg(string jmbg);
        Korisnik? GetByEmail(string email);
        bool Add(Korisnik korisnik);
        bool Update(Korisnik korisnik);
        bool Delete(string jmbg);
        void SaveToFile();
        void LoadFromFile();
    }
}
