using System;
using System.Linq;
using HotelReservationSystem.Models;
using HotelReservationSystem.Services;

namespace HotelReservationSystem.Helpers
{
    public class InitialDataSeeder
    {
        public static void SeedData()
        {
            try
            {
                var korisnikService = ServiceLocator.Instance.KorisnikService;
                
                // Provera da li već postoji administrator
                var existingAdmins = korisnikService.GetAllKorisnici()
                    .Where(k => k.GetTipKorisnika() == KorisnikTip.Administrator)
                    .ToList();

                if (existingAdmins.Count == 0)
                {
                    // Kreiranje inicijalnog administratora
                    var admin = new Korisnik(
                        "1234567890123",  // JMBG
                        "admin@hotel.rs",  // Email
                        "admin123",        // Lozinka
                        "Admin",           // Ime
                        "Adminović",       // Prezime
                        "0601234567",      // Telefon
                        KorisnikTip.Administrator
                    );

                    korisnikService.AddKorisnik(admin);
                }

                // Kreiranje test korisnika za demonstraciju
                SeedTestData(korisnikService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri inicijalizaciji podataka: {ex.Message}");
            }
        }

        private static void SeedTestData(KorisnikService korisnikService)
        {
            try
            {
                var allKorisnici = korisnikService.GetAllKorisnici();

                // Test vlasnik
                if (!allKorisnici.Any(k => k.GetEmail() == "vlasnik@hotel.rs"))
                {
                    var vlasnik = new Korisnik(
                        "1111111111111",
                        "vlasnik@hotel.rs",
                        "vlasnik123",
                        "Petar",
                        "Petrović",
                        "0611111111",
                        KorisnikTip.Vlasnik
                    );
                    korisnikService.AddKorisnik(vlasnik);
                }

                // Test gost
                if (!allKorisnici.Any(k => k.GetEmail() == "gost@hotel.rs"))
                {
                    var gost = new Korisnik(
                        "2222222222222",
                        "gost@hotel.rs",
                        "gost123",
                        "Marko",
                        "Marković",
                        "0622222222",
                        KorisnikTip.Gost
                    );
                    korisnikService.AddKorisnik(gost);
                }

                // Test podaci za hotel i apartmane
                var hotelService = ServiceLocator.Instance.HotelService;
                var allHotels = hotelService.GetAllHotels();

                if (allHotels.Count == 0)
                {
                    var testHotel = new Hotel(
                        "HOT0001",
                        "Grand Hotel Kragujevac",
                        2020,
                        5,
                        "1111111111111",
                        StatusHotela.Odobren
                    );
                    hotelService.AddHotel(testHotel);

                    // Dodavanje test apartmana
                    var apartmanService = ServiceLocator.Instance.ApartmanService;
                    var apartman1 = new Apartman(
                        Guid.NewGuid().ToString(),
                        "Lux apartman",
                        "Luksuzni apartman sa pogledom na grad",
                        2,
                        4,
                        "HOT0001"
                    );
                    apartmanService.AddApartman(apartman1);

                    var apartman2 = new Apartman(
                        Guid.NewGuid().ToString(),
                        "Standard apartman",
                        "Komforan apartman za porodicu",
                        1,
                        2,
                        "HOT0001"
                    );
                    apartmanService.AddApartman(apartman2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri dodavanju test podataka: {ex.Message}");
            }
        }
    }
}
