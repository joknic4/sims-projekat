using System;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;
using HotelReservationSystem.Services;

namespace HotelReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hotel Reservation System - v4");
            Console.WriteLine("==============================\n");
            
            // Inicijalizacija
            var korisnikRepo = new KorisnikRepository();
            var hotelRepo = new HotelRepository();
            
            var authService = new AuthService(korisnikRepo);
            var korisnikService = new KorisnikService(korisnikRepo);
            var hotelService = new HotelService(hotelRepo);
            
            // Test registracije
            Console.WriteLine("Registracija novog gosta...");
            bool uspesno = korisnikService.RegistrujGosta(
                "9876543210987", "marko@email.com", "sifra123",
                "Marko", "Markovic", "0641234567");
            
            if (uspesno)
                Console.WriteLine("Gost uspesno registrovan!\n");
            
            // Test logina
            Console.WriteLine("Pokusaj prijavljivanja...");
            if (authService.Login("marko@email.com", "sifra123"))
            {
                var korisnik = authService.GetTrenutniKorisnik();
                Console.WriteLine($"Uspesno prijavljen: {korisnik?.GetIme()} {korisnik?.GetPrezime()}\n");
            }
            
            // Test dodavanja hotela
            hotelService.DodajHotel("H001", "Grand Hotel", 2020, 5, "1234567890123");
            hotelService.DodajHotel("H002", "Hotel Kragujevac", 2018, 4, "1234567890123");
            
            Console.WriteLine("--- Svi hoteli ---");
            foreach (var h in hotelService.GetSviHoteli())
            {
                Console.WriteLine($"{h.GetIme()} - {h.GetBrojZvezdica()} zvezdica");
            }
        }
    }
}
