using System.Collections.Generic;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public interface IHotelRepository
    {
        List<Hotel> GetAll();
        Hotel? GetBySifra(string sifra);
        List<Hotel> GetByVlasnik(string jmbgVlasnika);
        bool Add(Hotel hotel);
        bool Update(Hotel hotel);
        bool Delete(string sifra);
        void SaveToFile();
        void LoadFromFile();
    }
}
