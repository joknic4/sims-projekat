using System;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hotel Reservation System - v3");
            Console.WriteLine("==============================\n");
            
            // Test Repository pattern
            var korisnikRepo = new KorisnikRepository();
            var hotelRepo = new HotelRepository();
            
            // Dodaj test korisnika
            var admin = new Korisnik("1234567890123", "admin@hotel.com", "admin123", 
                                     "Admin", "Adminovic", "0641111111", KorisnikTip.Administrator);
            korisnikRepo.Add(admin);
            korisnikRepo.Save();
            
            Console.WriteLine("Dodat admin korisnik i sacuvan u JSON");
            
            // Dodaj test hotel
            var hotel = new Hotel("H001", "Grand Hotel Kragujevac", 2020, 5, "1234567890123");
            hotelRepo.Add(hotel);
            hotelRepo.Save();
            
            Console.WriteLine("Dodat hotel i sacuvan u JSON");
            
            // Ucitaj i prikazi
            Console.WriteLine("\n--- Ucitani korisnici ---");
            foreach (var k in korisnikRepo.GetAll())
            {
                Console.WriteLine($"{k.GetIme()} {k.GetPrezime()} - {k.GetTipKorisnika()}");
            }
            
            Console.WriteLine("\n--- Ucitani hoteli ---");
            foreach (var h in hotelRepo.GetAll())
            {
                Console.WriteLine($"{h.GetIme()} - {h.GetBrojZvezdica()} zvezdica");
            }
        }
    }
}
