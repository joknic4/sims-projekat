using System.Collections.Generic;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public interface IApartmanRepository
    {
        List<Apartman> GetAll();
        List<Apartman> GetByHotel(string sifraHotela);
        void Add(Apartman apartman);
        void Save();
    }
}
