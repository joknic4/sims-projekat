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
            // Inicijalizacija repozitorijuma
            var korisnikRepo = new KorisnikRepository();
            var hotelRepo = new HotelRepository();
            var apartmanRepo = new ApartmanRepository();
            var rezervacijaRepo = new RezervacijaRepository();
            
            // Inicijalizacija servisa
            var authService = new AuthService(korisnikRepo);
            var korisnikService = new KorisnikService(korisnikRepo);
            var hotelService = new HotelService(hotelRepo);
            var apartmanService = new ApartmanService(apartmanRepo);
            var rezervacijaService = new RezervacijaService(rezervacijaRepo);
            
            // Dodaj test podatke ako ne postoje
            if (korisnikRepo.GetAll().Count == 0)
            {
                var admin = new Korisnik("1234567890123", "admin@hotel.com", "admin", 
                                         "Admin", "Adminovic", "0641111111", KorisnikTip.Administrator);
                korisnikRepo.Add(admin);
                
                var vlasnik = new Korisnik("9999999999999", "vlasnik@hotel.com", "vlasnik", 
                                          "Petar", "Petrovic", "0642222222", KorisnikTip.Vlasnik);
                korisnikRepo.Add(vlasnik);
                
                korisnikRepo.Save();
            }
            
            if (hotelRepo.GetAll().Count == 0)
            {
                hotelService.DodajHotel("H001", "Grand Hotel Kragujevac", 2020, 5, "9999999999999");
                hotelService.DodajHotel("H002", "Hotel Šumarice", 2018, 4, "9999999999999");
                hotelService.DodajHotel("H003", "Hotel Park", 2015, 3, "9999999999999");
            }
            
            if (apartmanRepo.GetAll().Count == 0)
            {
                apartmanService.DodajApartman("Apartman A1", "Luksuzni apartman sa pogledom", 3, 6, "H001");
                apartmanService.DodajApartman("Apartman A2", "Komforan porodicni apartman", 2, 4, "H001");
                apartmanService.DodajApartman("Apartman B1", "Studio apartman", 1, 2, "H002");
            }
            
            // Pokreni aplikaciju
            var meni = new KonzolniMeni(authService, korisnikService, hotelService);
            meni.Pokreni();
        }
    }
}
