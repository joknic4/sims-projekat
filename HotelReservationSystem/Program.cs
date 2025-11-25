using System;
using HotelReservationSystem.Models;

namespace HotelReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hotel Reservation System - v2");
            Console.WriteLine("==============================");
            
            // Test enkapsuliranih modela
            var korisnik = new Korisnik("1234567890123", "marko@email.com", "sifra123", 
                                        "Marko", "Markovic", "0641234567", KorisnikTip.Gost);
            
            Console.WriteLine($"Korisnik: {korisnik.GetIme()} {korisnik.GetPrezime()}");
            Console.WriteLine($"Email: {korisnik.GetEmail()}");
            Console.WriteLine($"Tip: {korisnik.GetTipKorisnika()}");
            
            var hotel = new Hotel("H001", "Grand Hotel", 2020, 5, "9876543210987");
            Console.WriteLine($"\nHotel: {hotel.GetIme()}");
            Console.WriteLine($"Zvezdice: {hotel.GetBrojZvezdica()}");
            Console.WriteLine($"Godina: {hotel.GetGodinaIzgradnje()}");
            
            var apartman = new Apartman("A101", "Luksuzni apartman", 3, 6, "H001");
            Console.WriteLine($"\nApartman: {apartman.GetIme()}");
            Console.WriteLine($"Broj soba: {apartman.GetBrojSoba()}");
            Console.WriteLine($"Max gostiju: {apartman.GetMaxBrojGostiju()}");
        }
    }
}
