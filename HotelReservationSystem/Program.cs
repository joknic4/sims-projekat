using System;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;
using HotelReservationSystem.Services;

namespace HotelReservationSystem
{
    // NAPOMENA: WPF aplikacija se pokreće preko App.xaml (StartupUri="Views/LoginWindow.xaml")
    // Ova klasa služi samo za inicijalizaciju test podataka
    class Program
    {
        static void Main(string[] args)
        {
            // Inicijalizacija repozitorijuma
            var korisnikRepo = new KorisnikRepository();
            var hotelRepo = new HotelRepository();
            var apartmanRepo = new ApartmanRepository();
            
            // Dodaj test podatke ako ne postoje
            if (korisnikRepo.GetAll().Count == 0)
            {
                var admin = new Korisnik("1234567890123", "admin@hotel.com", "admin", 
                                         "Admin", "Adminovic", "0641111111", KorisnikTip.Administrator);
                korisnikRepo.Add(admin);
                
                var vlasnik = new Korisnik("9999999999999", "vlasnik@hotel.com", "vlasnik", 
                                          "Petar", "Petrovic", "0642222222", KorisnikTip.Vlasnik);
                korisnikRepo.Add(vlasnik);
                
                var gost = new Korisnik("5555555555555", "gost@hotel.com", "gost", 
                                       "Ana", "Anić", "0643333333", KorisnikTip.Gost);
                korisnikRepo.Add(gost);
                
                korisnikRepo.Save();
            }
            
            if (hotelRepo.GetAll().Count == 0)
            {
                var hotelService = new HotelService(hotelRepo);
                hotelService.DodajHotel("H001", "Grand Hotel Kragujevac", 2020, 5, "9999999999999");
                hotelService.DodajHotel("H002", "Hotel Šumarice", 2018, 4, "9999999999999");
                hotelService.DodajHotel("H003", "Hotel Park", 2015, 3, "9999999999999");
                
                // Odmah odobrimo hotele
                hotelService.OdobriHotel("H001");
                hotelService.OdobriHotel("H002");
            }
            
            if (apartmanRepo.GetAll().Count == 0)
            {
                var apartmanService = new ApartmanService(apartmanRepo);
                apartmanService.DodajApartman("Apartman A1", "Luksuzni apartman sa pogledom", 3, 6, "H001");
                apartmanService.DodajApartman("Apartman A2", "Komforan porodični apartman", 2, 4, "H001");
                apartmanService.DodajApartman("Apartman B1", "Studio apartman", 1, 2, "H002");
            }
        }
    }
}
