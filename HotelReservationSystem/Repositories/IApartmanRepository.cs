using System.Collections.Generic;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public interface IApartmanRepository
    {
        List<Apartman> GetAll();
        Apartman? GetById(string id);
        List<Apartman> GetByHotel(string sifraHotela);
        bool Add(Apartman apartman);
        bool Update(Apartman apartman);
        bool Delete(string id);
        void SaveToFile();
        void LoadFromFile();
    }
}
