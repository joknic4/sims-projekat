using System;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;
using HotelReservationSystem.Services;
using HotelReservationSystem.UI;

namespace HotelReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inicijalizacija
            var korisnikRepo = new KorisnikRepository();
            var hotelRepo = new HotelRepository();
            
            var authService = new AuthService(korisnikRepo);
            var korisnikService = new KorisnikService(korisnikRepo);
            var hotelService = new HotelService(hotelRepo);
            
            // Dodaj test podatke ako ne postoje
            if (korisnikRepo.GetAll().Count == 0)
            {
                var admin = new Korisnik("1234567890123", "admin@hotel.com", "admin", 
                                         "Admin", "Adminovic", "0641111111", KorisnikTip.Administrator);
                korisnikRepo.Add(admin);
                korisnikRepo.Save();
            }
            
            if (hotelRepo.GetAll().Count == 0)
            {
                hotelService.DodajHotel("H001", "Grand Hotel Kragujevac", 2020, 5, "1234567890123");
                hotelService.DodajHotel("H002", "Hotel Šumarice", 2018, 4, "1234567890123");
                hotelService.DodajHotel("H003", "Hotel Park", 2015, 3, "1234567890123");
            }
            
            // Pokreni aplikaciju
            var meni = new KonzolniMeni(authService, korisnikService, hotelService);
            meni.Pokreni();
        }
    }
}
