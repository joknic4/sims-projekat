using System.Collections.Generic;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public interface IHotelRepository
    {
        List<Hotel> GetAll();
        Hotel? GetBySifra(string sifra);
        void Add(Hotel hotel);
        void Update(Hotel hotel);
        List<Hotel> GetByVlasnik(string jmbgVlasnika);
        void Save();
    }
}
